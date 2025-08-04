# Lab 6: Database Integration with Entity Framework Core - Product Inventory API

This folder contains the completed implementation of Lab 6 from the GitHub Copilot training at VSLIVE! 2025 Redmond.
This ASP.NET Core Web API demonstrates database integration using Entity Framework Core with SQL Server.

## Prerequisites

* **Visual Studio 2022** (any edition)
* **GitHub Copilot** extension enabled
* **.NET 9.0 SDK**
* **SQL Server LocalDB** (included with Visual Studio)

## Getting Started

### Step 1: Open the Solution

1. Launch **Visual Studio 2022**
2. Navigate to `File` → `Open` → `Project or Solution`
3. Browse to `completed\cphol25red.sln` and open it
4. Locate the `ProductInventoryAPIwithEFC` project under the `lab6` folder

### Step 2: Set as Startup Project

1. In **Solution Explorer**, right-click on `ProductInventoryAPIwithEFC`
2. Select **"Set as Startup Project"

### Step 3: Build and Run

1. Press `Ctrl+Shift+B` to build the solution
2. Press `F5` to run the application
3. The API will launch and open Swagger UI in your browser
4. Database will be automatically created on first run

## Features Demonstrated

### Database Technologies

* **Entity Framework Core 9.0**: Modern ORM for .NET
* **SQL Server LocalDB**: Local development database
* **Code-First Migrations**: Database schema management
* **Database Seeding**: Automatic sample data insertion
* **Repository Pattern**: Clean data access layer

### API Endpoints

* **GET /api/products** - Retrieve all products with pagination
* **GET /api/products/{id}** - Get a specific product by ID
* **GET /api/products/category/{category}** - Filter products by category
* **GET /api/products/lowstock** - Find products with low inventory
* **POST /api/products** - Create a new product
* **PUT /api/products/{id}** - Update an existing product
* **DELETE /api/products/{id}** - Soft delete a product

## Database Schema

### Products Table

* **Id** (int, Primary Key) - Auto-incrementing product identifier
* **Name** (nvarchar(100)) - Product name (required)
* **Description** (nvarchar(500)) - Product description
* **Price** (decimal(18,2)) - Product price (required)
* **StockQuantity** (int) - Available inventory (required)
* **CategoryId** (int, Foreign Key) - Link to category
* **CreatedAt** (datetime2) - Creation timestamp
* **UpdatedAt** (datetime2) - Last modification timestamp
* **IsDeleted** (bit) - Soft delete flag

### Categories Table

* **Id** (int, Primary Key) - Category identifier
* **Name** (nvarchar(50)) - Category name (required)
* **Description** (nvarchar(200)) - Category description

## Project Structure

``` shell
ProductInventoryAPIwithEFC/
├── Controllers/
│   └── ProductsController.cs       # API endpoints with EF Core
├── Data/
│   ├── AppDbContext.cs            # Entity Framework context
│   ├── DbInitializer.cs           # Database seeding logic
│   └── Configurations/
│       ├── ProductConfiguration.cs # Product entity configuration
│       └── CategoryConfiguration.cs # Category entity configuration
├── Models/
│   ├── Product.cs                 # Product entity
│   └── Category.cs                # Category entity
├── Repositories/
│   ├── IProductRepository.cs      # Repository interface
│   └── ProductRepository.cs       # Repository implementation
├── DTOs/
│   ├── ProductCreateDto.cs        # Create request model
│   ├── ProductUpdateDto.cs        # Update request model
│   └── ProductResponseDto.cs      # Response model
├── Extensions/
│   └── ProductMappingExtensions.cs # Entity to DTO mapping
├── Migrations/
│   ├── InitialCreate.cs           # Initial database schema
│   └── AddStoredProcedures.cs     # Stored procedures migration
└── Program.cs                     # Application configuration
```

## Entity Framework Features

### Entity Configuration

```csharp
public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.HasKey(p => p.Id);
        builder.Property(p => p.Name)
               .IsRequired()
               .HasMaxLength(100);
        builder.Property(p => p.Price)
               .HasColumnType("decimal(18,2)");
        // Additional configurations...
    }
}
```

### Repository Pattern

```csharp
public class ProductRepository : IProductRepository
{
    private readonly AppDbContext _context;
    
    public async Task<IEnumerable<Product>> GetAllAsync()
    {
        return await _context.Products
            .Include(p => p.Category)
            .Where(p => !p.IsDeleted)
            .ToListAsync();
    }
}
```

### Database Context

```csharp
public class AppDbContext : DbContext
{
    public DbSet<Product> Products { get; set; }
    public DbSet<Category> Categories { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new ProductConfiguration());
        modelBuilder.ApplyConfiguration(new CategoryConfiguration());
    }
}
```

## Sample Data

The database is automatically seeded with sample data:

### Categories

* Electronics
* Clothing
* Books
* Home & Garden
* Sports

### Products

* Gaming Laptop (Electronics) - $1,299.99
* Wireless Mouse (Electronics) - $49.99
* Cotton T-Shirt (Clothing) - $19.99
* Programming Book (Books) - $39.99
* Garden Tools Set (Home & Garden) - $89.99

## API Testing Examples

### Create a Product

```json
POST /api/products
{
  "name": "Mechanical Keyboard",
  "description": "RGB backlit mechanical keyboard",
  "price": 129.99,
  "stockQuantity": 50,
  "categoryId": 1
}
```

### Update a Product

```json
PUT /api/products/1
{
  "name": "Updated Gaming Laptop",
  "description": "High-performance gaming laptop with RTX 4070",
  "price": 1399.99,
  "stockQuantity": 25,
  "categoryId": 1
}
```

### Filter by Category

```http
GET /api/products/category/Electronics
```

### Find Low Stock Items

```http
GET /api/products/lowstock
```

## Database Migrations

### Creating Migrations

```shell
# Add a new migration
dotnet ef migrations add MigrationName

# Update database
dotnet ef database update
```

### Available Migrations

1. **InitialCreate** - Creates Products and Categories tables
2. **AddStoredProcedures** - Adds custom stored procedures

## Configuration

### Connection String (appsettings.json)

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=ProductInventoryDB;Trusted_Connection=true;MultipleActiveResultSets=true"
  }
}
```

### Entity Framework Registration (Program.cs)

```csharp
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IProductRepository, ProductRepository>();
```

## Key Learning Points

### Copilot Database Development

1. **Entity Generation**: Copilot suggests entity properties and relationships
2. **Configuration Classes**: Generates Fluent API configurations
3. **Repository Patterns**: Suggests repository interfaces and implementations
4. **Migration Scripts**: Helps create and modify database migrations
5. **Seeding Logic**: Generates realistic sample data

### EF Core Best Practices

* **Async Operations**: All database operations use async/await
* **Eager Loading**: Include related entities with `.Include()`
* **Soft Deletes**: Mark records as deleted instead of physical deletion
* **Configuration Classes**: Separate entity configuration from DbContext
* **Repository Pattern**: Abstraction layer over EF Core

## Troubleshooting

### Database Issues

1. **Connection Problems**:
   * Check SQL Server LocalDB is installed
   * Verify connection string in appsettings.json
   * Ensure database is created (automatic on first run)

2. **Migration Issues**:
   * Run `dotnet ef database update` manually
   * Check Package Manager Console for error details
   * Verify EF Core tools are installed

3. **Performance Issues**:
   * Add `.AsNoTracking()` for read-only queries
   * Use projection instead of loading full entities
   * Implement pagination for large datasets

### Common Errors

* **LocalDB not found**: Install SQL Server Express LocalDB
* **Migration conflicts**: Delete database and recreate from migrations
* **Seeding errors**: Check for duplicate data in seed methods

## Testing the Database

### Using Swagger UI

1. Launch the application (F5)
2. Navigate to Swagger UI
3. Test each endpoint with the interactive interface
4. Check database through SQL Server Object Explorer in Visual Studio

### Using SQL Server Object Explorer

1. Open **View** → **SQL Server Object Explorer**
2. Expand **(localdb)\MSSQLLocalDB**
3. Find **ProductInventoryDB** database
4. Browse tables and data

## Next Steps

* Add authentication and user roles
* Implement caching strategies
* Add database performance monitoring
* Create database backup strategies
* Add bulk operations support
* Implement audit trails

---

**Note**: This project demonstrates production-ready database integration patterns.
The automatic database creation and seeding make it easy to get started,
while the repository pattern provides a clean separation between business logic and data access.
