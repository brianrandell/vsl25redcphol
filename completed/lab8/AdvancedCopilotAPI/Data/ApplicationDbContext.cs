using AdvancedCopilotAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace AdvancedCopilotAPI.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        
        public DbSet<Product> Products { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Description).HasMaxLength(500);
                entity.Property(e => e.Price).HasPrecision(18, 2);
                entity.Property(e => e.Category).HasMaxLength(50);
                entity.HasIndex(e => e.Category);
                entity.HasIndex(e => e.IsActive);
            });
            
            SeedData(modelBuilder);
        }
        
        private static void SeedData(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>().HasData(
                new Product { Id = 1, Name = "Laptop", Description = "High-performance laptop", Price = 999.99m, Stock = 50, Category = "Electronics" },
                new Product { Id = 2, Name = "Mouse", Description = "Wireless mouse", Price = 29.99m, Stock = 100, Category = "Electronics" },
                new Product { Id = 3, Name = "Keyboard", Description = "Mechanical keyboard", Price = 149.99m, Stock = 75, Category = "Electronics" },
                new Product { Id = 4, Name = "Book", Description = "Programming guide", Price = 39.99m, Stock = 5, Category = "Books" },
                new Product { Id = 5, Name = "Headphones", Description = "Noise-cancelling headphones", Price = 199.99m, Stock = 25, Category = "Electronics" }
            );
        }
    }
}