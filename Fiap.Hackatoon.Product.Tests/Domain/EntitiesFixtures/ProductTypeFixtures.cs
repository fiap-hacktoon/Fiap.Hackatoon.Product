using EN = Fiap.Hackatoon.Product.Domain.Entities;
using Fiap.Hackatoon.Product.Tests.Shared.Utils;

namespace Fiap.Hackatoon.Product.Tests.Domain.EntitiesFixtures;

public sealed class ProductTypeFixtures : BaseFixtures<EN.ProductType>
{
    public ProductTypeFixtures() : base() { }

    public static EN.ProductType CreateAs_Base()
    {
        var type = new EN.ProductType()
        {
            Id = FakerDefault.Random.Guid(),
            Name = FakerDefault.Random.String2(5, 20),
            Description = FakerDefault.Random.String2(10, 50),
            Code = FakerDefault.Random.String2(3, 10),
        };

        return type;
    }
}