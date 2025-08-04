using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text.Json;

namespace AdvancedCopilotAPI.Handlers
{
    public class GlobalExceptionHandler : IExceptionHandler
    {
        private readonly ILogger<GlobalExceptionHandler> _logger;
        private readonly IWebHostEnvironment _environment;
        
        public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger, IWebHostEnvironment environment)
        {
            _logger = logger;
            _environment = environment;
        }
        
        public async ValueTask<bool> TryHandleAsync(
            HttpContext httpContext, 
            Exception exception, 
            CancellationToken cancellationToken)
        {
            var correlationId = Guid.NewGuid().ToString();
            
            _logger.LogError(exception, 
                "An unhandled exception occurred. CorrelationId: {CorrelationId}, Path: {Path}", 
                correlationId, httpContext.Request.Path);
            
            var (statusCode, title, message) = CategorizeException(exception);
            
            var problemDetails = new ProblemDetails
            {
                Status = statusCode,
                Title = title,
                Detail = _environment.IsDevelopment() ? exception.Message : message,
                Instance = httpContext.Request.Path,
                Extensions = 
                {
                    ["correlationId"] = correlationId,
                    ["timestamp"] = DateTime.UtcNow
                }
            };
            
            if (_environment.IsDevelopment())
            {
                problemDetails.Extensions["stackTrace"] = exception.StackTrace;
            }
            
            if (IsRateLimitingException(exception))
            {
                httpContext.Response.Headers["Retry-After"] = "60";
            }
            
            httpContext.Response.StatusCode = statusCode;
            httpContext.Response.ContentType = "application/problem+json";
            
            var json = JsonSerializer.Serialize(problemDetails, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            });
            
            await httpContext.Response.WriteAsync(json, cancellationToken);
            
            return true;
        }
        
        private static (int statusCode, string title, string message) CategorizeException(Exception exception)
        {
            return exception switch
            {
                ArgumentNullException => (400, "Bad Request", "Required parameter is missing"),
                ArgumentException => (400, "Bad Request", "Invalid parameter value"),
                UnauthorizedAccessException => (401, "Unauthorized", "Authentication required"),
                KeyNotFoundException => (404, "Not Found", "The requested resource was not found"),
                InvalidOperationException => (409, "Conflict", "The operation cannot be completed due to a conflict"),
                NotSupportedException => (422, "Unprocessable Entity", "The request is not supported"),
                TimeoutException => (408, "Request Timeout", "The request timed out"),
                _ => (500, "Internal Server Error", "An unexpected error occurred")
            };
        }
        
        private static bool IsRateLimitingException(Exception exception)
        {
            return exception.Message.Contains("rate limit", StringComparison.OrdinalIgnoreCase) ||
                   exception.Message.Contains("too many requests", StringComparison.OrdinalIgnoreCase);
        }
    }
}