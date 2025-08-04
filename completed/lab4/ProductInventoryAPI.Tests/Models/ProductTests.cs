// Unit tests for Product model validation
// Test cases needed:
// - Valid product creation
// - Name validation (null, empty, too long)
// - Price validation (negative, zero, valid)
// - Stock quantity validation (negative, valid)
// Use FluentAssertions for assertions
using Xunit;
using FluentAssertions;
using ProductInventoryAPI.Models;
using System.ComponentModel.DataAnnotations;

namespace ProductInventoryAPI.Tests.Models;

public class ProductTests
{
    [Fact]
    public void Product_WithValidData_ShouldPassAllValidations()
    {
        // Arrange
        var product = new Product
        {
            Id = 1,
            Name = "Test Product",
            Description = "A test product description",
            Price = 29.99m,
            StockQuantity = 10,
            Category = "Electronics",
            IsActive = true
        };

        // Act
        var validationResults = ValidateModel(product);

        // Assert
        validationResults.Should().BeEmpty();
        product.Name.Should().Be("Test Product");
        product.Price.Should().Be(29.99m);
        product.StockQuantity.Should().Be(10);
        product.IsActive.Should().BeTrue();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData("This is a very long product name that exceeds the maximum allowed length of 100 characters for the Name property which should trigger a validation error")]
    public void Product_WithInvalidName_ShouldFailValidation(string? invalidName)
    {
        // Arrange
        var product = new Product
        {
            Id = 1,
            Name = invalidName!,
            Description = "Valid description",
            Price = 29.99m,
            StockQuantity = 10,
            Category = "Electronics",
            IsActive = true
        };

        // Act
        var validationResults = ValidateModel(product);

        // Assert
        validationResults.Should().NotBeEmpty();
        validationResults.Should().Contain(r => r.MemberNames.Contains("Name"));
    }

    [Fact]
    public void Product_WithNullName_ShouldHaveRequiredErrorMessage()
    {
        // Arrange
        var product = new Product
        {
            Id = 1,
            Name = null!,
            Description = "Valid description",
            Price = 29.99m,
            StockQuantity = 10,
            Category = "Electronics"
        };

        // Act
        var validationResults = ValidateModel(product);

        // Assert
        var nameValidationResult = validationResults.FirstOrDefault(r => r.MemberNames.Contains("Name"));
        nameValidationResult.Should().NotBeNull();
        nameValidationResult!.ErrorMessage.Should().Be("Product name is required");
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(0)]
    public void Product_WithInvalidPrice_ShouldFailValidation(decimal invalidPrice)
    {
        // Arrange
        var product = new Product
        {
            Id = 1,
            Name = "Test Product",
            Description = "Valid description",
            Price = invalidPrice,
            StockQuantity = 10,
            Category = "Electronics",
            IsActive = true
        };

        // Act
        var validationResults = ValidateModel(product);

        // Assert
        validationResults.Should().NotBeEmpty();
        validationResults.Should().Contain(r => r.MemberNames.Contains("Price"));
        
        var priceValidationResult = validationResults.FirstOrDefault(r => r.MemberNames.Contains("Price"));
        priceValidationResult!.ErrorMessage.Should().Be("Price must be greater than 0");
    }

    [Fact]
    public void Product_WithValidPrice_ShouldPassValidation()
    {
        // Arrange
        var product = new Product
        {
            Id = 1,
            Name = "Test Product",
            Description = "Valid description",
            Price = 0.01m,
            StockQuantity = 10,
            Category = "Electronics",
            IsActive = true
        };

        // Act
        var validationResults = ValidateModel(product);

        // Assert
        validationResults.Where(r => r.MemberNames.Contains("Price")).Should().BeEmpty();
    }

    [Fact]
    public void Product_WithNegativeStockQuantity_ShouldFailValidation()
    {
        // Arrange
        var product = new Product
        {
            Id = 1,
            Name = "Test Product",
            Description = "Valid description",
            Price = 29.99m,
            StockQuantity = -1,
            Category = "Electronics",
            IsActive = true
        };

        // Act
        var validationResults = ValidateModel(product);

        // Assert
        validationResults.Should().NotBeEmpty();
        var stockValidationResult = validationResults.FirstOrDefault(r => r.MemberNames.Contains("StockQuantity"));
        stockValidationResult.Should().NotBeNull();
        stockValidationResult.ErrorMessage.Should().Be("Stock quantity cannot be negative");
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(100)]
    [InlineData(int.MaxValue)]
    public void Product_WithValidStockQuantity_ShouldPassValidation(int validStock)
    {
        // Arrange
        var product = new Product
        {
            Id = 1,
            Name = "Test Product",
            Description = "Valid description",
            Price = 29.99m,
            StockQuantity = validStock,
            Category = "Electronics",
            IsActive = true
        };

        // Act
        var validationResults = ValidateModel(product);

        // Assert
        validationResults.Where(r => r.MemberNames.Contains("StockQuantity")).Should().BeEmpty();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("This is a very long category name that exceeds fifty characters")]
    public void Product_WithInvalidCategory_ShouldFailValidation(string? invalidCategory)
    {
        // Arrange
        var product = new Product
        {
            Id = 1,
            Name = "Test Product",
            Description = "Valid description",
            Price = 29.99m,
            StockQuantity = 10,
            Category = invalidCategory!,
            IsActive = true
        };

        // Act
        var validationResults = ValidateModel(product);

        // Assert
        validationResults.Should().NotBeEmpty();
        validationResults.Should().Contain(r => r.MemberNames.Contains("Category"));
    }

    [Fact]
    public void Product_WithLongDescription_ShouldFailValidation()
    {
        // Arrange
        var longDescription = new string('a', 501); // 501 characters
        var product = new Product
        {
            Id = 1,
            Name = "Test Product",
            Description = longDescription,
            Price = 29.99m,
            StockQuantity = 10,
            Category = "Electronics",
            IsActive = true
        };

        // Act
        var validationResults = ValidateModel(product);

        // Assert
        validationResults.Should().NotBeEmpty();
        var descriptionValidationResult = validationResults.FirstOrDefault(r => r.MemberNames.Contains("Description"));
        descriptionValidationResult.Should().NotBeNull();
        descriptionValidationResult.ErrorMessage.Should().Be("Description cannot exceed 500 characters");
    }

    [Fact]
    public void Product_DefaultValues_ShouldBeCorrect()
    {
        // Arrange & Act
        var product = new Product();

        // Assert
        product.Name.Should().Be(string.Empty);
        product.Description.Should().Be(string.Empty);
        product.Category.Should().Be(string.Empty);
        product.IsActive.Should().BeTrue();
        product.Id.Should().Be(0);
        product.Price.Should().Be(0);
        product.StockQuantity.Should().Be(0);
    }

    private static List<ValidationResult> ValidateModel(object model)
    {
        var validationResults = new List<ValidationResult>();
        var context = new ValidationContext(model);
        Validator.TryValidateObject(model, context, validationResults, true);
        return validationResults;
    }
}