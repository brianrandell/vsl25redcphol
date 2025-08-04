using AdvancedCopilotAPI.Data;
using AdvancedCopilotAPI.Models;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;

namespace AdvancedCopilotAPI.Tests
{
    public class IntegrationTests : IClassFixture<TestWebApplicationFactory<Program>>
    {
        private readonly TestWebApplicationFactory<Program> _factory;
        private readonly HttpClient _client;

        public IntegrationTests(TestWebApplicationFactory<Program> factory)
        {
            _factory = factory;
            _client = _factory.CreateClient();
        }

        [Fact]
        public async Task GetProducts_ReturnsSuccessAndProducts()
        {
            var response = await _client.GetAsync("/api/products");
            
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            
            var content = await response.Content.ReadAsStringAsync();
            var products = JsonSerializer.Deserialize<List<Product>>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
            
            products.Should().NotBeNull();
            products.Should().HaveCountGreaterThan(0);
        }

        [Fact]
        public async Task GetProduct_WithValidId_ReturnsProduct()
        {
            var response = await _client.GetAsync("/api/products/1");
            
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            
            var content = await response.Content.ReadAsStringAsync();
            var product = JsonSerializer.Deserialize<Product>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
            
            product.Should().NotBeNull();
            product!.Id.Should().Be(1);
        }

        [Fact]
        public async Task GetProduct_WithInvalidId_ReturnsNotFound()
        {
            var response = await _client.GetAsync("/api/products/999");
            
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task CreateProduct_WithValidData_ReturnsCreated()
        {
            var newProduct = new Product
            {
                Name = "Test Product",
                Description = "Test Description",
                Price = 99.99m,
                Stock = 10,
                Category = "Test"
            };

            var response = await _client.PostAsJsonAsync("/api/products", newProduct);
            
            response.StatusCode.Should().Be(HttpStatusCode.Created);
            
            var content = await response.Content.ReadAsStringAsync();
            var createdProduct = JsonSerializer.Deserialize<Product>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
            
            createdProduct.Should().NotBeNull();
            createdProduct!.Name.Should().Be("Test Product");
            createdProduct.Id.Should().BeGreaterThan(0);
        }

        [Fact]
        public async Task CreateProduct_WithInvalidData_ReturnsBadRequest()
        {
            var invalidProduct = new Product
            {
                Name = "",
                Price = -1,
                Stock = -5
            };

            var response = await _client.PostAsJsonAsync("/api/products", invalidProduct);
            
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task UpdateProduct_WithValidData_ReturnsNoContent()
        {
            var updatedProduct = new Product
            {
                Id = 1,
                Name = "Updated Product",
                Description = "Updated Description",
                Price = 199.99m,
                Stock = 20,
                Category = "Updated"
            };

            var response = await _client.PutAsJsonAsync("/api/products/1", updatedProduct);
            
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        [Fact]
        public async Task UpdateProduct_WithMismatchedId_ReturnsBadRequest()
        {
            var product = new Product
            {
                Id = 999,
                Name = "Test Product"
            };

            var response = await _client.PutAsJsonAsync("/api/products/1", product);
            
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task DeleteProduct_WithValidId_ReturnsNoContent()
        {
            var response = await _client.DeleteAsync("/api/products/2");
            
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        [Fact]
        public async Task DeleteProduct_WithInvalidId_ReturnsNotFound()
        {
            var response = await _client.DeleteAsync("/api/products/999");
            
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task GetProductsByCategory_ReturnsFilteredProducts()
        {
            var response = await _client.GetAsync("/api/products/category/Electronics");
            
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            
            var content = await response.Content.ReadAsStringAsync();
            var products = JsonSerializer.Deserialize<List<Product>>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
            
            products.Should().NotBeNull();
            products.Should().AllSatisfy(p => p.Category.Should().Be("Electronics"));
        }

        [Fact]
        public async Task GetLowStockProducts_ReturnsProductsWithLowStock()
        {
            var response = await _client.GetAsync("/api/products/lowstock?threshold=10");
            
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            
            var content = await response.Content.ReadAsStringAsync();
            var products = JsonSerializer.Deserialize<List<Product>>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
            
            products.Should().NotBeNull();
            products.Should().AllSatisfy(p => p.Stock.Should().BeLessThanOrEqualTo(10));
        }

        [Fact]
        public async Task GetProductCount_ReturnsValidCount()
        {
            var response = await _client.GetAsync("/api/products/count");
            
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            
            var content = await response.Content.ReadAsStringAsync();
            var count = JsonSerializer.Deserialize<int>(content);
            
            count.Should().BeGreaterThan(0);
        }

        [Fact]
        public async Task CompleteProductLifecycle_WorksCorrectly()
        {
            var newProduct = new Product
            {
                Name = "Lifecycle Test Product",
                Description = "For testing complete lifecycle",
                Price = 149.99m,
                Stock = 50,
                Category = "Test"
            };

            var createResponse = await _client.PostAsJsonAsync("/api/products", newProduct);
            createResponse.StatusCode.Should().Be(HttpStatusCode.Created);
            
            var content = await createResponse.Content.ReadAsStringAsync();
            var createdProduct = JsonSerializer.Deserialize<Product>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
            
            createdProduct.Should().NotBeNull();
            var productId = createdProduct!.Id;

            var getResponse = await _client.GetAsync($"/api/products/{productId}");
            getResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            createdProduct.Name = "Updated Lifecycle Product";
            createdProduct.Price = 299.99m;
            
            var updateResponse = await _client.PutAsJsonAsync($"/api/products/{productId}", createdProduct);
            updateResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);

            var getUpdatedResponse = await _client.GetAsync($"/api/products/{productId}");
            getUpdatedResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            
            var updatedContent = await getUpdatedResponse.Content.ReadAsStringAsync();
            var updatedProduct = JsonSerializer.Deserialize<Product>(updatedContent, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
            
            updatedProduct!.Name.Should().Be("Updated Lifecycle Product");
            updatedProduct.Price.Should().Be(299.99m);

            var deleteResponse = await _client.DeleteAsync($"/api/products/{productId}");
            deleteResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);

            var getDeletedResponse = await _client.GetAsync($"/api/products/{productId}");
            getDeletedResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task PerformanceTest_MultipleRequests_CompletesWithinReasonableTime()
        {
            var stopwatch = System.Diagnostics.Stopwatch.StartNew();
            var tasks = new List<Task<HttpResponseMessage>>();

            for (int i = 0; i < 10; i++)
            {
                tasks.Add(_client.GetAsync("/api/products"));
            }

            var responses = await Task.WhenAll(tasks);
            stopwatch.Stop();

            responses.Should().AllSatisfy(r => r.StatusCode.Should().Be(HttpStatusCode.OK));
            stopwatch.ElapsedMilliseconds.Should().BeLessThan(5000);
        }

        [Fact]
        public async Task SecurityTest_XssAttempt_IsSanitized()
        {
            var maliciousProduct = new Product
            {
                Name = "<script>alert('xss')</script>Malicious Product",
                Description = "<img src=x onerror=alert('xss')>",
                Price = 99.99m,
                Stock = 10,
                Category = "Test"
            };

            var response = await _client.PostAsJsonAsync("/api/products", maliciousProduct);
            
            if (response.StatusCode == HttpStatusCode.Created)
            {
                var content = await response.Content.ReadAsStringAsync();
                var createdProduct = JsonSerializer.Deserialize<Product>(content, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
                
                createdProduct!.Name.Should().NotContain("<script>");
                createdProduct.Description.Should().NotContain("<img");
            }
        }
    }
}