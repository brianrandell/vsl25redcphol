namespace ProductInventoryAPI.Middleware;

public class AuditMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<AuditMiddleware> _logger;

    public AuditMiddleware(RequestDelegate next, ILogger<AuditMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        // Log the incoming request for audit purposes
        var method = context.Request.Method;
        var path = context.Request.Path;
        var userAgent = context.Request.Headers["User-Agent"].FirstOrDefault();
        var ipAddress = context.Connection.RemoteIpAddress?.ToString();
        
        _logger.LogInformation("Audit: {Method} {Path} from {IpAddress} using {UserAgent}", 
            method, path, ipAddress, userAgent);

        // Add default user information if not present for audit trail
        if (!context.Request.Headers.ContainsKey("X-User-Id"))
        {
            context.Request.Headers["X-User-Id"] = "demo-user";
        }
        
        if (!context.Request.Headers.ContainsKey("X-User-Name"))
        {
            context.Request.Headers["X-User-Name"] = "Demo User";
        }

        await _next(context);
    }
}