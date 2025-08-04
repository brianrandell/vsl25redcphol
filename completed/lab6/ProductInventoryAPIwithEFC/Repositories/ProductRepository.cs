using Microsoft.EntityFrameworkCore;
using ProductInventoryAPI.Data;
using ProductInventoryAPI.Models;
using Microsoft.Data.SqlClient;

namespace ProductInventoryAPI.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly AppDbContext _context;

    public ProductRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Product>> GetAllAsync()
    {
        return await _context.Products
            .AsNoTracking()
            .Include(p => p.CategoryNavigation)
            .TagWith("GetAllProducts")
            .OrderBy(p => p.Name)
            .ToListAsync();
    }

    public async Task<Product?> GetByIdAsync(int id)
    {
        return await _context.Products
            .Include(p => p.CategoryNavigation)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<IEnumerable<Product>> GetByCategoryAsync(int categoryId)
    {
        return await _context.Products
            .AsNoTracking()
            .Include(p => p.CategoryNavigation)
            .Where(p => p.CategoryId == categoryId && p.IsActive)
            .OrderBy(p => p.Name)
            .ToListAsync();
    }

    public async Task<IEnumerable<Product>> GetActiveProductsAsync()
    {
        return await _context.Products
            .AsNoTracking()
            .Include(p => p.CategoryNavigation)
            .Where(p => p.IsActive && p.CategoryNavigation!.IsActive)
            .OrderBy(p => p.Name)
            .ToListAsync();
    }

    public async Task<IEnumerable<Product>> SearchAsync(string searchTerm)
    {
        return await _context.Products
            .AsNoTracking()
            .Include(p => p.CategoryNavigation)
            .Where(p => EF.Functions.Like(p.Name, $"%{searchTerm}%") || 
                       EF.Functions.Like(p.Description, $"%{searchTerm}%"))
            .OrderBy(p => p.Name)
            .ToListAsync();
    }

    public async Task<IEnumerable<Product>> GetProductsWithLowStockAsync(int threshold)
    {
        return await _context.Products
            .AsNoTracking()
            .Include(p => p.CategoryNavigation)
            .Where(p => p.StockQuantity < threshold && p.IsActive)
            .OrderBy(p => p.StockQuantity)
            .ToListAsync();
    }

    public async Task<IEnumerable<Product>> GetProductsByPriceRangeAsync(decimal minPrice, decimal maxPrice)
    {
        return await _context.Products
            .AsNoTracking()
            .Include(p => p.CategoryNavigation)
            .Where(p => p.Price >= minPrice && p.Price <= maxPrice && p.IsActive)
            .OrderBy(p => p.Price)
            .ToListAsync();
    }

    public async Task<IEnumerable<Product>> GetTopSellingProductsAsync(int count)
    {
        // Placeholder implementation - in a real app, this would use sales data
        return await _context.Products
            .AsNoTracking()
            .Include(p => p.CategoryNavigation)
            .Where(p => p.IsActive)
            .OrderByDescending(p => p.StockQuantity) // Using stock as proxy for popularity
            .Take(count)
            .ToListAsync();
    }

    public async Task<Product> AddAsync(Product product)
    {
        _context.Products.Add(product);
        await _context.SaveChangesAsync();
        return product;
    }

    public async Task<Product> UpdateAsync(Product product)
    {
        _context.Entry(product).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        return product;
    }

    public async Task DeleteAsync(int id)
    {
        var product = await _context.Products.FindAsync(id);
        if (product != null)
        {
            // Soft delete
            product.IsActive = false;
            await _context.SaveChangesAsync();
        }
    }

    public async Task<bool> ExistsAsync(int id)
    {
        return await _context.Products.AnyAsync(p => p.Id == id && p.IsActive);
    }

    public async Task<bool> ExistsByNameAsync(string name, int? excludeId = null)
    {
        var query = _context.Products.Where(p => p.Name == name && p.IsActive);
        
        if (excludeId.HasValue)
        {
            query = query.Where(p => p.Id != excludeId.Value);
        }
        
        return await query.AnyAsync();
    }

    public async Task<IEnumerable<object>> GetProductStatisticsByCategoryAsync()
    {
        return await _context.Products
            .Include(p => p.CategoryNavigation)
            .Where(p => p.IsActive)
            .GroupBy(p => p.CategoryNavigation!.Name)
            .Select(g => new
            {
                CategoryName = g.Key,
                ProductCount = g.Count(),
                AveragePrice = g.Average(p => p.Price),
                TotalStockValue = g.Sum(p => p.Price * p.StockQuantity)
            })
            .OrderBy(x => x.CategoryName)
            .ToListAsync();
    }

    public async Task<IEnumerable<object>> GetInventoryValueReportAsync()
    {
        return await _context.Products
            .Include(p => p.CategoryNavigation)
            .Where(p => p.IsActive)
            .GroupBy(p => p.CategoryNavigation!.Name)
            .Select(g => new
            {
                CategoryName = g.Key,
                TotalValue = g.Sum(p => p.Price * p.StockQuantity),
                ProductCount = g.Count(),
                AverageValue = g.Average(p => p.Price * p.StockQuantity)
            })
            .OrderByDescending(x => x.TotalValue)
            .ToListAsync();
    }

    public async Task<IEnumerable<Product>> GetProductsByCategoryWithPaginationAsync(int categoryId, int pageNumber, int pageSize)
    {
        var categoryIdParam = new SqlParameter("@CategoryId", categoryId);
        var pageNumberParam = new SqlParameter("@PageNumber", pageNumber);
        var pageSizeParam = new SqlParameter("@PageSize", pageSize);

        return await _context.Products
            .FromSqlRaw("EXEC sp_GetProductsByCategory @CategoryId, @PageNumber, @PageSize", 
                       categoryIdParam, pageNumberParam, pageSizeParam)
            .ToListAsync();
    }

    public async Task<object> GetInventoryReportAsync()
    {
        var result = await _context.Database
            .SqlQueryRaw<InventoryReportResult>("EXEC sp_GetInventoryReport")
            .FirstOrDefaultAsync();

        return result ?? new InventoryReportResult();
    }

    // Helper class for stored procedure result
    private class InventoryReportResult
    {
        public int TotalProducts { get; set; }
        public decimal TotalInventoryValue { get; set; }
        public int LowStockItemsCount { get; set; }
        public string TopCategory { get; set; } = string.Empty;
    }
}