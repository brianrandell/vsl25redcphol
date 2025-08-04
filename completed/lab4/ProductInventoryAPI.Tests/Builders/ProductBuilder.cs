// Fluent builder pattern for creating test Product objects
// Should support:
// - Default valid product
// - Method chaining for setting properties
// - Build method to create the product
// - Multiple preset configurations (ValidProduct, InvalidProduct, ExpensiveProduct)
using ProductInventoryAPI.Models;
using ProductInventoryAPI.DTOs;

namespace ProductInventoryAPI.Tests.Builders;

public class ProductBuilder
{
    private int _id = 1;
    private string _name = "Test Product";
    private string _description = "A test product description";
    private decimal _price = 29.99m;
    private int _stockQuantity = 10;
    private string _category = "Electronics";
    private bool _isActive = true;

    public ProductBuilder WithId(int id)
    {
        _id = id;
        return this;
    }

    public ProductBuilder WithName(string name)
    {
        _name = name;
        return this;
    }

    public ProductBuilder WithDescription(string description)
    {
        _description = description;
        return this;
    }

    public ProductBuilder WithPrice(decimal price)
    {
        _price = price;
        return this;
    }

    public ProductBuilder WithStockQuantity(int stockQuantity)
    {
        _stockQuantity = stockQuantity;
        return this;
    }

    public ProductBuilder WithCategory(string category)
    {
        _category = category;
        return this;
    }

    public ProductBuilder WithIsActive(bool isActive)
    {
        _isActive = isActive;
        return this;
    }

    public Product Build()
    {
        return new Product
        {
            Id = _id,
            Name = _name,
            Description = _description,
            Price = _price,
            StockQuantity = _stockQuantity,
            Category = _category,
            IsActive = _isActive
        };
    }

    public ProductCreateDto BuildCreateDto()
    {
        return new ProductCreateDto
        {
            Name = _name,
            Description = _description,
            Price = _price,
            StockQuantity = _stockQuantity,
            Category = _category
        };
    }

    public ProductUpdateDto BuildUpdateDto()
    {
        return new ProductUpdateDto
        {
            Name = _name,
            Description = _description,
            Price = _price,
            StockQuantity = _stockQuantity,
            Category = _category,
            IsActive = _isActive
        };
    }

    // Preset configurations
    public static ProductBuilder ValidProduct()
    {
        return new ProductBuilder();
    }

    public static ProductBuilder InvalidProduct()
    {
        return new ProductBuilder()
            .WithName("")
            .WithPrice(-1)
            .WithStockQuantity(-1)
            .WithCategory("");
    }

    public static ProductBuilder ExpensiveProduct()
    {
        return new ProductBuilder()
            .WithName("Premium Laptop")
            .WithDescription("High-end gaming laptop with top specifications")
            .WithPrice(2999.99m)
            .WithStockQuantity(5)
            .WithCategory("Premium Electronics");
    }

    public static ProductBuilder CheapProduct()
    {
        return new ProductBuilder()
            .WithName("Budget Mouse")
            .WithDescription("Basic optical mouse")
            .WithPrice(9.99m)
            .WithStockQuantity(100)
            .WithCategory("Accessories");
    }

    public static ProductBuilder OutOfStockProduct()
    {
        return new ProductBuilder()
            .WithName("Out of Stock Item")
            .WithDescription("This item is currently out of stock")
            .WithPrice(49.99m)
            .WithStockQuantity(0)
            .WithCategory("Limited");
    }

    public static ProductBuilder InactiveProduct()
    {
        return new ProductBuilder()
            .WithName("Discontinued Product")
            .WithDescription("This product has been discontinued")
            .WithPrice(19.99m)
            .WithStockQuantity(0)
            .WithCategory("Discontinued")
            .WithIsActive(false);
    }

    public static ProductBuilder LongNameProduct()
    {
        return new ProductBuilder()
            .WithName("This is a very long product name that exceeds the maximum allowed length of 100 characters for testing validation rules");
    }

    public static ProductBuilder LongDescriptionProduct()
    {
        var longDescription = new string('a', 501); // 501 characters
        return new ProductBuilder()
            .WithDescription(longDescription);
    }

    public static ProductBuilder MinimalValidProduct()
    {
        return new ProductBuilder()
            .WithName("A")
            .WithDescription("")
            .WithPrice(0.01m)
            .WithStockQuantity(0)
            .WithCategory("A");
    }

    public static ProductBuilder ElectronicsProduct()
    {
        return new ProductBuilder()
            .WithName("Electronics Item")
            .WithDescription("An electronic device")
            .WithPrice(199.99m)
            .WithStockQuantity(25)
            .WithCategory("Electronics");
    }

    public static ProductBuilder BookProduct()
    {
        return new ProductBuilder()
            .WithName("Programming Book")
            .WithDescription("Learn programming with this comprehensive guide")
            .WithPrice(39.99m)
            .WithStockQuantity(50)
            .WithCategory("Books");
    }

    public static ProductBuilder ClothingProduct()
    {
        return new ProductBuilder()
            .WithName("T-Shirt")
            .WithDescription("Comfortable cotton t-shirt")
            .WithPrice(24.99m)
            .WithStockQuantity(75)
            .WithCategory("Clothing");
    }
}