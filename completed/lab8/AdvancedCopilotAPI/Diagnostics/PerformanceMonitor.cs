using System.Collections.Concurrent;
using System.Diagnostics;

namespace AdvancedCopilotAPI.Diagnostics
{
    public class PerformanceMetrics
    {
        public string OperationName { get; set; } = string.Empty;
        public long ExecutionTimeMs { get; set; }
        public long MemoryUsedBytes { get; set; }
        public DateTime Timestamp { get; set; }
        public string? AdditionalData { get; set; }
        public bool IsSlowOperation => ExecutionTimeMs > 1000;
    }
    
    public class DatabaseMetrics
    {
        internal int _queryCount;
        internal long _totalQueryTimeMs;
        
        public int QueryCount 
        { 
            get => _queryCount; 
            set => _queryCount = value; 
        }
        
        public long TotalQueryTimeMs 
        { 
            get => _totalQueryTimeMs; 
            set => _totalQueryTimeMs = value; 
        }
        
        public List<string> SlowQueries { get; set; } = new();
        public DateTime LastReset { get; set; } = DateTime.UtcNow;
    }
    
    public class HttpMetrics
    {
        public string Endpoint { get; set; } = string.Empty;
        public string Method { get; set; } = string.Empty;
        public int StatusCode { get; set; }
        public long DurationMs { get; set; }
        public DateTime Timestamp { get; set; }
        public long RequestSizeBytes { get; set; }
        public long ResponseSizeBytes { get; set; }
    }
    
    public interface IPerformanceMonitor
    {
        IDisposable StartOperation(string operationName);
        void RecordHttpRequest(HttpMetrics metrics);
        void RecordDatabaseQuery(string query, long durationMs);
        void RecordMemorySnapshot(string operation, long memoryBytes);
        IEnumerable<PerformanceMetrics> GetMetrics(DateTime? since = null);
        DatabaseMetrics GetDatabaseMetrics();
        void ResetMetrics();
        Task<string> GenerateReportAsync();
    }
    
    public class PerformanceMonitor : IPerformanceMonitor
    {
        private readonly ILogger<PerformanceMonitor> _logger;
        private readonly ConcurrentQueue<PerformanceMetrics> _performanceMetrics;
        private readonly ConcurrentQueue<HttpMetrics> _httpMetrics;
        private readonly DatabaseMetrics _databaseMetrics;
        private readonly SemaphoreSlim _semaphore;
        private const long SlowOperationThresholdMs = 1000;
        private const long SlowQueryThresholdMs = 500;
        
        public PerformanceMonitor(ILogger<PerformanceMonitor> logger)
        {
            _logger = logger;
            _performanceMetrics = new ConcurrentQueue<PerformanceMetrics>();
            _httpMetrics = new ConcurrentQueue<HttpMetrics>();
            _databaseMetrics = new DatabaseMetrics();
            _semaphore = new SemaphoreSlim(1, 1);
        }
        
        public IDisposable StartOperation(string operationName)
        {
            return new OperationTracker(this, operationName, _logger);
        }
        
        public void RecordHttpRequest(HttpMetrics metrics)
        {
            _httpMetrics.Enqueue(metrics);
            
            if (metrics.DurationMs > SlowOperationThresholdMs)
            {
                _logger.LogWarning("Slow HTTP request detected: {Method} {Endpoint} took {Duration}ms",
                    metrics.Method, metrics.Endpoint, metrics.DurationMs);
            }
            
            CleanupOldMetrics();
        }
        
        public void RecordDatabaseQuery(string query, long durationMs)
        {
            Interlocked.Increment(ref _databaseMetrics._queryCount);
            Interlocked.Add(ref _databaseMetrics._totalQueryTimeMs, durationMs);
            
            if (durationMs > SlowQueryThresholdMs)
            {
                _databaseMetrics.SlowQueries.Add($"{query} ({durationMs}ms)");
                
                _logger.LogWarning("Slow database query detected: {Query} took {Duration}ms",
                    query.Length > 100 ? query[..100] + "..." : query, durationMs);
            }
        }
        
        public void RecordMemorySnapshot(string operation, long memoryBytes)
        {
            var metrics = new PerformanceMetrics
            {
                OperationName = operation,
                MemoryUsedBytes = memoryBytes,
                Timestamp = DateTime.UtcNow,
                AdditionalData = $"Memory: {memoryBytes:N0} bytes"
            };
            
            _performanceMetrics.Enqueue(metrics);
            
            _logger.LogDebug("Memory snapshot for {Operation}: {Memory:N0} bytes",
                operation, memoryBytes);
        }
        
        public IEnumerable<PerformanceMetrics> GetMetrics(DateTime? since = null)
        {
            var cutoff = since ?? DateTime.UtcNow.AddHours(-1);
            return _performanceMetrics.Where(m => m.Timestamp >= cutoff).ToList();
        }
        
        public DatabaseMetrics GetDatabaseMetrics()
        {
            return new DatabaseMetrics
            {
                QueryCount = _databaseMetrics.QueryCount,
                TotalQueryTimeMs = _databaseMetrics.TotalQueryTimeMs,
                SlowQueries = new List<string>(_databaseMetrics.SlowQueries),
                LastReset = _databaseMetrics.LastReset
            };
        }
        
        public void ResetMetrics()
        {
            while (_performanceMetrics.TryDequeue(out _)) { }
            while (_httpMetrics.TryDequeue(out _)) { }
            
            _databaseMetrics.QueryCount = 0;
            _databaseMetrics.TotalQueryTimeMs = 0;
            _databaseMetrics.SlowQueries.Clear();
            _databaseMetrics.LastReset = DateTime.UtcNow;
            
            _logger.LogInformation("Performance metrics reset");
        }
        
        public async Task<string> GenerateReportAsync()
        {
            await _semaphore.WaitAsync();
            try
            {
                var report = new System.Text.StringBuilder();
                var now = DateTime.UtcNow;
                
                report.AppendLine("# Performance Monitoring Report");
                report.AppendLine($"Generated: {now:yyyy-MM-dd HH:mm:ss} UTC");
                report.AppendLine();
                
                var recentMetrics = GetMetrics(now.AddHours(-1)).ToList();
                
                report.AppendLine("## Operation Performance (Last Hour)");
                report.AppendLine($"Total Operations: {recentMetrics.Count}");
                
                if (recentMetrics.Any())
                {
                    var avgTime = recentMetrics.Where(m => m.ExecutionTimeMs > 0).Average(m => m.ExecutionTimeMs);
                    var slowOps = recentMetrics.Count(m => m.IsSlowOperation);
                    
                    report.AppendLine($"Average Execution Time: {avgTime:F2}ms");
                    report.AppendLine($"Slow Operations: {slowOps} ({(double)slowOps / recentMetrics.Count * 100:F1}%)");
                    report.AppendLine();
                    
                    report.AppendLine("### Slowest Operations");
                    var slowest = recentMetrics
                        .Where(m => m.ExecutionTimeMs > 0)
                        .OrderByDescending(m => m.ExecutionTimeMs)
                        .Take(5);
                    
                    foreach (var op in slowest)
                    {
                        report.AppendLine($"- {op.OperationName}: {op.ExecutionTimeMs}ms");
                    }
                }
                
                report.AppendLine();
                report.AppendLine("## Database Performance");
                var dbMetrics = GetDatabaseMetrics();
                report.AppendLine($"Total Queries: {dbMetrics.QueryCount}");
                report.AppendLine($"Total Query Time: {dbMetrics.TotalQueryTimeMs}ms");
                
                if (dbMetrics.QueryCount > 0)
                {
                    var avgQueryTime = (double)dbMetrics.TotalQueryTimeMs / dbMetrics.QueryCount;
                    report.AppendLine($"Average Query Time: {avgQueryTime:F2}ms");
                }
                
                report.AppendLine($"Slow Queries: {dbMetrics.SlowQueries.Count}");
                
                if (dbMetrics.SlowQueries.Any())
                {
                    report.AppendLine();
                    report.AppendLine("### Recent Slow Queries");
                    foreach (var query in dbMetrics.SlowQueries.TakeLast(5))
                    {
                        report.AppendLine($"- {query}");
                    }
                }
                
                report.AppendLine();
                report.AppendLine("## HTTP Requests (Last Hour)");
                var recentHttpMetrics = _httpMetrics.Where(m => m.Timestamp >= now.AddHours(-1)).ToList();
                report.AppendLine($"Total Requests: {recentHttpMetrics.Count}");
                
                if (recentHttpMetrics.Any())
                {
                    var avgDuration = recentHttpMetrics.Average(m => m.DurationMs);
                    var errorCount = recentHttpMetrics.Count(m => m.StatusCode >= 400);
                    
                    report.AppendLine($"Average Response Time: {avgDuration:F2}ms");
                    report.AppendLine($"Error Rate: {(double)errorCount / recentHttpMetrics.Count * 100:F1}%");
                    
                    var endpointStats = recentHttpMetrics
                        .GroupBy(m => $"{m.Method} {m.Endpoint}")
                        .Select(g => new
                        {
                            Endpoint = g.Key,
                            Count = g.Count(),
                            AvgDuration = g.Average(m => m.DurationMs)
                        })
                        .OrderByDescending(s => s.Count)
                        .Take(5);
                    
                    report.AppendLine();
                    report.AppendLine("### Top Endpoints");
                    foreach (var stat in endpointStats)
                    {
                        report.AppendLine($"- {stat.Endpoint}: {stat.Count} requests, {stat.AvgDuration:F2}ms avg");
                    }
                }
                
                return report.ToString();
            }
            finally
            {
                _semaphore.Release();
            }
        }
        
        internal void RecordOperation(string operationName, long executionTimeMs, long memoryUsed)
        {
            var metrics = new PerformanceMetrics
            {
                OperationName = operationName,
                ExecutionTimeMs = executionTimeMs,
                MemoryUsedBytes = memoryUsed,
                Timestamp = DateTime.UtcNow
            };
            
            _performanceMetrics.Enqueue(metrics);
            
            if (metrics.IsSlowOperation)
            {
                _logger.LogWarning("Slow operation detected: {Operation} took {Duration}ms",
                    operationName, executionTimeMs);
            }
            
            CleanupOldMetrics();
        }
        
        private void CleanupOldMetrics()
        {
            var cutoff = DateTime.UtcNow.AddHours(-24);
            
            while (_performanceMetrics.TryPeek(out var metric) && metric.Timestamp < cutoff)
            {
                _performanceMetrics.TryDequeue(out _);
            }
            
            while (_httpMetrics.TryPeek(out var httpMetric) && httpMetric.Timestamp < cutoff)
            {
                _httpMetrics.TryDequeue(out _);
            }
        }
    }
    
    internal class OperationTracker : IDisposable
    {
        private readonly PerformanceMonitor _monitor;
        private readonly string _operationName;
        private readonly ILogger _logger;
        private readonly Stopwatch _stopwatch;
        private readonly long _startMemory;
        
        public OperationTracker(PerformanceMonitor monitor, string operationName, ILogger logger)
        {
            _monitor = monitor;
            _operationName = operationName;
            _logger = logger;
            _stopwatch = Stopwatch.StartNew();
            _startMemory = GC.GetTotalMemory(false);
        }
        
        public void Dispose()
        {
            _stopwatch.Stop();
            var endMemory = GC.GetTotalMemory(false);
            var memoryUsed = Math.Max(0, endMemory - _startMemory);
            
            _monitor.RecordOperation(_operationName, _stopwatch.ElapsedMilliseconds, memoryUsed);
        }
    }
}