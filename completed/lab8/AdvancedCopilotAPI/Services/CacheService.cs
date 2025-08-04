using Microsoft.Extensions.Caching.Memory;
using System.Collections.Concurrent;

namespace AdvancedCopilotAPI.Services
{
    public interface ICacheService
    {
        Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken = default);
        Task SetAsync<T>(string key, T value, TimeSpan? expiration = null, CancellationToken cancellationToken = default);
        Task RemoveAsync(string key, CancellationToken cancellationToken = default);
        Task RemoveByPatternAsync(string pattern, CancellationToken cancellationToken = default);
        Task ClearAsync(CancellationToken cancellationToken = default);
    }
    
    public class CacheService : ICacheService
    {
        private readonly IMemoryCache _memoryCache;
        private readonly ILogger<CacheService> _logger;
        private readonly ConcurrentDictionary<string, bool> _cacheKeys;
        private readonly SemaphoreSlim _semaphore;
        
        public CacheService(IMemoryCache memoryCache, ILogger<CacheService> logger)
        {
            _memoryCache = memoryCache;
            _logger = logger;
            _cacheKeys = new ConcurrentDictionary<string, bool>();
            _semaphore = new SemaphoreSlim(1, 1);
        }
        
        public async Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken = default)
        {
            await _semaphore.WaitAsync(cancellationToken);
            try
            {
                if (_memoryCache.TryGetValue(key, out var value))
                {
                    _logger.LogDebug("Cache hit for key: {Key}", key);
                    return (T?)value;
                }
                
                _logger.LogDebug("Cache miss for key: {Key}", key);
                return default;
            }
            finally
            {
                _semaphore.Release();
            }
        }
        
        public async Task SetAsync<T>(string key, T value, TimeSpan? expiration = null, CancellationToken cancellationToken = default)
        {
            await _semaphore.WaitAsync(cancellationToken);
            try
            {
                var options = new MemoryCacheEntryOptions();
                
                if (expiration.HasValue)
                {
                    options.AbsoluteExpirationRelativeToNow = expiration.Value;
                }
                else
                {
                    options.SlidingExpiration = TimeSpan.FromMinutes(30);
                }
                
                options.RegisterPostEvictionCallback((key, value, reason, state) =>
                {
                    _cacheKeys.TryRemove(key.ToString()!, out _);
                    _logger.LogDebug("Cache entry evicted: {Key}, Reason: {Reason}", key, reason);
                });
                
                _memoryCache.Set(key, value, options);
                _cacheKeys.TryAdd(key, true);
                
                _logger.LogDebug("Cache entry set for key: {Key}", key);
            }
            finally
            {
                _semaphore.Release();
            }
        }
        
        public async Task RemoveAsync(string key, CancellationToken cancellationToken = default)
        {
            await _semaphore.WaitAsync(cancellationToken);
            try
            {
                _memoryCache.Remove(key);
                _cacheKeys.TryRemove(key, out _);
                
                _logger.LogDebug("Cache entry removed for key: {Key}", key);
            }
            finally
            {
                _semaphore.Release();
            }
        }
        
        public async Task RemoveByPatternAsync(string pattern, CancellationToken cancellationToken = default)
        {
            await _semaphore.WaitAsync(cancellationToken);
            try
            {
                var keysToRemove = _cacheKeys.Keys
                    .Where(key => key.Contains(pattern, StringComparison.OrdinalIgnoreCase))
                    .ToList();
                
                foreach (var key in keysToRemove)
                {
                    _memoryCache.Remove(key);
                    _cacheKeys.TryRemove(key, out _);
                }
                
                _logger.LogDebug("Removed {Count} cache entries matching pattern: {Pattern}", keysToRemove.Count, pattern);
            }
            finally
            {
                _semaphore.Release();
            }
        }
        
        public async Task ClearAsync(CancellationToken cancellationToken = default)
        {
            await _semaphore.WaitAsync(cancellationToken);
            try
            {
                var keysToRemove = _cacheKeys.Keys.ToList();
                
                foreach (var key in keysToRemove)
                {
                    _memoryCache.Remove(key);
                }
                
                _cacheKeys.Clear();
                
                _logger.LogDebug("Cache cleared. Removed {Count} entries", keysToRemove.Count);
            }
            finally
            {
                _semaphore.Release();
            }
        }
        
        public string GenerateCacheKey(string prefix, params object[] parameters)
        {
            var paramString = string.Join(":", parameters.Select(p => p?.ToString() ?? "null"));
            return $"{prefix}:{paramString}";
        }
    }
}