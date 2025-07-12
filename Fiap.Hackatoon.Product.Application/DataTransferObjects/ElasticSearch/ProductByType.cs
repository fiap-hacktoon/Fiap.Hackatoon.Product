using System.Text.Json.Serialization;

namespace Fiap.Hackatoon.Product.Application.DataTransferObjects.ElasticSearch;

public class ProductByType : BaseModel
{
    [JsonPropertyName("product_name")]
    public string ProductName { get; set; }

    [JsonPropertyName("description")]
    public string Description { get; set; }

    [JsonPropertyName("type_id")]
    public Guid TypeId { get; set; }

    [JsonPropertyName("type_code")]
    public string TypeCode { get; set; }

    [JsonPropertyName("type_name")]
    public string TypeName { get; set; }

    [JsonPropertyName("type_description")]
    public string TypeDescription { get; set; }

    [JsonPropertyName("price")]
    public decimal Price { get; set; }

    [JsonPropertyName("stock_quantity")]
    public int StockQuantity { get; set; }

    [JsonPropertyName("status")]
    public int Status { get; set; }
}