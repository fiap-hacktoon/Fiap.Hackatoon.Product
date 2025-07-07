using EN = Fiap.Hackatoon.Product.Domain.Entities;
using Fiap.Hackatoon.Product.Tests.Shared.Utils;

namespace Fiap.Hackatoon.Product.Tests.Domain.EntitiesFixtures;

public sealed class ProductFixtures : BaseFixtures<EN.Product>
{
    public ProductFixtures() : base() { }

    public static EN.Product CreateAs_Base()
    {
        var product = new EN.Product()
        {
            Id = FakerDefault.Random.Guid(),
            Name = FakerDefault.Random.String2(5, 20),
            Description = FakerDefault.Random.String2(10, 50),
            Price = FakerDefault.Random.Decimal(1.0m, 1000.0m),
            StockQuantity = FakerDefault.Random.Int(1, 100),
            Status = 1,
            Removed = false
        };

        return product;
    }
}