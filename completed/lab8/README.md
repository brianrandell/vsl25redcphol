# Lab 8: Advanced Copilot Techniques - Completed Solution

This folder contains the completed implementation of Lab 8 from the GitHub Copilot training at VSLIVE! 2025 Redmond.

This project demonstrates advanced Copilot features including multi-file context awareness, security analysis,
performance optimization, design patterns, and MCP integration.

## Prerequisites

* **Visual Studio 2022** (any edition) with the following workloads:
  * .NET desktop development
  * ASP.NET and web development
* **GitHub Copilot** extension enabled
* **.NET 9.0 SDK** (should be included with VS 2022)

## Getting Started

### Step 1: Open the Solution

1. Launch **Visual Studio 2022**
2. Navigate to `File` → `Open` → `Project or Solution`
3. Browse to `completed\cphol25red.sln` and open it
4. Locate the `AdvancedCopilotAPI` project under the `lab8` folder

### Step 2: Set as Startup Project

1. In **Solution Explorer**, right-click on `AdvancedCopilotAPI`
2. Select **"Set as Startup Project"

### Step 3: Build and Run

1. Press `Ctrl+Shift+B` to build the solution
2. Press `F5` to run the application
3. The API will launch. Navigate to `https://localhost:7xxx/swagger` to access the Swagger UI (replace `7xxx` with the actual port number)

## Features Demonstrated

### Advanced Copilot Techniques

* **Multi-File Context**: Repository pattern with interfaces and implementations
* **Security Features**: Input sanitization, authentication patterns
* **Performance Optimization**: Caching, async patterns, resilient HTTP clients
* **Design Patterns**: Factory, Decorator, Strategy, Unit of Work, Saga
* **Error Handling**: Global exception handling, custom analyzers
* **MCP Integration**: Complete .NET MCP server implementation with project and database context

## API Endpoints

* **GET /api/products** - Retrieve all products (with caching)
* **GET /api/products/{id}** - Get a specific product
* **GET /api/products/category/{category}** - Filter by category
* **GET /api/products/lowstock** - Find products with low inventory
* **POST /api/products** - Create a new product (with validation)
* **PUT /api/products/{id}** - Update an existing product
* **DELETE /api/products/{id}** - Soft delete a product
* **GET /api/products/count** - Get total product count

## Project Structure

``` shell
AdvancedCopilotAPI/
├── Controllers/
│   └── ProductsController.cs        # API endpoints with advanced features
├── Models/
│   └── Product.cs                   # Enhanced product model
├── Interfaces/
│   ├── IProductRepository.cs        # Repository interface
│   └── IUnitOfWork.cs              # Unit of Work pattern
├── Repositories/
│   ├── ProductRepository.cs         # Repository implementation
│   └── UnitOfWork.cs               # Transaction coordination
├── Services/
│   ├── CacheService.cs             # Memory caching implementation
│   └── ResilientHttpClient.cs      # HTTP client with retry policies
├── Decorators/
│   └── LoggingRepositoryDecorator.cs # Logging decorator pattern
├── Factories/
│   ├── INotificationFactory.cs      # Factory interface
│   └── NotificationFactory.cs       # Factory implementation
├── Strategies/
│   └── PricingStrategy.cs          # Strategy pattern for pricing
├── Patterns/
│   └── SagaPattern.cs              # Saga pattern for distributed transactions
├── Security/
│   └── InputSanitizer.cs           # XSS and SQL injection prevention
├── Handlers/
│   └── GlobalExceptionHandler.cs    # Centralized error handling
├── Analyzers/
│   └── NamingConventionAnalyzer.cs  # Custom Roslyn analyzer
├── Diagnostics/
│   └── PerformanceMonitor.cs        # Performance monitoring utilities
└── Program.cs                       # Application configuration
```

## Key Features in Detail

### Security Implementation

```csharp
// Input sanitization to prevent XSS
public class InputSanitizer
{
    public string SanitizeHtml(string input);
    public string SanitizeForSql(string input);
    public T ValidateAndSanitize<T>(T model);
}
```

### Caching Strategy

```csharp
// Generic memory cache with configurable expiration
public class CacheService
{
    public Task<T?> GetAsync<T>(string key);
    public Task SetAsync<T>(string key, T value, TimeSpan? expiration);
    public Task RemoveAsync(string key);
    public Task RemoveByPatternAsync(string pattern);
}
```

### Resilience Patterns

```csharp
// Polly policies for resilient HTTP calls
- Retry: 3 attempts with exponential backoff
- Circuit Breaker: Opens after 5 consecutive failures
- Timeout: 30-second timeout
- Bulkhead: Maximum 10 concurrent requests
```

### Design Patterns Implemented

#### Factory Pattern

* `INotificationFactory` - Creates notification services
* Supports Email, SMS, Push, and In-App notifications
* Returns appropriate service based on notification type

#### Decorator Pattern

* `LoggingRepositoryDecorator` - Adds logging to repositories
* Measures execution time
* Logs method entry, exit, and exceptions
* Preserves original functionality

#### Strategy Pattern

* `PricingStrategy` - Calculates prices based on context
* Regular, Bulk Discount, Member, and Seasonal pricing
* Context includes quantity, customer type, and season

#### Unit of Work Pattern

* Coordinates transactions across repositories
* Ensures data consistency
* Supports rollback on failure

### Performance Features

* **Async/Await Optimization**: ConfigureAwait(false) where appropriate
* **Parallel Processing**: Parallel.ForEach for bulk operations
* **Memory Caching**: Reduces database queries
* **Connection Pooling**: Efficient resource management
* **Query Optimization**: Indexed queries and pagination

### Error Handling

* **Global Exception Handler**: Catches all unhandled exceptions
* **Correlation IDs**: Tracks requests across services
* **Structured Logging**: Easy to query and analyze
* **Custom Exceptions**: Business-specific error types
* **Retry Headers**: Includes retry-after for rate limiting

## Testing

### Integration Tests

The `AdvancedCopilotAPI.Tests` project includes:

* Comprehensive API endpoint tests
* Performance benchmarks
* Security vulnerability tests
* Pattern implementation verification

### Running Tests

**In Visual Studio 2022**: Use Test Explorer (Test → Test Explorer) and click "Run All Tests"

**Alternative (Command Line)**:
```shell
dotnet test
```

## Configuration

### appsettings.json

```json
{
  "Caching": {
    "DefaultExpirationMinutes": 5,
    "MaxMemorySizeMB": 100
  },
  "Resilience": {
    "RetryCount": 3,
    "CircuitBreakerThreshold": 5,
    "TimeoutSeconds": 30
  }
}
```

## Key Learning Points

### Advanced Copilot Usage

1. **Multi-File Context**: Keep related files open for better suggestions
2. **Security Reviews**: Use Chat for vulnerability analysis
3. **Performance Analysis**: Let Copilot identify bottlenecks
4. **Pattern Generation**: Describe patterns in comments
5. **Refactoring**: Use Chat for cross-file refactoring

### Best Practices Demonstrated

* **SOLID Principles**: Single responsibility, dependency injection
* **Clean Architecture**: Separation of concerns
* **Security First**: Input validation and sanitization
* **Performance**: Caching and async optimization
* **Resilience**: Retry and circuit breaker patterns

## Troubleshooting

### Common Issues

* **Caching Issues**: Check memory limits in configuration
* **Circuit Breaker Open**: Wait for reset or adjust threshold
* **Performance**: Enable detailed logging to identify bottlenecks

### Debugging Tips

1. Check the Diagnostics page for performance metrics
2. Review logs for correlation IDs
3. Use breakpoints in decorators to trace execution
4. Monitor cache hit rates

## MCP Integration Capabilities

This solution demonstrates how to work with MCP (Model Context Protocol) servers in GitHub Copilot, focusing on using existing MCP servers rather than building custom ones.

### Available MCP Servers

* **Microsoft Learn MCP**: Access to official Microsoft documentation and best practices
* **HTTP-based servers**: More reliable than npm package-based alternatives

### Usage Examples

1. **Microsoft Learn integration**:
   ```
   @microsoft.docs.mcp How do I implement dependency injection in .NET 9?
   ```

2. **Get architecture guidance**:
   ```
   @microsoft.docs.mcp What are the best practices for ASP.NET Core Web APIs?
   ```

3. **Performance optimization**:
   ```
   @microsoft.docs.mcp What are the latest Entity Framework Core performance improvements?
   ```

### Configuration

Add the Microsoft Learn MCP server to your `%USERPROFILE%\.mcp.json` file:

```json
{
  "servers": {
    "microsoft.docs.mcp": {
      "type": "http",
      "url": "https://learn.microsoft.com/api/mcp"
    }
  }
}
```

**Benefits of HTTP-based MCP servers:**
- No Node.js installation required
- More reliable connectivity
- Access to up-to-date Microsoft documentation
- Easier configuration and troubleshooting

## Next Steps

* Implement distributed caching with Redis
* Add authentication and authorization
* Create comprehensive benchmarks
* Implement event sourcing
* Add health checks and monitoring
* Deploy to cloud with auto-scaling
* Integrate with additional MCP servers for enhanced context

---

**Note**: This project demonstrates advanced patterns and practices.
Some features are simplified for educational purposes.
In production, consider using established libraries and frameworks for these patterns.
