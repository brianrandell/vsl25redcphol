# Lab 6: Database and SQL with Copilot - Hands-on Exercise

**Duration:** 35 minutes

## Prerequisites

* Visual Studio 2022 with GitHub Copilot enabled
* SQL Server Express or LocalDB installed
* The Web API project from previous labs (or create new)
* Basic understanding of Entity Framework Core

## Exercise Overview

Extend your Web API with Entity Framework Core, use Copilot to generate entity configurations, create complex LINQ queries, write SQL stored procedures, and generate seed data. As you work with Copilot, try using comments, chat and even Agent mode. The instructions will generally provide the core comments but use your best judgement to ask Copilot for help with specific tasks.

---

## Part 1: Setup Entity Framework Core (5 minutes)

### Step 1: Install Required Packages

1. **Right-click on your API project** → Manage NuGet Packages

2. **Install the following packages:**

   * `Microsoft.EntityFrameworkCore.SqlServer`
   * `Microsoft.EntityFrameworkCore.Tools`
   * `Microsoft.EntityFrameworkCore.Design`

3. **Open Package Manager Console** (Tools → NuGet Package Manager → Package Manager Console)

### Step 2: Create Initial Models

1. **In Models folder, ensure you have:**

   * `Product.cs` (from previous labs)

2. **Create new model** `Models/Category.cs`

3. **Type this comment and let Copilot generate:**

   ```csharp
   // Category model for product categorization
   // Properties:
   // - Id (int) - primary key
   // - Name (string) - required, max 50 chars
   // - Description (string) - optional, max 200 chars
   // - IsActive (bool) - soft delete flag
   // - CreatedDate (DateTime) - when created
   // - Products (ICollection<Product>) - navigation property
   ```

4. **Update Product.cs** with navigation properties:

   ```csharp
   // Add to Product class:
   // - CategoryId (int) - foreign key
   // - Category (Category) - navigation property
   ```

---

## Part 2: Create DbContext with Configurations (8 minutes)

### Step 1: Generate the DbContext

1. **Create folder** `Data` in project root

2. **Add new class** `Data/AppDbContext.cs`

3. **Type this comprehensive comment in Chat:**

   ```csharp
   // Entity Framework DbContext for the application
   // Should include:
   // - DbSet for Products
   // - DbSet for Categories
   // - Constructor accepting DbContextOptions
   // - OnModelCreating override with Fluent API configurations
   // - Configure relationships, indexes, and constraints
   using Microsoft.EntityFrameworkCore;
   using ProductInventoryAPI.Models;
   ```

4. **Let Copilot generate the DbContext class**

### Step 2: Add Entity Configurations

1. **In the OnModelCreating method, add comments:**

   ```csharp
   // Configure Product entity:
   // - Name is required with max length 100
   // - Price has precision 18,2
   // - Create index on Name for searching
   // - Create composite index on CategoryId and IsActive
   
   // Configure Category entity:
   // - Name is required and unique
   // - Soft delete filter (query filter for IsActive)
   // - One-to-many relationship with Products
   ```

2. **Let Copilot generate each configuration**

### Step 3: Create Separate Configuration Classes

1. **Create folder** `Data/Configurations`

2. **Add class** `Data/Configurations/ProductConfiguration.cs`

3. **Type:**

   ```csharp
   // Fluent API configuration for Product entity
   // Implement IEntityTypeConfiguration<Product>
   // Move Product configurations from DbContext here
   ```

4. **Repeat for** `CategoryConfiguration.cs`

---

## Part 3: Complex LINQ Queries with Copilot (10 minutes)

### Step 1: Create Repository Pattern

1. **Create folder** `Repositories`

2. **Add interface** `Repositories/IProductRepository.cs`

3. **Type this comment:**

   ```csharp
   // Repository interface for Product operations
   // Methods needed:
   // - GetAllAsync() - with optional include for category
   // - GetByIdAsync(int id) - include category
   // - GetByCategoryAsync(int categoryId)
   // - GetActiveProductsAsync() - only products with active categories
   // - SearchAsync(string searchTerm) - search name and description
   // - GetProductsWithLowStockAsync(int threshold)
   // - GetProductsByPriceRangeAsync(decimal min, decimal max)
   // - GetTopSellingProductsAsync(int count) - placeholder for now
   ```

### Step 2: Implement Complex Queries

1. **Add class** `Repositories/ProductRepository.cs`

2. **Start with comment:**

   ```csharp
   // Implementation of IProductRepository using Entity Framework
   // Constructor should accept AppDbContext
   // All methods should use async/await
   // Use Include for eager loading
   ```

3. **For each method, add specific query comments:**

   ```csharp
   // Get all products with their categories, ordered by name
   // Use eager loading for category
   
   // Search products where name or description contains search term
   // Case-insensitive search using EF.Functions.Like
   
   // Get products with low stock
   // Include category, order by stock quantity ascending
   
   // Complex query: Get products by multiple filters
   // Parameters: categoryId?, minPrice?, maxPrice?, isActive?
   // Apply filters only if parameters have values
   ```

4. **Let Copilot generate LINQ queries for each method**

### Step 3: Add Aggregate Queries

1. **Add methods to repository interface:**

   ```csharp
   // Get product statistics by category
   // Return: category name, product count, average price, total stock value
   
   // Get inventory value report
   // Group by category, calculate total value (price * stock)
   ```

2. **Implement with Copilot's help:**

   ```csharp
   // Use GroupBy and Select with anonymous types
   // Calculate aggregates using Sum, Average, Count
   ```

---

## Part 4: SQL Stored Procedures and Raw SQL (7 minutes)

### Step 1: Create Migration with Stored Procedures

1. **In Package Manager Console:**

   ```
   Add-Migration AddStoredProcedures
   ```

2. **Open the new migration file**

3. **In the Up method, add comment:**

   ```csharp
   // Create stored procedure to get products by category with pagination
   // Procedure name: sp_GetProductsByCategory
   // Parameters: @CategoryId, @PageNumber, @PageSize
   // Should return products with total count
   ```

4. **Let Copilot generate the SQL:**

   ```csharp
   migrationBuilder.Sql(@"
       -- Copilot will generate the stored procedure here
   ");
   ```

### Step 2: Create Complex Stored Procedure

1. **Add another stored procedure comment:**

   ```csharp
   // Create stored procedure for product inventory report
   // Procedure name: sp_GetInventoryReport
   // Should return:
   // - Total products
   // - Total inventory value
   // - Low stock items count (< 10)
   // - Categories with most products
   ```

### Step 3: Execute Raw SQL Queries

1. **In ProductRepository, add methods:**

   ```csharp
   // Execute stored procedure sp_GetProductsByCategory
   // Use FromSqlRaw to map results to Product entities
   
   // Execute raw SQL for complex reporting query
   // Get top 5 categories by total inventory value
   ```

2. **Add SQL parameter handling:**

   ```csharp
   // Method to safely execute SQL with parameters
   // Prevent SQL injection using SqlParameter
   ```

---

## Part 5: Database Seeding and Migrations (5 minutes)

### Step 1: Create Seed Data Class

1. **Add class** `Data/DbInitializer.cs`

2. **Type comment:**

   ```csharp
   // Database initializer for seeding test data
   // Methods:
   // - Initialize(AppDbContext) - main seeding method
   // - SeedCategories() - create 5 sample categories
   // - SeedProducts() - create 20 sample products across categories
   // Should check if data exists before seeding
   // Use realistic product names and prices
   ```

3. **Let Copilot generate comprehensive seed data**

### Step 2: Configure Seeding in Program.cs

1. **Open Program.cs**

2. **After builder.Build(), add comment:**

   ```csharp
   // Configure database initialization
   // - Create scope for DbContext
   // - Ensure database is created
   // - Run migrations
   // - Call DbInitializer.Initialize()
   ```

3. **Let Copilot generate the initialization code:**

   ```csharp
   using (var scope = app.Services.CreateScope())
   {
       var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
       context.Database.EnsureCreated();
       DbInitializer.Initialize(context);
   }
   ```

### Step 3: Add Connection String

1. **In appsettings.json, add:**

   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Server=(localdb)\\MSSQLLocalDB;Database=ProductInventoryDB;Trusted_Connection=True;MultipleActiveResultSets=true"
     }
   }
   ```

   > We're using localdb which is automatical installed with Visual Studio and the workloads necessary for this labs. You could of course use a different SQL Server instance if you perfer and know what you're doing..

2. **Register DbContext in Program.cs:**

   ```csharp
   builder.Services.AddDbContext<AppDbContext>(options =>
       options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
   ```

### Step 4: Run Migration

1. **In Package Manager Console:**

   ```
   Add-Migration InitialCreate
   Update-Database
   ```

---

## Part 6: Advanced EF Core Features (5 minutes)

### Step 1: Add Audit Fields

1. **Create interface** `Models/IAuditable.cs`

2. **Type:**

   ```csharp
   // Interface for entities with audit fields
   // Properties: CreatedDate, CreatedBy, ModifiedDate, ModifiedBy
   ```

3. **In AppDbContext, add:**

   ```csharp
   // Override SaveChangesAsync to automatically set audit fields
   // For entities implementing IAuditable:
   // - Set CreatedDate/CreatedBy on insert
   // - Set ModifiedDate/ModifiedBy on update
   public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
   {
       foreach (var entry in ChangeTracker.Entries<IAuditable>())
       {
           switch (entry.State)
           {
               case EntityState.Added:
                   entry.Entity.CreatedDate = DateTime.UtcNow;
                   entry.Entity.CreatedBy = "System"; // In real app, get from current user
                   break;
               case EntityState.Modified:
                   entry.Entity.ModifiedDate = DateTime.UtcNow;
                   entry.Entity.ModifiedBy = "System";
                   break;
           }
       }
       return await base.SaveChangesAsync(cancellationToken);
   }
   ```

### Step 2: Add Value Conversions

1. **In ProductConfiguration, add comment:**

   ```csharp
   // Configure value conversion for Price
   // Store as decimal(18,2) in database
   // Add converter for enum to string if needed
   ```

### Step 3: Query Optimization

1. **In repository methods, add:**

   ```csharp
   // Use AsNoTracking() for read-only queries
   // Use AsSplitQuery() for queries with multiple includes
   // Add query tags for debugging
   
   public async Task<IEnumerable<Product>> GetAllAsync()
   {
       return await _context.Products
           .AsNoTracking()
           .Include(p => p.Category)
           .TagWith("GetAllProducts")
           .ToListAsync();
   }
   ```

---

## Testing Your Database Integration (3 minutes)

1. **Update your API controllers** to use the repository:

   ```csharp
   [ApiController]
   [Route("api/[controller]")]
   public class ProductsController : ControllerBase
   {
       private readonly IProductRepository _repository;
       
       public ProductsController(IProductRepository repository)
       {
           _repository = repository;
       }
       
       [HttpGet]
       public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
       {
           return Ok(await _repository.GetAllAsync());
       }
   }
   ```

2. **Register repository in Program.cs:**

   ```csharp
   builder.Services.AddScoped<IProductRepository, ProductRepository>();
   ```

3. **Run the application** and check:

   * Database is created
   * Seed data is inserted
   * API endpoints work with database

4. **Use SQL Server Object Explorer** to verify:

   * View → SQL Server Object Explorer
   * Expand (localdb)\MSSQLLocalDB → Databases
   * Find ProductInventoryDB
   * Check tables and stored procedures

---

## Key Takeaways

✅ **You've learned to:**

* Configure Entity Framework Core with Copilot
* Generate complex LINQ queries
* Create and execute stored procedures
* Write safe raw SQL queries
* Implement database seeding
* Use advanced EF Core features

## Best Practices

1. **Entity Configuration**

   * Use separate configuration classes
   * Define relationships explicitly
   * Add appropriate indexes

2. **Query Optimization**

   * Use AsNoTracking for read-only
   * Avoid N+1 queries with Include
   * Use projection for large datasets

3. **SQL Safety**

   * Always parameterize queries
   * Validate input before queries
   * Use stored procedures for complex logic

## Challenge Extensions (Optional)

1. **Add database features:**

   ```csharp
   // Implement soft delete globally
   // Add full-text search
   // Create database views
   ```

2. **Performance optimization:**

   ```csharp
   // Add query result caching
   // Implement bulk operations
   // Add connection resiliency
   ```

3. **Advanced patterns:**

   ```csharp
   // Implement specification pattern
   // Add domain events
   // Create audit log table
   ```

## Troubleshooting

**Migration errors?**

* Check connection string in appsettings.json
* Ensure SQL Server/LocalDB is running
* Delete Migrations folder and restart if needed

**Query performance issues?**

* Enable logging to see generated SQL:

  ```csharp
  optionsBuilder.LogTo(Console.WriteLine, LogLevel.Information)
      .EnableSensitiveDataLogging();
  ```

* Use SQL Profiler to analyze
* Ask Copilot for optimization suggestions

**LocalDB not working?**

* Run `sqllocaldb start MSSQLLocalDB` in command prompt
* Check if instance exists: `sqllocaldb info`
* For ARM devices, may need manual start

**Stored procedures not created?**

* Check migration was applied: `Update-Database -Verbose`
* Verify SQL syntax in migration file
* Run SQL directly in SSMS to test
