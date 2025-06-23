namespace Fiap.Hackatoon.Product.Domain.Entities;

public class OrderStatus : BaseEntity
{
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;

    public ICollection<Order> Orders { get; set; } = new List<Order>();
}