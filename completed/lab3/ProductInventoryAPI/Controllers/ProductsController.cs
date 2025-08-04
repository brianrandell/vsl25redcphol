using Microsoft.AspNetCore.Mvc;
using ProductInventoryAPI.DTOs;
using ProductInventoryAPI.Exceptions;
using ProductInventoryAPI.Extensions;
using ProductInventoryAPI.Models;
using ProductInventoryAPI.Services;
using System.Diagnostics;

namespace ProductInventoryAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[ApiVersion("1.0")]
public class ProductsController : ControllerBase
{
    private readonly ILogger<ProductsController> _logger;
    private readonly ISanitizationService _sanitizationService;
    private readonly ICurrentUserService _currentUserService;
    private static readonly object _lock = new object();
    private static readonly List<Product> _products = new()
    {
        new Product { Id = 1, Name = "Laptop", Description = "High-performance laptop", Price = 999.99m, StockQuantity = 10, Category = "Electronics", IsActive = true, CreatedBy = "system", UpdatedBy = "system", CreatedAt = DateTime.UtcNow.AddDays(-30), UpdatedAt = DateTime.UtcNow.AddDays(-30) },
        new Product { Id = 2, Name = "Mouse", Description = "Wireless mouse", Price = 29.99m, StockQuantity = 50, Category = "Electronics", IsActive = true, CreatedBy = "system", UpdatedBy = "system", CreatedAt = DateTime.UtcNow.AddDays(-20), UpdatedAt = DateTime.UtcNow.AddDays(-20) },
        new Product { Id = 3, Name = "Keyboard", Description = "Mechanical keyboard", Price = 79.99m, StockQuantity = 25, Category = "Electronics", IsActive = true, CreatedBy = "system", UpdatedBy = "system", CreatedAt = DateTime.UtcNow.AddDays(-10), UpdatedAt = DateTime.UtcNow.AddDays(-10) }
    };

    private static int _nextId = 4;

    public ProductsController(ILogger<ProductsController> logger, ISanitizationService sanitizationService, ICurrentUserService currentUserService)
    {
        _logger = logger;
        _sanitizationService = sanitizationService;
        _currentUserService = currentUserService;
    }

#if DEBUG
    // Internal method for test purposes only - allows resetting static state
    internal static void ResetDataForTesting()
    {
        lock (_lock)
        {
            _products.Clear();
            _products.AddRange(new[]
            {
                new Product { Id = 1, Name = "Laptop", Description = "High-performance laptop", Price = 999.99m, StockQuantity = 10, Category = "Electronics", IsActive = true, CreatedBy = "system", UpdatedBy = "system", CreatedAt = DateTime.UtcNow.AddDays(-30), UpdatedAt = DateTime.UtcNow.AddDays(-30) },
                new Product { Id = 2, Name = "Mouse", Description = "Wireless mouse", Price = 29.99m, StockQuantity = 50, Category = "Electronics", IsActive = true, CreatedBy = "system", UpdatedBy = "system", CreatedAt = DateTime.UtcNow.AddDays(-20), UpdatedAt = DateTime.UtcNow.AddDays(-20) },
                new Product { Id = 3, Name = "Keyboard", Description = "Mechanical keyboard", Price = 79.99m, StockQuantity = 25, Category = "Electronics", IsActive = true, CreatedBy = "system", UpdatedBy = "system", CreatedAt = DateTime.UtcNow.AddDays(-10), UpdatedAt = DateTime.UtcNow.AddDays(-10) }
            });
            _nextId = 4;
        }
    }
#endif

    /// <summary>
    /// Gets a paginated list of active products
    /// </summary>
    /// <param name="pageNumber">Page number (starting from 1)</param>
    /// <param name="pageSize">Number of items per page (max 100)</param>
    /// <returns>List of products</returns>
    /// <response code="200">Returns the list of products</response>
    [HttpGet]
    public ActionResult<IEnumerable<ProductResponseDto>> GetProducts([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    {
        var stopwatch = Stopwatch.StartNew();
        _logger.LogInformation("Entering GetProducts method with parameters: pageNumber={PageNumber}, pageSize={PageSize}", pageNumber, pageSize);
        
        try
        {
            if (pageNumber < 1) pageNumber = 1;
            if (pageSize < 1) pageSize = 10;
            if (pageSize > 100) pageSize = 100;

            lock (_lock)
            {
                var totalProducts = _products.Count(p => p.IsActive);
                var products = _products
                    .Where(p => p.IsActive)
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToResponseDtos()
                    .ToList();

                Response.Headers["X-Total-Count"] = totalProducts.ToString();
                Response.Headers["X-Page-Number"] = pageNumber.ToString();
                Response.Headers["X-Page-Size"] = pageSize.ToString();

                _logger.LogInformation("GetProducts completed successfully. Returned {ProductCount} products out of {TotalProducts} total", products.Count, totalProducts);
                return Ok(products);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in GetProducts method with parameters: pageNumber={PageNumber}, pageSize={PageSize}", pageNumber, pageSize);
            throw;
        }
        finally
        {
            stopwatch.Stop();
            _logger.LogInformation("GetProducts method execution time: {ElapsedMilliseconds}ms", stopwatch.ElapsedMilliseconds);
        }
    }

    /// <summary>
    /// Gets a specific product by ID
    /// </summary>
    /// <param name="id">Product ID</param>
    /// <returns>Product details</returns>
    /// <response code="200">Returns the product</response>
    /// <response code="404">Product not found</response>
    [HttpGet("{id}")]
    public ActionResult<ProductResponseDto> GetProduct(int id)
    {
        var stopwatch = Stopwatch.StartNew();
        _logger.LogInformation("Entering GetProduct method with parameter: id={Id}", id);
        
        try
        {
            lock (_lock)
            {
                var product = _products.FirstOrDefault(p => p.Id == id && p.IsActive);
                if (product == null)
                {
                    _logger.LogWarning("Product not found with id={Id}", id);
                    throw new NotFoundException("Product", id);
                }
                
                _logger.LogInformation("GetProduct completed successfully for product: {ProductName} (id={Id})", product.Name, id);
                return Ok(product.ToResponseDto());
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in GetProduct method with parameter: id={Id}", id);
            throw;
        }
        finally
        {
            stopwatch.Stop();
            _logger.LogInformation("GetProduct method execution time: {ElapsedMilliseconds}ms", stopwatch.ElapsedMilliseconds);
        }
    }

    /// <summary>
    /// Creates a new product
    /// </summary>
    /// <param name="createDto">Product data</param>
    /// <returns>Created product</returns>
    /// <response code="201">Product created successfully</response>
    /// <response code="400">Invalid product data</response>
    /// <response code="422">Business rule violation (e.g., duplicate name)</response>
    [HttpPost]
    public ActionResult<ProductResponseDto> CreateProduct([FromBody] ProductCreateDto createDto)
    {
        var stopwatch = Stopwatch.StartNew();
        _logger.LogInformation("Entering CreateProduct method with parameters: Name={Name}, Category={Category}, Price={Price}", 
            createDto?.Name, createDto?.Category, createDto?.Price);
        
        try
        {
            if (createDto == null)
            {
                _logger.LogWarning("CreateProduct called with null createDto");
                throw new ValidationException(new Dictionary<string, string[]> { ["createDto"] = new[] { "Request body cannot be null" } });
            }

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("CreateProduct validation failed for product: {Name}", createDto.Name);
                var errors = ModelState.ToDictionary(
                    kvp => kvp.Key,
                    kvp => kvp.Value?.Errors.Select(e => e.ErrorMessage).ToArray() ?? Array.Empty<string>()
                );
                throw new ValidationException(errors);
            }

            lock (_lock)
            {
                // Check for unique name
                if (_products.Any(p => p.Name.Equals(createDto.Name, StringComparison.OrdinalIgnoreCase) && p.IsActive))
                {
                    _logger.LogWarning("CreateProduct failed: duplicate product name {Name}", createDto.Name);
                    throw new BusinessRuleException($"A product with the name '{createDto.Name}' already exists.");
                }

                var product = createDto.ToEntity(_sanitizationService);
                product.Id = _nextId++;
                
                // Set audit properties
                var currentUser = _currentUserService.GetCurrentUserName();
                product.CreatedBy = currentUser;
                product.UpdatedBy = currentUser;
                product.CreatedAt = DateTime.UtcNow;
                product.UpdatedAt = DateTime.UtcNow;
                
                _products.Add(product);

                _logger.LogInformation("CreateProduct completed successfully. Created product: {ProductName} with ID={Id}", product.Name, product.Id);
                var responseDto = product.ToResponseDto();
                return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, responseDto);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in CreateProduct method for product: {Name}", createDto?.Name ?? "null");
            throw;
        }
        finally
        {
            stopwatch.Stop();
            _logger.LogInformation("CreateProduct method execution time: {ElapsedMilliseconds}ms", stopwatch.ElapsedMilliseconds);
        }
    }

    /// <summary>
    /// Updates an existing product
    /// </summary>
    /// <param name="id">Product ID</param>
    /// <param name="updateDto">Updated product data</param>
    /// <returns>No content</returns>
    /// <response code="204">Product updated successfully</response>
    /// <response code="400">Invalid product data</response>
    /// <response code="404">Product not found</response>
    /// <response code="422">Business rule violation (e.g., duplicate name)</response>
    [HttpPut("{id}")]
    public IActionResult UpdateProduct(int id, [FromBody] ProductUpdateDto updateDto)
    {
        var stopwatch = Stopwatch.StartNew();
        _logger.LogInformation("Entering UpdateProduct method with parameters: id={Id}, Name={Name}, Price={Price}", 
            id, updateDto?.Name, updateDto?.Price);
        
        try
        {
            if (updateDto == null)
            {
                _logger.LogWarning("UpdateProduct called with null updateDto for id={Id}", id);
                throw new ValidationException(new Dictionary<string, string[]> { ["updateDto"] = new[] { "Request body cannot be null" } });
            }

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("UpdateProduct validation failed for product id={Id}, Name={Name}", id, updateDto.Name);
                var errors = ModelState.ToDictionary(
                    kvp => kvp.Key,
                    kvp => kvp.Value?.Errors.Select(e => e.ErrorMessage).ToArray() ?? Array.Empty<string>()
                );
                throw new ValidationException(errors);
            }

            lock (_lock)
            {
                var existingProduct = _products.FirstOrDefault(p => p.Id == id && p.IsActive);
                if (existingProduct == null)
                {
                    _logger.LogWarning("UpdateProduct failed: Product not found with id={Id}", id);
                    throw new NotFoundException("Product", id);
                }

                // Check for unique name (excluding the current product)
                if (_products.Any(p => p.Id != id && p.Name.Equals(updateDto.Name, StringComparison.OrdinalIgnoreCase) && p.IsActive))
                {
                    _logger.LogWarning("UpdateProduct failed: duplicate product name {Name} for id={Id}", updateDto.Name, id);
                    throw new BusinessRuleException($"A product with the name '{updateDto.Name}' already exists.");
                }

                var oldName = existingProduct.Name;
                updateDto.UpdateEntity(existingProduct, _sanitizationService);
                
                // Set audit properties
                var currentUser = _currentUserService.GetCurrentUserName();
                existingProduct.UpdatedBy = currentUser;
                existingProduct.UpdatedAt = DateTime.UtcNow;
                
                _logger.LogInformation("UpdateProduct completed successfully. Updated product from {OldName} to {NewName} (id={Id}) by {User}", 
                    oldName, existingProduct.Name, id, currentUser);
            }

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in UpdateProduct method for id={Id}, Name={Name}", id, updateDto?.Name ?? "null");
            throw;
        }
        finally
        {
            stopwatch.Stop();
            _logger.LogInformation("UpdateProduct method execution time: {ElapsedMilliseconds}ms", stopwatch.ElapsedMilliseconds);
        }
    }

    /// <summary>
    /// Deletes a product (soft delete)
    /// </summary>
    /// <param name="id">Product ID</param>
    /// <returns>No content</returns>
    /// <response code="204">Product deleted successfully</response>
    /// <response code="404">Product not found</response>
    [HttpDelete("{id}")]
    public IActionResult DeleteProduct(int id)
    {
        var stopwatch = Stopwatch.StartNew();
        _logger.LogInformation("Entering DeleteProduct method with parameter: id={Id}", id);
        
        try
        {
            lock (_lock)
            {
                var product = _products.FirstOrDefault(p => p.Id == id && p.IsActive);
                if (product == null)
                {
                    _logger.LogWarning("DeleteProduct failed: Product not found with id={Id}", id);
                    throw new NotFoundException("Product", id);
                }

                var productName = product.Name;
                // Soft delete - mark as inactive
                product.IsActive = false;
                
                // Set audit properties
                var currentUser = _currentUserService.GetCurrentUserName();
                product.UpdatedBy = currentUser;
                product.UpdatedAt = DateTime.UtcNow;
                
                _logger.LogInformation("DeleteProduct completed successfully. Soft deleted product: {ProductName} (id={Id}) by {User}", productName, id, currentUser);
                return NoContent();
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in DeleteProduct method with parameter: id={Id}", id);
            throw;
        }
        finally
        {
            stopwatch.Stop();
            _logger.LogInformation("DeleteProduct method execution time: {ElapsedMilliseconds}ms", stopwatch.ElapsedMilliseconds);
        }
    }

    /// <summary>
    /// Searches products with filtering and pagination
    /// </summary>
    /// <param name="name">Filter by product name (partial match)</param>
    /// <param name="category">Filter by category (exact match)</param>
    /// <param name="minPrice">Minimum price filter</param>
    /// <param name="maxPrice">Maximum price filter</param>
    /// <param name="pageNumber">Page number (starting from 1)</param>
    /// <param name="pageSize">Number of items per page (max 100)</param>
    /// <returns>Filtered list of products</returns>
    /// <response code="200">Returns the filtered list of products</response>
    [HttpGet("search")]
    public ActionResult<IEnumerable<ProductResponseDto>> SearchProducts(
        [FromQuery] string? name,
        [FromQuery] string? category,
        [FromQuery] decimal? minPrice,
        [FromQuery] decimal? maxPrice,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10)
    {
        var stopwatch = Stopwatch.StartNew();
        _logger.LogInformation("Entering SearchProducts method with parameters: name={Name}, category={Category}, minPrice={MinPrice}, maxPrice={MaxPrice}, pageNumber={PageNumber}, pageSize={PageSize}", 
            name, category, minPrice, maxPrice, pageNumber, pageSize);
        
        try
        {
            if (pageNumber < 1) pageNumber = 1;
            if (pageSize < 1) pageSize = 10;
            if (pageSize > 100) pageSize = 100;

            lock (_lock)
            {
                var query = _products.Where(p => p.IsActive);

                if (!string.IsNullOrWhiteSpace(name))
                {
                    query = query.Where(p => p.Name.Contains(name, StringComparison.OrdinalIgnoreCase));
                }

                if (!string.IsNullOrWhiteSpace(category))
                {
                    query = query.Where(p => p.Category.Equals(category, StringComparison.OrdinalIgnoreCase));
                }

                if (minPrice.HasValue)
                {
                    query = query.Where(p => p.Price >= minPrice.Value);
                }

                if (maxPrice.HasValue)
                {
                    query = query.Where(p => p.Price <= maxPrice.Value);
                }

                var totalProducts = query.Count();
                var products = query
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToResponseDtos()
                    .ToList();

                Response.Headers["X-Total-Count"] = totalProducts.ToString();
                Response.Headers["X-Page-Number"] = pageNumber.ToString();
                Response.Headers["X-Page-Size"] = pageSize.ToString();

                _logger.LogInformation("SearchProducts completed successfully. Found {ProductCount} products out of {TotalProducts} matching criteria", products.Count, totalProducts);
                return Ok(products);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in SearchProducts method with parameters: name={Name}, category={Category}", name, category);
            throw;
        }
        finally
        {
            stopwatch.Stop();
            _logger.LogInformation("SearchProducts method execution time: {ElapsedMilliseconds}ms", stopwatch.ElapsedMilliseconds);
        }
    }

    /// <summary>
    /// Gets all available product categories
    /// </summary>
    /// <returns>List of categories</returns>
    /// <response code="200">Returns the list of categories</response>
    [HttpGet("categories")]
    public ActionResult<IEnumerable<string>> GetCategories()
    {
        var stopwatch = Stopwatch.StartNew();
        _logger.LogInformation("Entering GetCategories method");
        
        try
        {
            lock (_lock)
            {
                var categories = _products
                    .Where(p => p.IsActive)
                    .Select(p => p.Category)
                    .Distinct()
                    .OrderBy(c => c)
                    .ToList();

                _logger.LogInformation("GetCategories completed successfully. Found {CategoryCount} categories", categories.Count);
                return Ok(categories);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in GetCategories method");
            throw;
        }
        finally
        {
            stopwatch.Stop();
            _logger.LogInformation("GetCategories method execution time: {ElapsedMilliseconds}ms", stopwatch.ElapsedMilliseconds);
        }
    }
}