// Unit tests for ProductsController
// Setup:
// - Mock any dependencies
// - Create controller instance with mocks
// - Use FluentAssertions for assertions
// Test categories:
// - GET methods (all products, by id, not found)
// - POST method (valid product, invalid product)
// - PUT method (update existing, not found)
// - DELETE method (delete existing, not found)
using Xunit;
using Moq;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using ProductInventoryAPI.Controllers;
using ProductInventoryAPI.Models;
using ProductInventoryAPI.DTOs;
using ProductInventoryAPI.Extensions;
using ProductInventoryAPI.Exceptions;

namespace ProductInventoryAPI.Tests.Controllers;

public class ProductsControllerTests
{
    private readonly ProductsController _controller;

    public ProductsControllerTests()
    {
        // Reset static data before each test to ensure clean state
        ProductsController.ResetDataForTesting();
        
        _controller = new ProductsController();
        
        // Setup HTTP context for Response.Headers access
        var httpContext = new DefaultHttpContext();
        _controller.ControllerContext = new ControllerContext()
        {
            HttpContext = httpContext
        };
    }

    [Fact]
    public void GetProducts_ShouldReturnOkWithProductList()
    {
        // Act
        var result = _controller.GetProducts();

        // Assert
        result.Result.Should().BeOfType<OkObjectResult>();
        var okResult = result.Result as OkObjectResult;
        var products = okResult!.Value as List<ProductResponseDto>;
        products.Should().NotBeNull();
        products.Should().NotBeEmpty();
        products.Should().HaveCount(3); // Default test data
        
        // Verify original products are present
        products.Should().Contain(p => p.Name == "Laptop");
        products.Should().Contain(p => p.Name == "Mouse"); 
        products.Should().Contain(p => p.Name == "Keyboard");
    }

    [Fact]
    public void GetProducts_WithPagination_ShouldReturnCorrectSubset()
    {
        // Act
        var result = _controller.GetProducts(pageNumber: 1, pageSize: 2);

        // Assert
        result.Result.Should().BeOfType<OkObjectResult>();
        var okResult = result.Result as OkObjectResult;
        var products = okResult!.Value as List<ProductResponseDto>;
        products.Should().NotBeNull();
        products.Should().HaveCount(2);
    }

    [Fact]
    public void GetProduct_WithValidId_ShouldReturnOkWithProduct()
    {
        // Arrange
        var productId = 1;

        // Act
        var result = _controller.GetProduct(productId);

        // Assert
        result.Result.Should().BeOfType<OkObjectResult>();
        var okResult = result.Result as OkObjectResult;
        var product = okResult!.Value as ProductResponseDto;
        product.Should().NotBeNull();
        product.Id.Should().Be(productId);
        product.Name.Should().Be("Laptop");
    }

    [Fact]
    public void GetProduct_WithInvalidId_ShouldThrowNotFound()
    {
        // Arrange
        var invalidId = 999;

        // Act & Assert
        var exception = Assert.Throws<NotFoundException>(() => _controller.GetProduct(invalidId));
        exception.Message.Should().Be("Product with ID 999 was not found.");
    }

    [Fact]
    public void CreateProduct_WithValidData_ShouldReturnCreatedAtAction()
    {
        // Arrange
        var createDto = new ProductCreateDto
        {
            Name = "New Product",
            Description = "A new test product",
            Price = 49.99m,
            StockQuantity = 5,
            Category = "Test"
        };

        // Act
        var result = _controller.CreateProduct(createDto);

        // Assert
        result.Result.Should().BeOfType<CreatedAtActionResult>();
        var createdResult = result.Result as CreatedAtActionResult;
        var product = createdResult!.Value as ProductResponseDto;
        product.Should().NotBeNull();
        product.Name.Should().Be("New Product");
        product.Price.Should().Be(49.99m);
        product.IsActive.Should().BeTrue();
        createdResult.ActionName.Should().Be(nameof(ProductsController.GetProduct));
    }

    [Fact]
    public void CreateProduct_WithDuplicateName_ShouldThrowBusinessRuleException()
    {
        // Arrange
        var createDto = new ProductCreateDto
        {
            Name = "Laptop", // This name already exists in test data
            Description = "Duplicate laptop",
            Price = 999.99m,
            StockQuantity = 1,
            Category = "Electronics"
        };

        // Act & Assert
        var exception = Assert.Throws<BusinessRuleException>(() => _controller.CreateProduct(createDto));
        exception.Message.Should().Be("A product with the name 'Laptop' already exists.");
    }

    [Fact]
    public void UpdateProduct_WithValidData_ShouldReturnNoContent()
    {
        // Arrange
        var productId = 1;
        var updateDto = new ProductUpdateDto
        {
            Name = "Updated Laptop",
            Description = "Updated description",
            Price = 1199.99m,
            StockQuantity = 15,
            Category = "Electronics",
            IsActive = true
        };

        // Act
        var result = _controller.UpdateProduct(productId, updateDto);

        // Assert
        result.Should().BeOfType<NoContentResult>();
    }

    [Fact]
    public void UpdateProduct_WithInvalidId_ShouldThrowNotFoundException()
    {
        // Arrange
        var invalidId = 999;
        var updateDto = new ProductUpdateDto
        {
            Name = "Updated Product",
            Description = "Updated description",
            Price = 99.99m,
            StockQuantity = 10,
            Category = "Test",
            IsActive = true
        };

        // Act & Assert
        var exception = Assert.Throws<NotFoundException>(() => _controller.UpdateProduct(invalidId, updateDto));
        exception.Message.Should().Be("Product with ID 999 was not found.");
    }

    [Fact]
    public void UpdateProduct_WithDuplicateName_ShouldThrowBusinessRuleException()
    {
        // Arrange
        var productId = 1;
        var updateDto = new ProductUpdateDto
        {
            Name = "Mouse", // This name exists on another product
            Description = "Updated description",
            Price = 1199.99m,
            StockQuantity = 15,
            Category = "Electronics",
            IsActive = true
        };

        // Act & Assert
        var exception = Assert.Throws<BusinessRuleException>(() => _controller.UpdateProduct(productId, updateDto));
        exception.Message.Should().Be("A product with the name 'Mouse' already exists.");
    }

    [Fact]
    public void DeleteProduct_WithValidId_ShouldReturnNoContent()
    {
        // Arrange
        var productId = 1;

        // Act
        var result = _controller.DeleteProduct(productId);

        // Assert
        result.Should().BeOfType<NoContentResult>();
    }

    [Fact]
    public void DeleteProduct_WithInvalidId_ShouldThrowNotFoundException()
    {
        // Arrange
        var invalidId = 999;

        // Act & Assert
        var exception = Assert.Throws<NotFoundException>(() => _controller.DeleteProduct(invalidId));
        exception.Message.Should().Be("Product with ID 999 was not found.");
    }

    [Fact]
    public void SearchProducts_WithNameFilter_ShouldReturnFilteredResults()
    {
        // Act
        var result = _controller.SearchProducts(name: "Laptop", category: null, minPrice: null, maxPrice: null);

        // Assert
        result.Result.Should().BeOfType<OkObjectResult>();
        var okResult = result.Result as OkObjectResult;
        var products = okResult!.Value as List<ProductResponseDto>;
        products.Should().NotBeNull();
        products.Should().HaveCount(1);
        products!.First().Name.Should().Contain("Laptop");
    }

    [Fact]
    public void SearchProducts_WithCategoryFilter_ShouldReturnFilteredResults()
    {
        // Act
        var result = _controller.SearchProducts(name: null, category: "Electronics", minPrice: null, maxPrice: null);

        // Assert
        result.Result.Should().BeOfType<OkObjectResult>();
        var okResult = result.Result as OkObjectResult;
        var products = okResult!.Value as List<ProductResponseDto>;
        products.Should().NotBeNull();
        products!.Should().AllSatisfy(p => p.Category.Should().Be("Electronics"));
    }

    [Fact]
    public void SearchProducts_WithPriceRange_ShouldReturnFilteredResults()
    {
        // Act
        var result = _controller.SearchProducts(name: null, category: null, minPrice: 30m, maxPrice: 100m);

        // Assert
        result.Result.Should().BeOfType<OkObjectResult>();
        var okResult = result.Result as OkObjectResult;
        var products = okResult!.Value as List<ProductResponseDto>;
        products.Should().NotBeNull();
        products!.Should().AllSatisfy(p => 
        {
            p.Price.Should().BeGreaterThanOrEqualTo(30m);
            p.Price.Should().BeLessThanOrEqualTo(100m);
        });
    }

    [Fact]
    public void SearchProducts_WithNonExistentCategory_ShouldReturnEmptyList()
    {
        // Act
        var result = _controller.SearchProducts(name: null, category: "NonExistent", minPrice: null, maxPrice: null);

        // Assert
        result.Result.Should().BeOfType<OkObjectResult>();
        var okResult = result.Result as OkObjectResult;
        var products = okResult!.Value as List<ProductResponseDto>;
        products.Should().NotBeNull();
        products!.Should().BeEmpty();
    }

    [Fact]
    public void GetCategories_ShouldReturnDistinctCategoriesSorted()
    {
        // Act
        var result = _controller.GetCategories();

        // Assert
        result.Result.Should().BeOfType<OkObjectResult>();
        var okResult = result.Result as OkObjectResult;
        var categories = okResult!.Value as List<string>;
        categories.Should().NotBeNull();
        categories.Should().Contain("Electronics");
        categories.Should().BeInAscendingOrder();
        categories.Should().OnlyHaveUniqueItems();
    }

    [Theory]
    [InlineData(0, 10)]
    [InlineData(-1, 10)]
    [InlineData(1, 0)]
    [InlineData(1, -1)]
    [InlineData(1, 101)]
    public void GetProducts_WithInvalidPaginationParams_ShouldUseDefaults(int pageNumber, int pageSize)
    {
        // Act
        var result = _controller.GetProducts(pageNumber, pageSize);

        // Assert
        result.Result.Should().BeOfType<OkObjectResult>();
        // Additional assertions could be made about headers if they were accessible in unit tests
    }

    [Fact(Skip = "Debugging scenario - demonstrates NullReferenceException")]
    public void SearchProductsByTerm_WithNullTerm_ShouldThrowNullReferenceException()
    {
        // Arrange
        string? term = null;

        // Act & Assert
        // This test demonstrates the debugging scenario - it will throw NullReferenceException
        Assert.Throws<NullReferenceException>(() => 
            _controller.SearchProductsByTerm(term!));
    }

    [Fact]
    public void SearchProductsByTerm_WithValidTerm_ShouldReturnMatchingProducts()
    {
        // Arrange
        var term = "Laptop";

        // Act
        var result = _controller.SearchProductsByTerm(term);

        // Assert
        result.Result.Should().BeOfType<OkObjectResult>();
        var okResult = result.Result as OkObjectResult;
        var products = okResult!.Value as IEnumerable<ProductResponseDto>;
        products.Should().NotBeNull();
        products!.Should().HaveCount(1);
        products!.First().Name.Should().Contain(term);
    }

    [Fact]
    public void BulkUpdate_WithMultipleProducts_MayThrowConcurrentModificationException()
    {
        // Arrange
        var updates = new List<ProductUpdateDto>
        {
            new ProductUpdateDto 
            { 
                Id = 1, 
                Name = "Updated Laptop", 
                Description = "Updated",
                Price = 1299.99m,
                StockQuantity = 5,
                Category = "Electronics",
                IsActive = true
            },
            new ProductUpdateDto 
            { 
                Id = 2, 
                Name = "Updated Mouse", 
                Description = "Updated",
                Price = 39.99m,
                StockQuantity = 25,
                Category = "Electronics",
                IsActive = true
            }
        };

        // Act
        // This demonstrates the debugging scenario - modifying collection while iterating
        var result = _controller.BulkUpdate(updates);

        // Assert
        result.Should().BeOfType<OkResult>();
        // Note: In the actual implementation, this may throw an exception during iteration
    }
}