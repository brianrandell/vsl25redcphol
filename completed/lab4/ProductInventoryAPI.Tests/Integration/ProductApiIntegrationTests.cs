// Integration tests for Product API endpoints
// Uses WebApplicationFactory to test full HTTP pipeline
// Tests actual HTTP requests and responses
// Includes database setup and teardown
using Xunit;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http;
using System.Net;
using FluentAssertions;
using System.Text;
using System.Text.Json;
using ProductInventoryAPI.DTOs;
using ProductInventoryAPI.Tests.Builders;
using ProductInventoryAPI.Controllers;

namespace ProductInventoryAPI.Tests.Integration;

public class ProductApiIntegrationTests : IClassFixture<TestWebApplicationFactory<Program>>
{
    private readonly HttpClient _client;
    private readonly JsonSerializerOptions _jsonOptions;

    public ProductApiIntegrationTests(TestWebApplicationFactory<Program> factory)
    {
        // Reset static data before each integration test
        ProductsController.ResetDataForTesting();
        
        _client = factory.CreateClient();
        _jsonOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
    }

    [Fact]
    public async Task GetProducts_ShouldReturnOkWithProducts()
    {
        // Act
        var response = await _client.GetAsync("/api/products");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        response.Content.Headers.ContentType?.MediaType.Should().Be("application/json");
        
        var content = await response.Content.ReadAsStringAsync();
        var products = JsonSerializer.Deserialize<List<ProductResponseDto>>(content, _jsonOptions);
        
        products.Should().NotBeNull();
        products!.Should().NotBeEmpty();
        products.Should().HaveCount(3); // Default test data
    }

    [Fact]
    public async Task GetProduct_WithValidId_ShouldReturnProduct()
    {
        // Arrange
        var productId = 1;

        // Act
        var response = await _client.GetAsync($"/api/products/{productId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        
        var content = await response.Content.ReadAsStringAsync();
        var product = JsonSerializer.Deserialize<ProductResponseDto>(content, _jsonOptions);
        
        product.Should().NotBeNull();
        product.Id.Should().Be(productId);
        product.Name.Should().Be("Laptop");
    }

    [Fact]
    public async Task GetProduct_WithInvalidId_ShouldReturn404()
    {
        // Arrange
        var invalidId = 999;

        // Act
        var response = await _client.GetAsync($"/api/products/{invalidId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        
        var content = await response.Content.ReadAsStringAsync();
        content.Should().Contain("Product with ID 999 was not found");
    }

    [Fact]
    public async Task PostProduct_WithValidData_ShouldCreateProduct()
    {
        // Arrange
        var createDto = ProductBuilder.ValidProduct()
            .WithName("Integration Test Product")
            .WithPrice(99.99m)
            .BuildCreateDto();

        var json = JsonSerializer.Serialize(createDto, _jsonOptions);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        // Act
        var response = await _client.PostAsync("/api/products", content);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        response.Headers.Location.Should().NotBeNull();
        
        var responseContent = await response.Content.ReadAsStringAsync();
        var createdProduct = JsonSerializer.Deserialize<ProductResponseDto>(responseContent, _jsonOptions);
        
        createdProduct.Should().NotBeNull();
        createdProduct.Name.Should().Be("Integration Test Product");
        createdProduct.Price.Should().Be(99.99m);
        createdProduct.IsActive.Should().BeTrue();

        // Verify the product was actually created by fetching it
        var getResponse = await _client.GetAsync($"/api/products/{createdProduct.Id}");
        getResponse.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task PostProduct_WithInvalidData_ShouldReturn400()
    {
        // Arrange
        var invalidProduct = new
        {
            name = "", // Invalid: empty name
            description = "Test description",
            price = -1, // Invalid: negative price
            stockQuantity = -5, // Invalid: negative stock
            category = ""
        };

        var json = JsonSerializer.Serialize(invalidProduct, _jsonOptions);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        // Act
        var response = await _client.PostAsync("/api/products", content);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        
        var responseContent = await response.Content.ReadAsStringAsync();
        responseContent.Should().Contain("validation");
    }

    [Fact]
    public async Task PostProduct_WithDuplicateName_ShouldReturn422()
    {
        // Arrange
        var duplicateProduct = ProductBuilder.ValidProduct()
            .WithName("Laptop") // This name already exists
            .BuildCreateDto();

        var json = JsonSerializer.Serialize(duplicateProduct, _jsonOptions);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        // Act
        var response = await _client.PostAsync("/api/products", content);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.UnprocessableEntity);
        
        var responseContent = await response.Content.ReadAsStringAsync();
        responseContent.Should().Contain("already exists");
    }

    [Fact]
    public async Task PutProduct_WithValidData_ShouldUpdateProduct()
    {
        // Arrange
        var productId = 1;
        var updateDto = ProductBuilder.ValidProduct()
            .WithName("Updated Integration Product")
            .WithPrice(1299.99m)
            .BuildUpdateDto();

        var json = JsonSerializer.Serialize(updateDto, _jsonOptions);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        // Act
        var response = await _client.PutAsync($"/api/products/{productId}", content);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);

        // Verify the product was actually updated by fetching it
        var getResponse = await _client.GetAsync($"/api/products/{productId}");
        getResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        
        var getContent = await getResponse.Content.ReadAsStringAsync();
        var updatedProduct = JsonSerializer.Deserialize<ProductResponseDto>(getContent, _jsonOptions);
        
        updatedProduct!.Name.Should().Be("Updated Integration Product");
        updatedProduct.Price.Should().Be(1299.99m);
    }

    [Fact]
    public async Task PutProduct_WithInvalidId_ShouldReturn404()
    {
        // Arrange
        var invalidId = 999;
        var updateDto = ProductBuilder.ValidProduct().BuildUpdateDto();

        var json = JsonSerializer.Serialize(updateDto, _jsonOptions);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        // Act
        var response = await _client.PutAsync($"/api/products/{invalidId}", content);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task DeleteProduct_WithValidId_ShouldSoftDeleteProduct()
    {
        // Arrange
        var productId = 2; // Use Mouse product

        // Act
        var response = await _client.DeleteAsync($"/api/products/{productId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);

        // Verify the product is no longer available (soft deleted)
        var getResponse = await _client.GetAsync($"/api/products/{productId}");
        getResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task DeleteProduct_WithInvalidId_ShouldReturn404()
    {
        // Arrange
        var invalidId = 999;

        // Act
        var response = await _client.DeleteAsync($"/api/products/{invalidId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task GetProducts_WithPagination_ShouldReturnCorrectHeaders()
    {
        // Act
        var response = await _client.GetAsync("/api/products?pageNumber=1&pageSize=2");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        response.Headers.Should().ContainKey("X-Total-Count");
        response.Headers.Should().ContainKey("X-Page-Number");
        response.Headers.Should().ContainKey("X-Page-Size");
        
        response.Headers.GetValues("X-Page-Number").First().Should().Be("1");
        response.Headers.GetValues("X-Page-Size").First().Should().Be("2");
    }

    [Fact]
    public async Task SearchProducts_WithCategoryFilter_ShouldReturnFilteredResults()
    {
        // Act
        var response = await _client.GetAsync("/api/products/search?category=Electronics");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        
        var content = await response.Content.ReadAsStringAsync();
        var products = JsonSerializer.Deserialize<List<ProductResponseDto>>(content, _jsonOptions);
        
        products.Should().NotBeNull();
        products.Should().AllSatisfy(p => p.Category.Should().Be("Electronics"));
    }

    [Fact]
    public async Task SearchProducts_WithPriceRange_ShouldReturnFilteredResults()
    {
        // Act
        var response = await _client.GetAsync("/api/products/search?minPrice=30&maxPrice=100");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        
        var content = await response.Content.ReadAsStringAsync();
        var products = JsonSerializer.Deserialize<List<ProductResponseDto>>(content, _jsonOptions);
        
        products.Should().NotBeNull();
        products.Should().AllSatisfy(p => 
        {
            p.Price.Should().BeGreaterThanOrEqualTo(30m);
            p.Price.Should().BeLessThanOrEqualTo(100m);
        });
    }

    [Fact]
    public async Task GetCategories_ShouldReturnAvailableCategories()
    {
        // Act
        var response = await _client.GetAsync("/api/products/categories");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        
        var content = await response.Content.ReadAsStringAsync();
        var categories = JsonSerializer.Deserialize<List<string>>(content, _jsonOptions);
        
        categories.Should().NotBeNull();
        categories.Should().Contain("Electronics");
        categories.Should().BeInAscendingOrder();
    }

    [Fact]
    public async Task FullProductLifecycle_ShouldWorkCorrectly()
    {
        // Create
        var createDto = ProductBuilder.ValidProduct()
            .WithName("Lifecycle Test Product")
            .WithPrice(199.99m)
            .BuildCreateDto();

        var createJson = JsonSerializer.Serialize(createDto, _jsonOptions);
        var createContent = new StringContent(createJson, Encoding.UTF8, "application/json");
        var createResponse = await _client.PostAsync("/api/products", createContent);
        
        createResponse.StatusCode.Should().Be(HttpStatusCode.Created);
        var createdContent = await createResponse.Content.ReadAsStringAsync();
        var createdProduct = JsonSerializer.Deserialize<ProductResponseDto>(createdContent, _jsonOptions);
        var productId = createdProduct!.Id;

        // Read
        var getResponse = await _client.GetAsync($"/api/products/{productId}");
        getResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        // Update
        var updateDto = ProductBuilder.ValidProduct()
            .WithName("Updated Lifecycle Product")
            .WithPrice(249.99m)
            .BuildUpdateDto();

        var updateJson = JsonSerializer.Serialize(updateDto, _jsonOptions);
        var updateContent = new StringContent(updateJson, Encoding.UTF8, "application/json");
        var updateResponse = await _client.PutAsync($"/api/products/{productId}", updateContent);
        
        updateResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);

        // Verify Update
        var getUpdatedResponse = await _client.GetAsync($"/api/products/{productId}");
        var getUpdatedContent = await getUpdatedResponse.Content.ReadAsStringAsync();
        var updatedProduct = JsonSerializer.Deserialize<ProductResponseDto>(getUpdatedContent, _jsonOptions);
        updatedProduct!.Name.Should().Be("Updated Lifecycle Product");
        updatedProduct.Price.Should().Be(249.99m);

        // Delete
        var deleteResponse = await _client.DeleteAsync($"/api/products/{productId}");
        deleteResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);

        // Verify Delete
        var getDeletedResponse = await _client.GetAsync($"/api/products/{productId}");
        getDeletedResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task ConcurrentRequests_ShouldHandleCorrectly()
    {
        // Arrange
        var tasks = new List<Task<HttpResponse>>();
        
        // Create multiple concurrent GET requests
        for (int i = 0; i < 10; i++)
        {
            tasks.Add(GetAsync("/api/products"));
        }

        // Act
        var responses = await Task.WhenAll(tasks);

        // Assert
        responses.Should().AllSatisfy(response => 
            response.StatusCode.Should().Be(HttpStatusCode.OK));
    }

    private async Task<HttpResponse> GetAsync(string requestUri)
    {
        var response = await _client.GetAsync(requestUri);
        var content = await response.Content.ReadAsStringAsync();
        return new HttpResponse { StatusCode = response.StatusCode, Content = content };
    }

    private class HttpResponse
    {
        public HttpStatusCode StatusCode { get; set; }
        public string Content { get; set; } = string.Empty;
    }
}