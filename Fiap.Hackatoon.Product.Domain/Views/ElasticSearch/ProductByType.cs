using Fiap.Hackatoon.Product.Domain.Entities;

namespace Fiap.Hackatoon.Product.Domain.Views.ElasticSearch;

public class ProductByType : BaseEntity
{
    public required string ProductName { get; set; }
    public required Guid TypeId { get; set; }
    public required string TypeCode { get; set; }
    public required string TypeName { get; set; }
    public required string TypeDescription { get; set; }
    public required decimal Price { get; set; }
    public required int StockQuantity { get; set; }
    public required int Status { get; set; }
    public required string Description { get; set; }
}