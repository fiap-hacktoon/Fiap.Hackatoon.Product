namespace Fiap.Hackatoon.Product.Domain.Entities;

public class Product : BaseEntity
{
    public Guid TypeId { get; set; }
    public ProductType Type { get; set; } = null!;

    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public decimal Price { get; set; }
    public int StockQuantity { get; set; }
    public int Status { get; set; }
}