# Lab 3: Copilot Chat & Contextual Assistance - Product Inventory API

This folder contains the completed implementation of Lab 3 from the GitHub Copilot training at VSLIVE! 2025 Redmond. This ASP.NET Core Web API demonstrates Copilot Chat features, Agent Mode capabilities, and comprehensive RESTful API development with advanced cross-cutting concerns.

## Prerequisites

* **Visual Studio 2022** (any edition)
* **GitHub Copilot** extension enabled
* **.NET 9.0 SDK**

## Getting Started

### Step 1: Open the Project

1. Launch **Visual Studio 2022**
2. Navigate to `File` → `Open` → `Project or Solution`
3. Browse to `completed\lab3\ProductInventoryAPI\ProductInventoryAPI.csproj` and open it

### Step 2: Build and Run

1. Press `Ctrl+Shift+B` to build the solution
2. Press `F5` to run the application
3. The API will launch and open Swagger UI in your browser
4. The URL will be something like: `https://localhost:7XXX/swagger/index.html`

## Features Demonstrated

### API Endpoints

* **GET /api/products** - Retrieve all products with pagination
* **GET /api/products/{id}** - Get a specific product by ID
* **POST /api/products** - Create a new product
* **PUT /api/products/{id}** - Update an existing product
* **DELETE /api/products/{id}** - Soft delete a product
* **GET /api/products/search** - Search products with filtering
* **GET /api/products/categories** - Get all available product categories

### Core Features

* **DTOs (Data Transfer Objects)**: Clean separation between API and domain models
* **Custom Exceptions**: BusinessRuleException, NotFoundException, ValidationException
* **Error Handling Middleware**: Centralized exception handling with proper HTTP status codes
* **Extension Methods**: ProductMappingExtensions for clean object mapping
* **Model Validation**: Data annotations and custom validation logic
* **API Versioning**: Version 1.0 support with proper headers
* **XML Documentation**: Comprehensive API documentation for Swagger

### Agent Mode Features

* **Comprehensive Logging**: 
  - Method entry/exit logs with parameter values
  - Execution time tracking with Stopwatch
  - Detailed error logging in all catch blocks
  - Structured logging with ILogger<T>

* **Input Sanitization**:
  - XSS attack prevention for Product name and description
  - HTML encoding and dangerous character filtering
  - Custom SanitizationService with dependency injection

* **Complete Audit Trail System**:
  - IAuditableEntity interface implementation
  - CreatedAt, UpdatedAt, CreatedBy, UpdatedBy properties
  - AuditMiddleware for capturing user context
  - Automatic audit property updates in all CRUD operations

## API Testing

### Using Swagger UI

1. Launch the application (F5)
2. Navigate to the Swagger interface
3. Expand any endpoint to see parameters and schemas
4. Click **"Try it out"** to test endpoints interactively

### Sample Product JSON

```json
{
  "name": "Gaming Laptop",
  "description": "High-performance gaming laptop with RTX graphics",
  "price": 1299.99,
  "category": "Electronics",
  "stockQuantity": 25
}
```

### Testing Agent Mode Features

**Audit Trail**: Create or update products and notice the automatic timestamps and user tracking in responses:

```json
{
  "id": 1,
  "name": "Gaming Laptop",
  "description": "High-performance gaming laptop with RTX graphics",
  "price": 1299.99,
  "category": "Electronics",
  "stockQuantity": 25,
  "isActive": true,
  "createdAt": "2025-01-15T10:30:00Z",
  "updatedAt": "2025-01-15T14:45:00Z",
  "createdBy": "Demo User",
  "updatedBy": "Demo User"
}
```

**XSS Protection**: Try sending malicious input like `<script>alert('xss')</script>` in product names - it will be automatically sanitized.

**Logging**: Check the application logs for detailed method execution information, timing, and parameter values.

### Using the HTTP File

* Open `ProductInventoryAPI.http` in Visual Studio
* Contains pre-configured HTTP requests for testing
* Click the **"Send Request"** links to execute

## Project Structure

``` shell
ProductInventoryAPI/
├── Controllers/
│   └── ProductsController.cs    # API endpoints with comprehensive logging
├── DTOs/
│   ├── ProductCreateDto.cs      # Create request model
│   ├── ProductUpdateDto.cs      # Update request model
│   └── ProductResponseDto.cs    # Response model with audit fields
├── Models/
│   ├── Product.cs               # Domain model implementing IAuditableEntity
│   └── IAuditableEntity.cs      # Audit trail interface
├── Services/
│   ├── ISanitizationService.cs  # XSS protection interface
│   ├── SanitizationService.cs   # XSS protection implementation
│   ├── ICurrentUserService.cs   # User context interface
│   └── CurrentUserService.cs    # User context implementation
├── Exceptions/
│   ├── ApiException.cs          # Base exception
│   ├── BusinessRuleException.cs # Business logic errors
│   ├── NotFoundException.cs     # Resource not found
│   └── ValidationException.cs   # Input validation errors
├── Extensions/
│   └── ProductMappingExtensions.cs # Object mapping with sanitization
├── Middleware/
│   ├── ErrorHandlingMiddleware.cs  # Global error handling
│   └── AuditMiddleware.cs       # User context and audit logging
└── Program.cs                   # Application configuration with DI setup
```

## Key Learning Points

### Ask Mode vs Agent Mode Demonstrations

1. **Ask Mode Features**: Interactive Q&A for understanding code and exploring options
2. **Agent Mode Features**: Autonomous implementation across multiple files
3. **Context-Aware Suggestions**: Chat understands the current codebase
4. **API Pattern Recognition**: Generates standard REST API patterns
5. **Cross-cutting Concerns**: Automatically implements logging, security, and audit features

### Advanced Implementation Patterns

* **Comprehensive Logging**: Method-level logging with performance tracking
* **Security by Design**: Input sanitization and XSS protection
* **Audit Trail Architecture**: Complete user and timestamp tracking
* **Dependency Injection**: Service-oriented architecture with proper DI
* **Middleware Pipeline**: Custom middleware for cross-cutting concerns

### Best Practices Demonstrated

* **Separation of Concerns**: DTOs separate from domain models
* **Consistent Error Handling**: Centralized middleware approach
* **API Documentation**: Swagger/OpenAPI integration with XML comments
* **HTTP Status Codes**: Proper REST API response codes
* **Input Validation**: Data annotations and custom validation
* **Thread Safety**: Proper locking mechanisms for in-memory storage

## Testing the API

### Create a Product

```http
POST https://localhost:7XXX/api/products
Content-Type: application/json

{
  "name": "Wireless Mouse",
  "description": "Ergonomic wireless mouse with RGB lighting",
  "price": 49.99,
  "category": "Electronics",
  "stockQuantity": 100
}
```

### Update a Product

```http
PUT https://localhost:7XXX/api/products/1
Content-Type: application/json

{
  "name": "Updated Product Name",
  "description": "Updated description",
  "price": 59.99,
  "category": "Electronics",
  "stockQuantity": 75,
  "isActive": true
}
```

### Search Products

```http
GET https://localhost:7XXX/api/products/search?name=laptop&category=Electronics&minPrice=500&maxPrice=2000&pageNumber=1&pageSize=10
```

### Test Audit Headers

Add custom headers to see audit trail in action:

```http
POST https://localhost:7XXX/api/products
Content-Type: application/json
X-User-Id: user123
X-User-Name: John Doe

{
  "name": "Test Product",
  "description": "Product with custom user context",
  "price": 29.99,
  "category": "Test",
  "stockQuantity": 10
}
```

## Error Handling Examples

The API returns structured error responses:

```json
{
  "type": "ValidationError",
  "title": "Validation failed",
  "status": 400,
  "errors": {
    "Name": ["The Name field is required."],
    "Price": ["Price must be greater than 0."]
  }
}
```

## Troubleshooting

### Build Issues

* Ensure all NuGet packages are restored
* Check that .NET 9.0 SDK is installed
* Verify project references are correct

### Runtime Issues

* Check that no other applications are using the same ports
* Verify the startup project is set correctly
* Ensure HTTPS certificates are trusted

### API Testing Issues

* Use Swagger UI for interactive testing
* Check request headers and content types
* Verify JSON formatting in requests

## Advanced Features Deep Dive

### Logging Implementation

The API includes comprehensive logging that tracks:
- Method entry/exit with parameter values
- Execution time for performance monitoring
- Detailed error information with stack traces
- User context and audit information

### Security Features

- **XSS Protection**: All string inputs are sanitized using HTML encoding
- **Input Validation**: Data annotations with custom validation rules
- **Safe Defaults**: Proper null handling and defensive programming

### Audit Trail System

- **IAuditableEntity**: Interface defining audit requirements
- **Automatic Timestamps**: CreatedAt/UpdatedAt set automatically
- **User Tracking**: CreatedBy/UpdatedBy from request headers or defaults
- **Middleware Integration**: AuditMiddleware captures user context

## Next Steps

* Add database integration with Entity Framework (see Lab 5)
* Implement JWT authentication and authorization
* Create comprehensive unit tests (see Lab 4)
* Add caching and performance optimizations
* Implement async/await patterns throughout
* Add health checks and monitoring endpoints

---

**Note**: This API uses in-memory storage. Data will be lost when the application stops. See Lab 5 for database integration.
