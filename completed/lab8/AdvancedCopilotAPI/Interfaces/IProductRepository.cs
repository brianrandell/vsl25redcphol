using AdvancedCopilotAPI.Models;

namespace AdvancedCopilotAPI.Interfaces
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetAllProductsAsync(CancellationToken cancellationToken = default);
        Task<Product?> GetProductByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<IEnumerable<Product>> GetProductsByCategoryAsync(string category, CancellationToken cancellationToken = default);
        Task<IEnumerable<Product>> GetLowStockProductsAsync(int threshold = 10, CancellationToken cancellationToken = default);
        Task<Product> CreateProductAsync(Product product, CancellationToken cancellationToken = default);
        Task<Product> UpdateProductAsync(Product product, CancellationToken cancellationToken = default);
        Task<bool> DeleteProductAsync(int id, CancellationToken cancellationToken = default);
        Task<bool> ProductExistsAsync(int id, CancellationToken cancellationToken = default);
        Task<int> GetProductCountAsync(CancellationToken cancellationToken = default);
    }
}