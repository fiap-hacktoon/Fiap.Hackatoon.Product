using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using DO = Fiap.Hackatoon.Product.Domain.Entities;

namespace Fiap.Hackatoon.Product.Infrastructure.Data.Configurations;

public class ProductTypeConfiguration : BaseEntityConfiguration<DO.ProductType>
{
    public override void Configure(EntityTypeBuilder<DO.ProductType> builder)
    {
        base.Configure(builder);

        builder.Property(p => p.Name).IsRequired().HasMaxLength(50);
        builder.Property(p => p.Code).IsRequired().HasMaxLength(20);
        builder.Property(p => p.Description).IsRequired().HasMaxLength(250);
    }
}