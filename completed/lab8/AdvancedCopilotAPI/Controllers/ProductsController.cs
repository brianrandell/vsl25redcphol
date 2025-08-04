using AdvancedCopilotAPI.Interfaces;
using AdvancedCopilotAPI.Models;
using AdvancedCopilotAPI.Security;
using Microsoft.AspNetCore.Mvc;

namespace AdvancedCopilotAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<ProductsController> _logger;
        private readonly InputSanitizer _inputSanitizer;
        
        public ProductsController(IUnitOfWork unitOfWork, ILogger<ProductsController> logger, InputSanitizer inputSanitizer)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _inputSanitizer = inputSanitizer;
        }
        
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts(CancellationToken cancellationToken)
        {
            try
            {
                var products = await _unitOfWork.ProductRepository.GetAllProductsAsync(cancellationToken);
                return Ok(products);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving products");
                return StatusCode(500, "An error occurred while retrieving products");
            }
        }
        
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id, CancellationToken cancellationToken)
        {
            try
            {
                var product = await _unitOfWork.ProductRepository.GetProductByIdAsync(id, cancellationToken);
                
                if (product == null)
                {
                    return NotFound($"Product with ID {id} not found");
                }
                
                return Ok(product);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving product with ID {ProductId}", id);
                return StatusCode(500, "An error occurred while retrieving the product");
            }
        }
        
        [HttpGet("category/{category}")]
        public async Task<ActionResult<IEnumerable<Product>>> GetProductsByCategory(string category, CancellationToken cancellationToken)
        {
            try
            {
                var products = await _unitOfWork.ProductRepository.GetProductsByCategoryAsync(category, cancellationToken);
                return Ok(products);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving products for category {Category}", category);
                return StatusCode(500, "An error occurred while retrieving products by category");
            }
        }
        
        [HttpGet("lowstock")]
        public async Task<ActionResult<IEnumerable<Product>>> GetLowStockProducts([FromQuery] int threshold = 10, CancellationToken cancellationToken = default)
        {
            try
            {
                var products = await _unitOfWork.ProductRepository.GetLowStockProductsAsync(threshold, cancellationToken);
                return Ok(products);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving low stock products with threshold {Threshold}", threshold);
                return StatusCode(500, "An error occurred while retrieving low stock products");
            }
        }
        
        [HttpPost]
        public async Task<ActionResult<Product>> CreateProduct(Product product, CancellationToken cancellationToken)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                
                // Sanitize the input
                _inputSanitizer.ValidateAndSanitize(product);
                
                var createdProduct = await _unitOfWork.ProductRepository.CreateProductAsync(product, cancellationToken);
                
                return CreatedAtAction(nameof(GetProduct), new { id = createdProduct.Id }, createdProduct);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating product");
                return StatusCode(500, "An error occurred while creating the product");
            }
        }
        
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, Product product, CancellationToken cancellationToken)
        {
            try
            {
                if (id != product.Id)
                {
                    return BadRequest("Product ID mismatch");
                }
                
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                
                // Sanitize the input
                _inputSanitizer.ValidateAndSanitize(product);
                
                var exists = await _unitOfWork.ProductRepository.ProductExistsAsync(id, cancellationToken);
                if (!exists)
                {
                    return NotFound($"Product with ID {id} not found");
                }
                
                await _unitOfWork.ProductRepository.UpdateProductAsync(product, cancellationToken);
                
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating product with ID {ProductId}", id);
                return StatusCode(500, "An error occurred while updating the product");
            }
        }
        
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id, CancellationToken cancellationToken)
        {
            try
            {
                var deleted = await _unitOfWork.ProductRepository.DeleteProductAsync(id, cancellationToken);
                
                if (!deleted)
                {
                    return NotFound($"Product with ID {id} not found");
                }
                
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting product with ID {ProductId}", id);
                return StatusCode(500, "An error occurred while deleting the product");
            }
        }
        
        [HttpGet("count")]
        public async Task<ActionResult<int>> GetProductCount(CancellationToken cancellationToken)
        {
            try
            {
                var count = await _unitOfWork.ProductRepository.GetProductCountAsync(cancellationToken);
                return Ok(count);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving product count");
                return StatusCode(500, "An error occurred while retrieving product count");
            }
        }
    }
}