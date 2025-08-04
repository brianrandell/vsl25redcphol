using AdvancedCopilotAPI.Interfaces;
using AdvancedCopilotAPI.Models;
using System.Diagnostics;

namespace AdvancedCopilotAPI.Decorators
{
    public class LoggingRepositoryDecorator<T> : IProductRepository where T : class
    {
        private readonly IProductRepository _repository;
        private readonly ILogger<LoggingRepositoryDecorator<T>> _logger;
        
        public LoggingRepositoryDecorator(IProductRepository repository, ILogger<LoggingRepositoryDecorator<T>> logger)
        {
            _repository = repository;
            _logger = logger;
        }
        
        public async Task<IEnumerable<Product>> GetAllProductsAsync(CancellationToken cancellationToken = default)
        {
            return await ExecuteWithLogging(
                nameof(GetAllProductsAsync),
                () => _repository.GetAllProductsAsync(cancellationToken),
                cancellationToken);
        }
        
        public async Task<Product?> GetProductByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await ExecuteWithLogging(
                nameof(GetProductByIdAsync),
                () => _repository.GetProductByIdAsync(id, cancellationToken),
                cancellationToken,
                new { id });
        }
        
        public async Task<IEnumerable<Product>> GetProductsByCategoryAsync(string category, CancellationToken cancellationToken = default)
        {
            return await ExecuteWithLogging(
                nameof(GetProductsByCategoryAsync),
                () => _repository.GetProductsByCategoryAsync(category, cancellationToken),
                cancellationToken,
                new { category });
        }
        
        public async Task<IEnumerable<Product>> GetLowStockProductsAsync(int threshold = 10, CancellationToken cancellationToken = default)
        {
            return await ExecuteWithLogging(
                nameof(GetLowStockProductsAsync),
                () => _repository.GetLowStockProductsAsync(threshold, cancellationToken),
                cancellationToken,
                new { threshold });
        }
        
        public async Task<Product> CreateProductAsync(Product product, CancellationToken cancellationToken = default)
        {
            return await ExecuteWithLogging(
                nameof(CreateProductAsync),
                () => _repository.CreateProductAsync(product, cancellationToken),
                cancellationToken,
                new { productName = product.Name });
        }
        
        public async Task<Product> UpdateProductAsync(Product product, CancellationToken cancellationToken = default)
        {
            return await ExecuteWithLogging(
                nameof(UpdateProductAsync),
                () => _repository.UpdateProductAsync(product, cancellationToken),
                cancellationToken,
                new { productId = product.Id, productName = product.Name });
        }
        
        public async Task<bool> DeleteProductAsync(int id, CancellationToken cancellationToken = default)
        {
            return await ExecuteWithLogging(
                nameof(DeleteProductAsync),
                () => _repository.DeleteProductAsync(id, cancellationToken),
                cancellationToken,
                new { id });
        }
        
        public async Task<bool> ProductExistsAsync(int id, CancellationToken cancellationToken = default)
        {
            return await ExecuteWithLogging(
                nameof(ProductExistsAsync),
                () => _repository.ProductExistsAsync(id, cancellationToken),
                cancellationToken,
                new { id });
        }
        
        public async Task<int> GetProductCountAsync(CancellationToken cancellationToken = default)
        {
            return await ExecuteWithLogging(
                nameof(GetProductCountAsync),
                () => _repository.GetProductCountAsync(cancellationToken),
                cancellationToken);
        }
        
        private async Task<TResult> ExecuteWithLogging<TResult>(
            string methodName,
            Func<Task<TResult>> operation,
            CancellationToken cancellationToken,
            object? parameters = null)
        {
            var stopwatch = Stopwatch.StartNew();
            var correlationId = Guid.NewGuid().ToString("N")[..8];
            
            _logger.LogInformation("Starting {MethodName} [CorrelationId: {CorrelationId}] with parameters: {@Parameters}",
                methodName, correlationId, parameters);
            
            try
            {
                var result = await operation();
                
                stopwatch.Stop();
                _logger.LogInformation("Completed {MethodName} [CorrelationId: {CorrelationId}] in {ElapsedMs}ms",
                    methodName, correlationId, stopwatch.ElapsedMilliseconds);
                
                return result;
            }
            catch (Exception ex)
            {
                stopwatch.Stop();
                _logger.LogError(ex, "Failed {MethodName} [CorrelationId: {CorrelationId}] after {ElapsedMs}ms",
                    methodName, correlationId, stopwatch.ElapsedMilliseconds);
                
                throw;
            }
        }
    }
}