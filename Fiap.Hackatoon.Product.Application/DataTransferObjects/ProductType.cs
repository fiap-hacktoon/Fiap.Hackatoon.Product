using System.Text.Json.Serialization;

namespace Fiap.Hackatoon.Product.Application.DataTransferObjects;

public class ProductType : BaseModel
{
    [JsonPropertyName("name")]
    public string Name { get; set; } = null!;

    [JsonPropertyName("description")]
    public string Description { get; set; } = null!;

    public ProductType() : base() { }
}