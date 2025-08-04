using Microsoft.AspNetCore.Mvc;
using ProductInventoryAPI.DTOs;
using ProductInventoryAPI.Exceptions;
using ProductInventoryAPI.Extensions;
using ProductInventoryAPI.Models;

namespace ProductInventoryAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[ApiVersion("1.0")]
public class ProductsController : ControllerBase
{
    private static readonly object _lock = new object();
    private static readonly List<Product> _products = new()
    {
        new Product { Id = 1, Name = "Laptop", Description = "High-performance laptop", Price = 999.99m, StockQuantity = 10, Category = "Electronics", IsActive = true },
        new Product { Id = 2, Name = "Mouse", Description = "Wireless mouse", Price = 29.99m, StockQuantity = 50, Category = "Electronics", IsActive = true },
        new Product { Id = 3, Name = "Keyboard", Description = "Mechanical keyboard", Price = 79.99m, StockQuantity = 25, Category = "Electronics", IsActive = true }
    };

    private static int _nextId = 4;

#if DEBUG
    // Internal method for test purposes only - allows resetting static state
    internal static void ResetDataForTesting()
    {
        lock (_lock)
        {
            _products.Clear();
            _products.AddRange(new[]
            {
                new Product { Id = 1, Name = "Laptop", Description = "High-performance laptop", Price = 999.99m, StockQuantity = 10, Category = "Electronics", IsActive = true },
                new Product { Id = 2, Name = "Mouse", Description = "Wireless mouse", Price = 29.99m, StockQuantity = 50, Category = "Electronics", IsActive = true },
                new Product { Id = 3, Name = "Keyboard", Description = "Mechanical keyboard", Price = 79.99m, StockQuantity = 25, Category = "Electronics", IsActive = true }
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

            return Ok(products);
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
        lock (_lock)
        {
            var product = _products.FirstOrDefault(p => p.Id == id && p.IsActive);
            if (product == null)
            {
                throw new NotFoundException("Product", id);
            }
            return Ok(product.ToResponseDto());
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
        if (!ModelState.IsValid)
        {
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
                throw new BusinessRuleException($"A product with the name '{createDto.Name}' already exists.");
            }

            var product = createDto.ToEntity();
            product.Id = _nextId++;
            _products.Add(product);

            var responseDto = product.ToResponseDto();
            return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, responseDto);
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
        if (!ModelState.IsValid)
        {
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
                throw new NotFoundException("Product", id);
            }

            // Check for unique name (excluding the current product)
            if (_products.Any(p => p.Id != id && p.Name.Equals(updateDto.Name, StringComparison.OrdinalIgnoreCase) && p.IsActive))
            {
                throw new BusinessRuleException($"A product with the name '{updateDto.Name}' already exists.");
            }

            updateDto.UpdateEntity(existingProduct);
        }

        return NoContent();
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
        lock (_lock)
        {
            var product = _products.FirstOrDefault(p => p.Id == id && p.IsActive);
            if (product == null)
            {
                throw new NotFoundException("Product", id);
            }

            // Soft delete - mark as inactive
            product.IsActive = false;

            return NoContent();
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

            return Ok(products);
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
        lock (_lock)
        {
            var categories = _products
                .Where(p => p.IsActive)
                .Select(p => p.Category)
                .Distinct()
                .OrderBy(c => c)
                .ToList();

            return Ok(categories);
        }
    }

    /// <summary>
    /// Searches products by term (debugging scenario - contains intentional bug)
    /// </summary>
    /// <param name="term">Search term</param>
    /// <returns>List of matching products</returns>
    [HttpGet("search-by-term")]
    public ActionResult<IEnumerable<ProductResponseDto>> SearchProductsByTerm(string term)
    {
        // Intentional bug: null reference exception when term is null
        // This will throw NullReferenceException if term is null
        var termLength = term.Length; // Force null reference here
        var results = _products.Where(p => p.Name.Contains(term)).ToList();
        return Ok(results.Select(p => new ProductResponseDto
        {
            Id = p.Id,
            Name = p.Name,
            Price = p.Price,
            Description = p.Description,
            Category = p.Category,
            StockQuantity = p.StockQuantity,
            IsActive = p.IsActive
        }));
    }

    /// <summary>
    /// Bulk update products (debugging scenario - contains intentional bug)
    /// </summary>
    /// <param name="products">List of products to update</param>
    /// <returns>Action result</returns>
    [HttpPost("bulk-update")]
    public ActionResult BulkUpdate(List<ProductUpdateDto> products)
    {
        // Complex bug: concurrent modification exception
        foreach (var dto in products)
        {
            var product = _products.FirstOrDefault(p => p.Id == dto.Id);
            if (product != null)
            {
                _products.Remove(product);
                _products.Add(new Product 
                { 
                    Id = dto.Id ?? 0, 
                    Name = dto.Name, 
                    Description = dto.Description,
                    Price = dto.Price,
                    StockQuantity = dto.StockQuantity,
                    Category = dto.Category,
                    IsActive = dto.IsActive
                });
            }
        }
        return Ok();
    }
}