using ProductInventoryAPI.DTOs;
using ProductInventoryAPI.Models;

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
            IsActive = product.IsActive
        };
    }

    public static Product ToEntity(this ProductCreateDto createDto)
    {
        return new Product
        {
            Name = createDto.Name,
            Description = createDto.Description,
            Price = createDto.Price,
            StockQuantity = createDto.StockQuantity,
            Category = createDto.Category,
            IsActive = true
        };
    }

    public static void UpdateEntity(this ProductUpdateDto updateDto, Product product)
    {
        product.Name = updateDto.Name;
        product.Description = updateDto.Description;
        product.Price = updateDto.Price;
        product.StockQuantity = updateDto.StockQuantity;
        product.Category = updateDto.Category;
        product.IsActive = updateDto.IsActive;
    }

    public static IEnumerable<ProductResponseDto> ToResponseDtos(this IEnumerable<Product> products)
    {
        return products.Select(p => p.ToResponseDto());
    }
}