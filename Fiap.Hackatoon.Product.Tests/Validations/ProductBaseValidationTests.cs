using Xunit;
using System.ComponentModel.DataAnnotations;
using Fiap.Hackatoon.Product.Application.DataTransferObjects;
using System;
using FluentAssertions;
using DTO = Fiap.Hackatoon.Product.Application.DataTransferObjects;

namespace Fiap.Hackatoon.Product.Tests.Validations;

public class ProductValidationTests : BaseValidationTest
{
    [Fact]
    public void Should_Pass_Validation_When_All_Fields_Are_Valid()
    {
        // Arrange
        var dto = new DTO.Product
        {
            TypeId = Guid.NewGuid(),
            Name = "Coca-Cola",
            Description = "Bebida gelada",
            Price = 5.99m,
            StockQuantity = 100
        };

        var context = new ValidationContext(dto, null, null);
        var results = new List<ValidationResult>();

        // Act
        var isValid = Validator.TryValidateObject(dto, context, results, true);

        // Assert
        isValid.Should().BeTrue();
        results.Should().BeEmpty();
    }

    /// <summary>
    /// Dados para testar Product inválido
    /// </summary>
    public static IEnumerable<object[]> GetInvalidProductData()
    {
        yield return new object[] { new DTO.Product { Name = "Produto", Description = "Desc", Price = 10.0m, StockQuantity = 5 } }; // TypeId ausente
        yield return new object[] { new DTO.Product { TypeId = Guid.NewGuid(), Description = "Desc", Price = 10.0m, StockQuantity = 5 } }; // Name ausente
        yield return new object[] { new DTO.Product { TypeId = Guid.NewGuid(), Name = "Produto", Price = 10.0m, StockQuantity = 5 } }; // Description ausente
        yield return new object[] { new DTO.Product { TypeId = Guid.NewGuid(), Name = "Produto", Description = "Desc", Price = 0m, StockQuantity = 5 } }; // Price ausente
        yield return new object[] { new DTO.Product { TypeId = Guid.NewGuid(), Name = "Produto", Description = "Desc", Price = 10.0m, StockQuantity = -1 } }; // StockQuantity ausente

    }

    [Theory]
    [MemberData(nameof(GetInvalidProductData))]
    public void Product_ShouldHaveRequiredAttribute_AllResultsInvalid(DTO.Product product)
    {
        // Act
        var validationResults = ValidateModel(product);

        // Assert
        validationResults.Should().NotBeEmpty();
    }

    [Fact]
    public void Should_Return_Error_When_TypeId_Is_Missing()
    {
        // Arrange
        var product = new DTO.Product { Name = "Produto", Description = "Desc", Price = 10.0m, StockQuantity = 5 };

        // Act
        var results = ValidateModel(product);

        // Assert
        Assert.Contains(results, r => r.ErrorMessage == "O tipo do produto é obrigatório.");
    }

    [Fact]
    public void Should_Return_Error_When_Name_Is_Missing()
    {
        // Arrange
        var product = new DTO.Product
        {
            TypeId = Guid.NewGuid(),
            Description = "Desc",
            Price = 10.0m,
            StockQuantity = 5
        };

        // Act
        var results = ValidateModel(product);

        // Assert
        Assert.Contains(results, r => r.ErrorMessage == "O campo nome é obrigatório.");
    }

    [Fact]
    public void Should_Return_Error_When_Description_Is_Missing()
    {
        // Arrange
        var product = new DTO.Product{ TypeId = Guid.NewGuid(), Name = "Produto", Price = 10.0m, StockQuantity = 5 };

        // Act
        var results = ValidateModel(product);

        // Assert
        Assert.Contains(results, r => r.ErrorMessage == "O campo descrição é obrigatório.");
    }

    [Fact]
    public void Should_Return_Error_When_Price_IsZero()
    {
        // Arrange
        var product = new DTO.Product { TypeId = Guid.NewGuid(), Name = "Produto", Description = "Desc", Price = 0m, StockQuantity = 5 };

        // Act
        var results = ValidateModel(product);

        // Assert
        Assert.Contains(results, r => r.ErrorMessage == "O preço deve ser maior que zero.");
    }

    [Fact]
    public void Should_Return_Error_When_StockQuantity_Is_Missing()
    {
        // Arrange
        var product = new DTO.Product { TypeId = Guid.NewGuid(), Name = "Produto", Description = "Desc", Price = 10.0m, StockQuantity = -1 };

        // Act
        var results = ValidateModel(product);

        // Assert
        Assert.Contains(results, r => r.ErrorMessage == "A quantidade em estoque não pode ser negativa.");
    }
}
