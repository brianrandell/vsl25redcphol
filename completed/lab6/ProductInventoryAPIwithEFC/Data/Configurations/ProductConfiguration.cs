using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProductInventoryAPI.Models;

namespace ProductInventoryAPI.Data.Configurations;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.HasKey(e => e.Id);
        
        builder.Property(e => e.Name)
            .IsRequired()
            .HasMaxLength(100);
        
        builder.Property(e => e.Description)
            .HasMaxLength(500);
        
        builder.Property(e => e.Price)
            .HasPrecision(18, 2);
        
        builder.Property(e => e.Category)
            .IsRequired()
            .HasMaxLength(50);
        
        builder.HasIndex(e => e.Name)
            .HasDatabaseName("IX_Products_Name");
        
        builder.HasIndex(e => new { e.CategoryId, e.IsActive })
            .HasDatabaseName("IX_Products_CategoryId_IsActive");

        // Soft delete filter - matches Category filter
        builder.HasQueryFilter(p => p.IsActive);

        // Configure relationship with Category
        builder.HasOne(e => e.CategoryNavigation)
            .WithMany(c => c.Products)
            .HasForeignKey(e => e.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}