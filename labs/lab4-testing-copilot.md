# Lab 4: Testing with Copilot - Hands-on Exercise

**Duration:** 40 minutes

## Prerequisites

* Visual Studio 2022 with GitHub Copilot enabled
* Your Web API project from Lab 3
   If you we're able to complete Lab 3, you can use the completed version (located at `completed\lab3\ProductInventoryAPI\` in the downloaded HOL repository)
* Basic familiarity with unit testing concepts

## Exercise Overview

Learn how to use Copilot to generate comprehensive unit tests, create test data and mocks, write integration tests, and handle edge cases—demonstrating test-driven development with AI assistance.

---

## Part 1: Add Test Project (3 minutes)

1. **Right-click on your solution** → Add → New Project

2. **Search for "xUnit Test Project"**

   * Select xUnit Test Project
   * Name: `ProductInventoryAPI.Tests`
   * Framework: .NET 9.0
   * Click `Create`

3. **Add project reference:**

   * Right-click on the test project → Add → Project Reference
   * Check `ProductInventoryAPI`
   * Click `OK`

4. **Install required NuGet packages:**

   * Right-click on test project → Manage NuGet Packages
   * Install: `Moq` (for mocking)
   * Install: `FluentAssertions` (for better assertions)

 5. **Update out-of-date NuGet packages:**

   * There may be four out-of-date packages in the test project
   * Update all packages to the latest version
---

## Part 2: Generate Unit Tests for Models (7 minutes)

### Step 1: Test Product Model Validation

1. **Create folder structure** in test project:

   * `Models` folder
   * `Controllers` folder
   * `Services` folder

2. **Add new class** `Models/ProductTests.cs`

3. **Type this in Chat:**

   ```csharp
   // Unit tests for Product model validation
   // Test cases needed:
   // - Valid product creation
   // - Name validation (null, empty, too long)
   // - Price validation (negative, zero, valid)
   // - Stock quantity validation (negative, valid)
   // Use FluentAssertions for assertions
   using Xunit;
   using FluentAssertions;
   using ProductInventoryAPI.Models;
   ```

4. **Press Enter** and let Copilot generate the test class structure.
   Add the completed code to the `ProductTests.cs` file.

5. **Add specific test method comments:**

   ```csharp
   // Test that product with valid data passes all validations
   
   // Test that product name cannot be null or empty
   
   // Test that product price must be greater than zero
   
   // Test that stock quantity cannot be negative
   ```

6. **Let Copilot generate each test method**

### Step 2: Use Data-Driven Tests

1. **Add a comment for theory tests:**

   ```csharp
   // Theory test for invalid product names with inline data
   // Test cases: null, empty string, whitespace, string over 100 chars
   ```

2. **Notice how Copilot:**

   * Generates `[Theory]` attribute
   * Adds `[InlineData]` for each test case
   * Creates parameterized test method

---

## Part 3: Generate Controller Unit Tests (10 minutes)

### Step 1: Create Controller Test Class

1. **Add new class** `Controllers/ProductsControllerTests.cs`

2. **Start with this comprehensive comment in Chat:**

   ```csharp
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
   using ProductInventoryAPI.Controllers;
   using ProductInventoryAPI.Models;
   using ProductInventoryAPI.DTOs;
   ```

3. **Let Copilot generate the test class setup**

### Step 2: Generate Individual Test Methods

1. **Add test methods with comments as needed:**

   ```csharp
   // Test GET all products returns OK with product list
   
   // Test GET product by ID returns OK when product exists
   
   // Test GET product by ID returns NotFound when product doesn't exist
   
   // Test POST creates product and returns CreatedAtAction
   
   // Test POST returns BadRequest for invalid model state
   ```

2. **For each test, observe how Copilot:**

   * Sets up mocks appropriately
   * Arranges test data
   * Calls the controller method
   * Asserts the expected results

### Step 3: Test Edge Cases

1. **Add edge case tests:**

   ```csharp
   // Test GET all products with pagination returns correct subset
   
   // Test PUT updates only specified fields
   
   // Test DELETE of non-existent product returns NotFound
   
   // Test concurrent updates handle conflicts appropriately
   ```

---

## Part 4: Create Test Data Builders (5 minutes)

### Step 1: Generate Test Data Builder

1. **Create new folder** `Builders` in test project

2. **Add new class** `Builders/ProductBuilder.cs`

3. **Type this comment in Chat:**

   ```csharp
   // Fluent builder pattern for creating test Product objects
   // Should support:
   // - Default valid product
   // - Method chaining for setting properties
   // - Build method to create the product
   // - Multiple preset configurations (ValidProduct, InvalidProduct, ExpensiveProduct)
   ```

4. **Let Copilot generate the builder pattern**

### Step 2: Use Builder in Tests

1. **Go back to ProductsControllerTests.cs**

2. **Add a test using the builder:**

   ```csharp
   // Test POST with product created using builder pattern
   ```

3. **Notice how Copilot uses (should? it's AI after all) your builder:**

   ```csharp
   var product = new ProductBuilder()
       .WithName("Test Product")
       .WithPrice(99.99m)
       .Build();
   ```

---

## Part 5: Generate Integration Tests (8 minutes)

### Step 1: Create Integration Test Class

1. **Add new class** `Integration/ProductApiIntegrationTests.cs`

2. **Type this setup comment in Chat:**

   ```csharp
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
   ```

3. **Add test factory setup:**

   ```csharp
   // Create test class that implements IClassFixture<WebApplicationFactory<Program>>
   // Initialize HttpClient in constructor
   ```

### Step 2: Generate HTTP Tests

1. **Add integration test methods:**

   ```csharp
   // Test full product creation flow:
   // POST new product, GET by ID, verify data matches
   
   // Test API validation returns proper error response format
   
   // Test pagination works correctly with multiple products
   
   // Test concurrent requests handle properly
   ```

2. **For the POST test, add helper:**

   ```csharp
   // Helper method to serialize object to JSON StringContent
   ```

### Step 3: Test Error Scenarios

1. **Add error handling tests:**

   ```csharp
   // Test 404 response includes proper error message format
   
   // Test 400 response includes validation errors
   
   // Test 500 response is handled by global error handler
   ```

---

## Part 6: Test-Driven Development Demo (5 minutes)

### Step 1: Write Test First

1. **Add a new test for a feature that doesn't exist:**

   ```csharp
   // Test that products can be filtered by category
   // GET /api/products?category=Electronics should return only electronics
   ```

2. **Let Copilot generate the test**
   * The test will fail since the feature doesn't exist

### Step 2: Implement the Feature

1. **Go to ProductsController.cs**

2. **Select the GET all method**

3. **In Copilot Chat, type:**

   ```
   Add a category parameter to filter products. If category is provided, return only products matching that category.
   ```

4. **Apply the changes and run the test again**

### Step 3: Generate Edge Case Tests

1. **Back in the test file, add:**

   ```csharp
   // Test filter with non-existent category returns empty list
   
   // Test filter is case-insensitive
   
   // Test filter with null/empty category returns all products
   ```

---

## Running and Debugging Tests (2 minutes)

1. **Run all tests:**
   * Test → Run All Tests
   * Or use Test Explorer (Test → Test Explorer)

2. **Debug a specific test:**

   * Right-click on test method → Debug Test
   * Set breakpoints to inspect values

3. **Use Copilot Chat for test failures:**

   * Copy failing test output
   * Ask: "Why is this test failing and how do I fix it?"

---

## Part 7: Debugging with Copilot (10 minutes)

### Step 1: Introduction to Debug with Copilot

GitHub Copilot can help you understand and fix bugs more efficiently by analyzing exceptions, suggesting fixes, and explaining complex call stacks.

1. **Create a bug scenario** in `ProductsController.cs`:

   ```csharp
   // Add this method to ProductsController to simulate a bug
   [HttpGet("search")]
   public async Task<ActionResult<IEnumerable<ProductDTO>>> SearchProducts(string term)
   {
       // Intentional bug: null reference exception
       var results = _products.Where(p => p.Name.Contains(term));
       return Ok(results.Select(p => new ProductDTO
       {
           Id = p.Id,
           Name = p.Name,
           Price = p.Price
       }));
   }
   ```

### Step 2: Write a Test that Fails

1. **Add this test to** `ProductsControllerTests.cs`:

   ```csharp
   [Fact]
   public async Task SearchProducts_WithNullTerm_ShouldReturnAllProducts()
   {
       // Arrange
       var products = new List<Product>
       {
           new Product { Id = 1, Name = "Laptop", Price = 999.99m },
           new Product { Id = 2, Name = "Mouse", Price = 29.99m }
       };
       // Mock setup here...
       
       // Act
       var result = await _controller.SearchProducts(null);
       
       // Assert
       result.Should().BeOfType<OkObjectResult>();
   }
   ```

2. **Run the test** - it will fail with a NullReferenceException

### Step 3: Use Debug with Copilot

1. **When the test fails:**

   * Click on the test failure in Test Explorer
   * Look for the **"Debug with Copilot"** option in the context menu
   * Or use the lightbulb icon that appears next to the exception

2. **Copilot will analyze:**

   * The exception type and message
   * The call stack
   * The surrounding code context
   * Suggest potential fixes

3. **Example Copilot response:**

   ```
   The NullReferenceException occurs because when 'term' is null, 
   the Contains method is called on it. Here's how to fix it:
   
   Option 1: Add null check
   Option 2: Use null-conditional operator
   Option 3: Default to empty string
   ```

### Step 4: Apply and Verify the Fix

1. **Select Copilot's suggested fix** (usually Option 2):

   ```csharp
   var results = _products.Where(p => 
       term == null || p.Name.Contains(term, StringComparison.OrdinalIgnoreCase));
   ```

2. **Re-run the test** - it should now pass

### Step 5: Debug Complex Scenarios

1. **Create a more complex bug** - Add to ProductsController:

   ```csharp
   [HttpPost("bulk")]
   public async Task<ActionResult> BulkUpdate(List<ProductDTO> products)
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
                   Id = dto.Id, 
                   Name = dto.Name, 
                   Price = dto.Price 
               });
           }
       }
       return Ok();
   }
   ```

2. **When debugging this with Copilot:**

   * Copilot will explain the concurrent modification issue
   * Suggest using ToList() or alternative collection methods
   * Provide thread-safe alternatives

### Step 6: Understanding Call Stacks

1. **Create a deep call stack scenario:**

   ```csharp
   // Add a test that causes a stack overflow or deep exception
   [Fact]
   public void DeepCallStack_ThrowsException_CopilotExplainsStack()
   {
       // Test with recursive method call
       // When it fails, use Debug with Copilot
   }
   ```

2. **Copilot helps by:**

   * Simplifying complex stack traces
   * Identifying the root cause
   * Explaining each frame's purpose
   * Suggesting where to set breakpoints

### Best Practices for Debugging with Copilot

1. **Provide context:** Include relevant error messages and stack traces
2. **Ask specific questions:** "Why does this throw NullReferenceException?"
3. **Request explanations:** "Explain what this call stack means"
4. **Get fix suggestions:** "How do I fix this ConcurrentModificationException?"
5. **Verify fixes:** Always test Copilot's suggestions with unit tests

---

## Key Takeaways

✅ **You've learned to:**

* Generate unit tests with comprehensive coverage
* Create mock objects and test data
* Write data-driven tests with theories
* Build integration tests for APIs
* Practice test-driven development with Copilot
* Generate edge case scenarios

## Best Practices for Testing with Copilot

1. **Describe test intent clearly** in comments
2. **Include test categories** (happy path, edge cases, error cases)
3. **Specify assertion library** preferences
4. **Generate arrange-act-assert** structure
5. **Ask for multiple test scenarios** for each method

## Challenge Extensions (Optional)

1. **Performance Tests:**

   ```csharp
   // Test that GET all products responds within 100ms
   ```

2. **Security Tests:**

   ```csharp
   // Test that API rejects requests with SQL injection attempts
   ```

3. **Load Tests:**

   ```csharp
   // Test API handles 100 concurrent requests
   ```

## Test Organization Tips

* **Naming Convention**: Test_Scenario_ExpectedResult
* **Folder Structure**: Mirror your main project
* **Test Data**: Use builders for complex objects
* **Assertions**: Be specific about what you're testing

## Troubleshooting

**Tests not discovered?**

* Ensure test project targets same framework
* Check test method has `[Fact]` or `[Theory]`
* Rebuild solution

**Mocking issues?**

* Ask Copilot: "Show me how to mock this interface"
* Ensure Moq is properly installed

**Assertion failures?**

* Use FluentAssertions for clearer error messages
* Debug test to inspect actual values
