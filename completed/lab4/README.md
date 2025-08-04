# Lab 4: Test-Driven Development with Copilot - API Testing Suite

This folder contains the completed implementation of Lab 4 from the GitHub Copilot training at VSLIVE! 2025 Redmond. This project demonstrates test-driven development (TDD) techniques using GitHub Copilot for comprehensive API testing.

## Prerequisites

* **Visual Studio 2022** (any edition)
* **GitHub Copilot** extension enabled
* **.NET 9.0 SDK**

## Getting Started

### Step 1: Open the Solution

1. Launch **Visual Studio 2022**
2. Navigate to `File` → `Open` → `Project or Solution`
3. Browse to `completed\cphol25red.sln` and open it
4. Locate the `ProductInventoryAPI.Tests` project under the `lab4` folder

### Step 2: Run the Tests

#### Option A: Test Explorer

1. Open `Test` → `Test Explorer` from the menu
2. Click **"Run All Tests"** or press `Ctrl+R, A`
3. View test results in the Test Explorer window

#### Option B: Solution Explorer

1. Right-click on `ProductInventoryAPI.Tests` project
2. Select **"Run Tests"** from the context menu

#### Option C: Individual Test Classes

1. Open any test file (e.g., `ProductsControllerTests.cs`)
2. Click the **green play button** next to individual tests or test classes
3. Use `Ctrl+R, T` to run tests in the current file

## Test Coverage

### Unit Tests

* **Controllers**: `ProductsControllerTests.cs` - Tests API endpoints in isolation
* **Models**: `ProductTests.cs` - Tests domain model validation and behavior

### Integration Tests

* **API Integration**: `ProductApiIntegrationTests.cs` - End-to-end API testing
* **Full Request/Response**: Tests complete HTTP request/response cycle

### Test Utilities

* **Builders**: `ProductBuilder.cs` - Test data builder pattern for creating test objects

## Features Demonstrated

### Testing Frameworks & Libraries

* **xUnit**: Primary testing framework
* **FluentAssertions**: Readable and expressive assertions
* **ASP.NET Core Testing**: `TestServer` and `WebApplicationFactory`
* **Microsoft.AspNetCore.Mvc.Testing**: Integration testing utilities

### Testing Patterns

* **Arrange-Act-Assert (AAA)**: Standard test structure
* **Builder Pattern**: `ProductBuilder` for flexible test data creation
* **Test Data Isolation**: Each test creates its own data
* **Integration Testing**: Full HTTP pipeline testing

## Key Test Scenarios

### Controller Unit Tests (`ProductsControllerTests.cs`)

```csharp
[Fact]
public async Task GetProduct_WithValidId_ReturnsProduct()
[Fact]
public async Task GetProduct_WithInvalidId_ReturnsNotFound()
[Fact]
public async Task CreateProduct_WithValidData_ReturnsCreated()
[Fact]
public async Task CreateProduct_WithInvalidData_ReturnsBadRequest()
[Fact]
public async Task UpdateProduct_WithValidData_ReturnsNoContent()
[Fact]
public async Task DeleteProduct_WithValidId_ReturnsNoContent()
```

### Model Unit Tests (`ProductTests.cs`)

```csharp
[Fact]
public void Product_WithValidData_CreatesSuccessfully()
[Fact]
public void Product_WithInvalidPrice_ThrowsValidationException()
[Fact]
public void Product_WithEmptyName_ThrowsValidationException()
```

### Integration Tests (`ProductApiIntegrationTests.cs`)

```csharp
[Fact]
public async Task GetProducts_ReturnsSuccessAndCorrectContentType()
[Fact]
public async Task PostProduct_WithValidProduct_ReturnsCreatedResponse()
[Fact]
public async Task PutProduct_WithValidData_ReturnsNoContent()
[Fact]
public async Task DeleteProduct_RemovesProductFromStore()
```

## Test Data Builder Pattern

The `ProductBuilder` class demonstrates the builder pattern for test data:

```csharp
var product = new ProductBuilder()
    .WithName("Test Product")
    .WithPrice(99.99m)
    .WithCategory("Electronics")
    .WithStockQuantity(50)
    .Build();
```

## Running Specific Tests

### Visual Studio Test Explorer (Recommended)

1. **Open Test Explorer**: Test → Test Explorer
2. **Run all tests**: Click "Run All Tests" button or press `Ctrl+R, A`
3. **Run specific test class**: Right-click class → "Run"
4. **Run specific test method**: Right-click method → "Run"
5. **Debug tests**: Right-click → "Debug"

### Visual Studio Shortcuts

* `Ctrl+R, A` - Run all tests in solution
* `Ctrl+R, T` - Run tests in current context
* `Ctrl+R, Ctrl+T` - Debug tests in current context

### Command Line (Alternative)

```shell
# Run all tests
dotnet test

# Run specific test class
dotnet test --filter "ProductsControllerTests"

# Run specific test method
dotnet test --filter "GetProduct_WithValidId_ReturnsProduct"

# Run with detailed output
dotnet test --logger "console;verbosity=detailed"
```

## Test Results Analysis

### Understanding Test Output

* **Green checkmarks**: Passing tests
* **Red X marks**: Failing tests
* **Yellow warning**: Skipped tests
* **Duration**: Execution time for each test

### Common Assertions (FluentAssertions)

```csharp
result.Should().NotBeNull();
result.Should().BeOfType<OkObjectResult>();
response.StatusCode.Should().Be(HttpStatusCode.Created);
products.Should().HaveCount(3);
product.Name.Should().Be("Expected Name");
```

## Key Learning Points

### Copilot Test Generation

1. **Test Method Generation**: Copilot suggests complete test methods from method names
2. **Assertion Patterns**: Recognizes testing patterns and suggests appropriate assertions
3. **Test Data Creation**: Generates realistic test data and scenarios
4. **Mock Object Setup**: Suggests appropriate mocking and stubbing
5. **Integration Test Patterns**: Understands web API testing patterns

### TDD Best Practices

* **Red-Green-Refactor**: Write failing test, make it pass, refactor
* **Test Isolation**: Each test should be independent
* **Meaningful Names**: Test names describe the scenario and expected outcome
* **Single Responsibility**: Each test should verify one specific behavior
* **Fast Execution**: Unit tests should run quickly

## Project Structure

``` shell
ProductInventoryAPI.Tests/
├── Controllers/
│   └── ProductsControllerTests.cs   # Controller unit tests
├── Models/
│   └── ProductTests.cs              # Model validation tests
├── Integration/
│   └── ProductApiIntegrationTests.cs # End-to-end API tests
├── Builders/
│   └── ProductBuilder.cs            # Test data builder
└── Services/                        # Future service tests
```

## Troubleshooting

### Test Failures

* Check test names and assertions carefully
* Verify test data setup is correct
* Ensure proper async/await usage in async tests
* Check that services and dependencies are properly configured

### Integration Test Issues

* Verify the API project builds successfully
* Check that test server configuration matches the main application
* Ensure test database/storage is properly isolated

### Performance Issues

* Integration tests may be slower than unit tests
* Consider running unit tests more frequently during development
* Use `dotnet test --parallel` for faster execution

## Next Steps

* Add more comprehensive edge case testing
* Implement performance testing
* Add database integration tests (see Lab 5)
* Create automated test reporting
* Set up continuous integration testing

---

**Note**: This test suite provides comprehensive coverage for the Product Inventory API. The tests serve as living documentation of the expected behavior and help ensure code quality during development.
