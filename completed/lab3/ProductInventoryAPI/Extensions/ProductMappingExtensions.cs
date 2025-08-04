using ProductInventoryAPI.DTOs;
using ProductInventoryAPI.Models;
using ProductInventoryAPI.Services;

namespace ProductInventoryAPI.Extensions;

public static class ProductMappingExtensions
{
    public static ProductResponseDto ToResponseDto(this Product product)
    {
        return new ProductResponseDto
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            Price = product.Price,
            StockQuantity = product.StockQuantity,
            Category = product.Category,
            IsActive = product.IsActive,
            CreatedAt = product.CreatedAt,
            UpdatedAt = product.UpdatedAt,
            CreatedBy = product.CreatedBy,
            UpdatedBy = product.UpdatedBy
        };
    }

    public static Product ToEntity(this ProductCreateDto createDto, ISanitizationService sanitizationService)
    {
        return new Product
        {
            Name = sanitizationService.SanitizeInput(createDto.Name),
            Description = sanitizationService.SanitizeInput(createDto.Description),
            Price = createDto.Price,
            StockQuantity = createDto.StockQuantity,
            Category = sanitizationService.SanitizeInput(createDto.Category),
            IsActive = true
        };
    }

    public static void UpdateEntity(this ProductUpdateDto updateDto, Product product, ISanitizationService sanitizationService)
    {
        product.Name = sanitizationService.SanitizeInput(updateDto.Name);
        product.Description = sanitizationService.SanitizeInput(updateDto.Description);
        product.Price = updateDto.Price;
        product.StockQuantity = updateDto.StockQuantity;
        product.Category = sanitizationService.SanitizeInput(updateDto.Category);
        product.IsActive = updateDto.IsActive;
    }

    public static IEnumerable<ProductResponseDto> ToResponseDtos(this IEnumerable<Product> products)
    {
        return products.Select(p => p.ToResponseDto());
    }
}