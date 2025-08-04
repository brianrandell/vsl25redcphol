using ProductInventoryAPI.Models;

namespace ProductInventoryAPI.Data;

public static class DbInitializer
{
    public static void Initialize(AppDbContext context)
    {
        // Ensure database is created
        context.Database.EnsureCreated();

        // Check if data already exists
        if (context.Categories.Any())
        {
            return; // Database has been seeded
        }

        SeedCategories(context);
        SeedProducts(context);

        context.SaveChanges();
    }

    private static void SeedCategories(AppDbContext context)
    {
        var categories = new Category[]
        {
            new Category
            {
                Name = "Electronics",
                Description = "Electronic devices and gadgets",
                IsActive = true,
                CreatedDate = DateTime.UtcNow
            },
            new Category
            {
                Name = "Books",
                Description = "Physical and digital books",
                IsActive = true,
                CreatedDate = DateTime.UtcNow
            },
            new Category
            {
                Name = "Clothing",
                Description = "Apparel and fashion items",
                IsActive = true,
                CreatedDate = DateTime.UtcNow
            },
            new Category
            {
                Name = "Home & Garden",
                Description = "Home improvement and gardening supplies",
                IsActive = true,
                CreatedDate = DateTime.UtcNow
            },
            new Category
            {
                Name = "Sports",
                Description = "Sports equipment and fitness gear",
                IsActive = true,
                CreatedDate = DateTime.UtcNow
            }
        };

        context.Categories.AddRange(categories);
    }

    private static void SeedProducts(AppDbContext context)
    {
        var products = new Product[]
        {
            // Electronics
            new Product
            {
                Name = "Laptop Pro",
                Description = "High-performance laptop for professionals",
                Price = 1299.99m,
                StockQuantity = 15,
                Category = "Electronics",
                CategoryId = 1,
                IsActive = true
            },
            new Product
            {
                Name = "Wireless Mouse",
                Description = "Ergonomic wireless mouse with precision tracking",
                Price = 29.99m,
                StockQuantity = 50,
                Category = "Electronics",
                CategoryId = 1,
                IsActive = true
            },
            new Product
            {
                Name = "USB-C Hub",
                Description = "Multi-port USB-C hub with HDMI and ethernet",
                Price = 79.99m,
                StockQuantity = 25,
                Category = "Electronics",
                CategoryId = 1,
                IsActive = true
            },
            new Product
            {
                Name = "Bluetooth Headphones",
                Description = "Noise-cancelling wireless headphones",
                Price = 199.99m,
                StockQuantity = 8,
                Category = "Electronics",
                CategoryId = 1,
                IsActive = true
            },

            // Books
            new Product
            {
                Name = "Clean Code",
                Description = "A handbook of agile software craftsmanship",
                Price = 42.99m,
                StockQuantity = 30,
                Category = "Books",
                CategoryId = 2,
                IsActive = true
            },
            new Product
            {
                Name = "Design Patterns",
                Description = "Elements of reusable object-oriented software",
                Price = 54.99m,
                StockQuantity = 20,
                Category = "Books",
                CategoryId = 2,
                IsActive = true
            },
            new Product
            {
                Name = "The Pragmatic Programmer",
                Description = "Your journey to mastery",
                Price = 39.99m,
                StockQuantity = 25,
                Category = "Books",
                CategoryId = 2,
                IsActive = true
            },

            // Clothing
            new Product
            {
                Name = "Cotton T-Shirt",
                Description = "100% organic cotton t-shirt",
                Price = 24.99m,
                StockQuantity = 100,
                Category = "Clothing",
                CategoryId = 3,
                IsActive = true
            },
            new Product
            {
                Name = "Denim Jeans",
                Description = "Classic fit denim jeans",
                Price = 79.99m,
                StockQuantity = 45,
                Category = "Clothing",
                CategoryId = 3,
                IsActive = true
            },
            new Product
            {
                Name = "Winter Jacket",
                Description = "Waterproof winter jacket with insulation",
                Price = 149.99m,
                StockQuantity = 12,
                Category = "Clothing",
                CategoryId = 3,
                IsActive = true
            },

            // Home & Garden
            new Product
            {
                Name = "Garden Hose",
                Description = "50ft expandable garden hose",
                Price = 34.99m,
                StockQuantity = 18,
                Category = "Home & Garden",
                CategoryId = 4,
                IsActive = true
            },
            new Product
            {
                Name = "LED Light Bulbs",
                Description = "Energy-efficient LED bulbs 4-pack",
                Price = 19.99m,
                StockQuantity = 60,
                Category = "Home & Garden",
                CategoryId = 4,
                IsActive = true
            },
            new Product
            {
                Name = "Plant Fertilizer",
                Description = "Organic plant fertilizer for indoor plants",
                Price = 12.99m,
                StockQuantity = 35,
                Category = "Home & Garden",
                CategoryId = 4,
                IsActive = true
            },

            // Sports
            new Product
            {
                Name = "Yoga Mat",
                Description = "Non-slip yoga mat with carrying strap",
                Price = 29.99m,
                StockQuantity = 40,
                Category = "Sports",
                CategoryId = 5,
                IsActive = true
            },
            new Product
            {
                Name = "Dumbbells Set",
                Description = "Adjustable dumbbells set 5-50 lbs",
                Price = 299.99m,
                StockQuantity = 6,
                Category = "Sports",
                CategoryId = 5,
                IsActive = true
            },
            new Product
            {
                Name = "Running Shoes",
                Description = "Lightweight running shoes with arch support",
                Price = 119.99m,
                StockQuantity = 22,
                Category = "Sports",
                CategoryId = 5,
                IsActive = true
            },

            // Additional products with low stock for testing
            new Product
            {
                Name = "Smartphone Case",
                Description = "Protective case for smartphones",
                Price = 19.99m,
                StockQuantity = 3,
                Category = "Electronics",
                CategoryId = 1,
                IsActive = true
            },
            new Product
            {
                Name = "Notebook Set",
                Description = "Set of 3 lined notebooks",
                Price = 14.99m,
                StockQuantity = 5,
                Category = "Books",
                CategoryId = 2,
                IsActive = true
            },
            new Product
            {
                Name = "Baseball Cap",
                Description = "Adjustable baseball cap",
                Price = 24.99m,
                StockQuantity = 7,
                Category = "Clothing",
                CategoryId = 3,
                IsActive = true
            },
            new Product
            {
                Name = "Tennis Racket",
                Description = "Professional tennis racket",
                Price = 89.99m,
                StockQuantity = 4,
                Category = "Sports",
                CategoryId = 5,
                IsActive = true
            }
        };

        context.Products.AddRange(products);
    }
}