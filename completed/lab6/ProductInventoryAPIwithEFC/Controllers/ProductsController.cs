using Microsoft.AspNetCore.Mvc;
using ProductInventoryAPI.DTOs;
using ProductInventoryAPI.Exceptions;
using ProductInventoryAPI.Extensions;
using ProductInventoryAPI.Models;
using ProductInventoryAPI.Repositories;

namespace ProductInventoryAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[ApiVersion("1.0")]
public class ProductsController : ControllerBase
{
    private readonly IProductRepository _repository;

    public ProductsController(IProductRepository repository)
    {
        _repository = repository;
    }

    /// <summary>
    /// Gets a paginated list of active products
    /// </summary>
    /// <param name="pageNumber">Page number (starting from 1)</param>
    /// <param name="pageSize">Number of items per page (max 100)</param>
    /// <returns>List of products</returns>
    /// <response code="200">Returns the list of products</response>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProductResponseDto>>> GetProducts([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    {
        if (pageNumber < 1) pageNumber = 1;
        if (pageSize < 1) pageSize = 10;
        if (pageSize > 100) pageSize = 100;

        var allProducts = await _repository.GetActiveProductsAsync();
        var totalProducts = allProducts.Count();
        var products = allProducts
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToResponseDtos()
            .ToList();

        Response.Headers["X-Total-Count"] = totalProducts.ToString();
        Response.Headers["X-Page-Number"] = pageNumber.ToString();
        Response.Headers["X-Page-Size"] = pageSize.ToString();

        return Ok(products);
    }

    /// <summary>
    /// Gets a specific product by ID
    /// </summary>
    /// <param name="id">Product ID</param>
    /// <returns>Product details</returns>
    /// <response code="200">Returns the product</response>
    /// <response code="404">Product not found</response>
    [HttpGet("{id}")]
    public async Task<ActionResult<ProductResponseDto>> GetProduct(int id)
    {
        var product = await _repository.GetByIdAsync(id);
        if (product == null || !product.IsActive)
        {
            throw new NotFoundException("Product", id);
        }
        return Ok(product.ToResponseDto());
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
    public async Task<ActionResult<ProductResponseDto>> CreateProduct([FromBody] ProductCreateDto createDto)
    {
        if (!ModelState.IsValid)
        {
            var errors = ModelState.ToDictionary(
                kvp => kvp.Key,
                kvp => kvp.Value?.Errors.Select(e => e.ErrorMessage).ToArray() ?? Array.Empty<string>()
            );
            throw new ValidationException(errors);
        }

        // Check for unique name
        if (await _repository.ExistsByNameAsync(createDto.Name))
        {
            throw new BusinessRuleException($"A product with the name '{createDto.Name}' already exists.");
        }

        var product = createDto.ToEntity();
        var createdProduct = await _repository.AddAsync(product);

        var responseDto = createdProduct.ToResponseDto();
        return CreatedAtAction(nameof(GetProduct), new { id = createdProduct.Id }, responseDto);
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
    public async Task<IActionResult> UpdateProduct(int id, [FromBody] ProductUpdateDto updateDto)
    {
        if (!ModelState.IsValid)
        {
            var errors = ModelState.ToDictionary(
                kvp => kvp.Key,
                kvp => kvp.Value?.Errors.Select(e => e.ErrorMessage).ToArray() ?? Array.Empty<string>()
            );
            throw new ValidationException(errors);
        }

        var existingProduct = await _repository.GetByIdAsync(id);
        if (existingProduct == null || !existingProduct.IsActive)
        {
            throw new NotFoundException("Product", id);
        }

        // Check for unique name (excluding the current product)
        if (await _repository.ExistsByNameAsync(updateDto.Name, id))
        {
            throw new BusinessRuleException($"A product with the name '{updateDto.Name}' already exists.");
        }

        updateDto.UpdateEntity(existingProduct);
        await _repository.UpdateAsync(existingProduct);

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
    public async Task<IActionResult> DeleteProduct(int id)
    {
        if (!await _repository.ExistsAsync(id))
        {
            throw new NotFoundException("Product", id);
        }

        await _repository.DeleteAsync(id);
        return NoContent();
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
    public async Task<ActionResult<IEnumerable<ProductResponseDto>>> SearchProducts(
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

        IEnumerable<Product> results;

        if (!string.IsNullOrWhiteSpace(name))
        {
            results = await _repository.SearchAsync(name);
        }
        else if (minPrice.HasValue || maxPrice.HasValue)
        {
            var min = minPrice ?? 0;
            var max = maxPrice ?? decimal.MaxValue;
            results = await _repository.GetProductsByPriceRangeAsync(min, max);
        }
        else
        {
            results = await _repository.GetActiveProductsAsync();
        }

        // Apply category filter
        if (!string.IsNullOrWhiteSpace(category))
        {
            results = results.Where(p => p.Category.Equals(category, StringComparison.OrdinalIgnoreCase));
        }

        var totalProducts = results.Count();
        var products = results
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToResponseDtos()
            .ToList();

        Response.Headers["X-Total-Count"] = totalProducts.ToString();
        Response.Headers["X-Page-Number"] = pageNumber.ToString();
        Response.Headers["X-Page-Size"] = pageSize.ToString();

        return Ok(products);
    }

    /// <summary>
    /// Gets all available product categories
    /// </summary>
    /// <returns>List of categories</returns>
    /// <response code="200">Returns the list of categories</response>
    [HttpGet("categories")]
    public async Task<ActionResult<IEnumerable<string>>> GetCategories()
    {
        var products = await _repository.GetActiveProductsAsync();
        var categories = products
            .Select(p => p.Category)
            .Distinct()
            .OrderBy(c => c)
            .ToList();

        return Ok(categories);
    }
}