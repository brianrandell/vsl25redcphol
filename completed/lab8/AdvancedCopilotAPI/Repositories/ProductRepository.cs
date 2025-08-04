using AdvancedCopilotAPI.Data;
using AdvancedCopilotAPI.Interfaces;
using AdvancedCopilotAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace AdvancedCopilotAPI.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext _context;
        
        public ProductRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        
        public async Task<IEnumerable<Product>> GetAllProductsAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Products
                .Where(p => p.IsActive)
                .OrderBy(p => p.Name)
                .ToListAsync(cancellationToken);
        }
        
        public async Task<Product?> GetProductByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _context.Products
                .FirstOrDefaultAsync(p => p.Id == id && p.IsActive, cancellationToken);
        }
        
        public async Task<IEnumerable<Product>> GetProductsByCategoryAsync(string category, CancellationToken cancellationToken = default)
        {
            return await _context.Products
                .Where(p => p.Category == category && p.IsActive)
                .OrderBy(p => p.Name)
                .ToListAsync(cancellationToken);
        }
        
        public async Task<IEnumerable<Product>> GetLowStockProductsAsync(int threshold = 10, CancellationToken cancellationToken = default)
        {
            return await _context.Products
                .Where(p => p.Stock <= threshold && p.IsActive)
                .OrderBy(p => p.Stock)
                .ToListAsync(cancellationToken);
        }
        
        public async Task<Product> CreateProductAsync(Product product, CancellationToken cancellationToken = default)
        {
            product.CreatedAt = DateTime.UtcNow;
            product.UpdatedAt = DateTime.UtcNow;
            
            _context.Products.Add(product);
            await _context.SaveChangesAsync(cancellationToken);
            return product;
        }
        
        public async Task<Product> UpdateProductAsync(Product product, CancellationToken cancellationToken = default)
        {
            product.UpdatedAt = DateTime.UtcNow;
            
            _context.Entry(product).State = EntityState.Modified;
            await _context.SaveChangesAsync(cancellationToken);
            return product;
        }
        
        public async Task<bool> DeleteProductAsync(int id, CancellationToken cancellationToken = default)
        {
            var product = await _context.Products.FindAsync(new object[] { id }, cancellationToken);
            if (product == null) return false;
            
            product.IsActive = false;
            product.UpdatedAt = DateTime.UtcNow;
            
            await _context.SaveChangesAsync(cancellationToken);
            return true;
        }
        
        public async Task<bool> ProductExistsAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _context.Products
                .AnyAsync(p => p.Id == id && p.IsActive, cancellationToken);
        }
        
        public async Task<int> GetProductCountAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Products
                .CountAsync(p => p.IsActive, cancellationToken);
        }
    }
}