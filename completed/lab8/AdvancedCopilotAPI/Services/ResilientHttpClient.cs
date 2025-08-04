using Polly;
using Polly.Extensions.Http;
using Polly.Wrap;

namespace AdvancedCopilotAPI.Services
{
    public interface IResilientHttpClient
    {
        Task<HttpResponseMessage> GetAsync(string requestUri, CancellationToken cancellationToken = default);
        Task<HttpResponseMessage> PostAsync(string requestUri, HttpContent content, CancellationToken cancellationToken = default);
        Task<HttpResponseMessage> PutAsync(string requestUri, HttpContent content, CancellationToken cancellationToken = default);
        Task<HttpResponseMessage> DeleteAsync(string requestUri, CancellationToken cancellationToken = default);
        Task<string> GetStringAsync(string requestUri, CancellationToken cancellationToken = default);
    }
    
    public class ResilientHttpClient : IResilientHttpClient
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<ResilientHttpClient> _logger;
        private readonly AsyncPolicyWrap<HttpResponseMessage> _resilientPolicy;
        
        public ResilientHttpClient(HttpClient httpClient, ILogger<ResilientHttpClient> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
            _resilientPolicy = CreateResilientPolicy();
        }
        
        public async Task<HttpResponseMessage> GetAsync(string requestUri, CancellationToken cancellationToken = default)
        {
            return await _resilientPolicy.ExecuteAsync(async (ct) =>
            {
                _logger.LogDebug("Executing GET request to {Uri}", requestUri);
                return await _httpClient.GetAsync(requestUri, ct);
            }, cancellationToken);
        }
        
        public async Task<HttpResponseMessage> PostAsync(string requestUri, HttpContent content, CancellationToken cancellationToken = default)
        {
            return await _resilientPolicy.ExecuteAsync(async (ct) =>
            {
                _logger.LogDebug("Executing POST request to {Uri}", requestUri);
                return await _httpClient.PostAsync(requestUri, content, ct);
            }, cancellationToken);
        }
        
        public async Task<HttpResponseMessage> PutAsync(string requestUri, HttpContent content, CancellationToken cancellationToken = default)
        {
            return await _resilientPolicy.ExecuteAsync(async (ct) =>
            {
                _logger.LogDebug("Executing PUT request to {Uri}", requestUri);
                return await _httpClient.PutAsync(requestUri, content, ct);
            }, cancellationToken);
        }
        
        public async Task<HttpResponseMessage> DeleteAsync(string requestUri, CancellationToken cancellationToken = default)
        {
            return await _resilientPolicy.ExecuteAsync(async (ct) =>
            {
                _logger.LogDebug("Executing DELETE request to {Uri}", requestUri);
                return await _httpClient.DeleteAsync(requestUri, ct);
            }, cancellationToken);
        }
        
        public async Task<string> GetStringAsync(string requestUri, CancellationToken cancellationToken = default)
        {
            var response = await GetAsync(requestUri, cancellationToken);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync(cancellationToken);
        }
        
        private AsyncPolicyWrap<HttpResponseMessage> CreateResilientPolicy()
        {
            var retryPolicy = HttpPolicyExtensions
                .HandleTransientHttpError()
                .WaitAndRetryAsync(
                    retryCount: 3,
                    sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                    onRetry: (outcome, timespan, retryCount, context) =>
                    {
                        _logger.LogWarning("Retry {RetryCount} after {Delay}ms. Result: {Result}",
                            retryCount, timespan.TotalMilliseconds,
                            outcome.Exception?.Message ?? outcome.Result?.StatusCode.ToString());
                    });
            
            var circuitBreakerPolicy = HttpPolicyExtensions
                .HandleTransientHttpError()
                .CircuitBreakerAsync(
                    handledEventsAllowedBeforeBreaking: 5,
                    durationOfBreak: TimeSpan.FromSeconds(30),
                    onBreak: (exception, duration) =>
                    {
                        _logger.LogWarning("Circuit breaker opened for {Duration}ms. Exception: {Exception}",
                            duration.TotalMilliseconds, exception.Exception?.Message ?? exception.Result?.StatusCode.ToString());
                    },
                    onReset: () =>
                    {
                        _logger.LogInformation("Circuit breaker reset");
                    });
            
            var timeoutPolicy = Policy.TimeoutAsync<HttpResponseMessage>(
                timeout: TimeSpan.FromSeconds(30),
                onTimeoutAsync: (context, timespan, task) =>
                {
                    _logger.LogWarning("Request timed out after {Timeout}ms", timespan.TotalMilliseconds);
                    return Task.CompletedTask;
                });
            
            var bulkheadPolicy = Policy.BulkheadAsync<HttpResponseMessage>(
                maxParallelization: 10,
                maxQueuingActions: 20,
                onBulkheadRejectedAsync: (context) =>
                {
                    _logger.LogWarning("Request rejected by bulkhead policy");
                    return Task.CompletedTask;
                });
            
            return Policy.WrapAsync(retryPolicy, circuitBreakerPolicy, timeoutPolicy, bulkheadPolicy);
        }
    }
}