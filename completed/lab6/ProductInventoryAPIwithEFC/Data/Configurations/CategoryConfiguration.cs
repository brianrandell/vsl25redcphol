using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProductInventoryAPI.Models;

namespace ProductInventoryAPI.Data.Configurations;

public class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.HasKey(e => e.Id);
        
        builder.Property(e => e.Name)
            .IsRequired()
            .HasMaxLength(50);
        
        builder.Property(e => e.Description)
            .HasMaxLength(200);
        
        builder.HasIndex(e => e.Name)
            .IsUnique()
            .HasDatabaseName("IX_Categories_Name_Unique");
        
        // Soft delete filter
        builder.HasQueryFilter(c => c.IsActive);
        
        // One-to-many relationship with Products
        builder.HasMany(c => c.Products)
            .WithOne(p => p.CategoryNavigation)
            .HasForeignKey(p => p.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}