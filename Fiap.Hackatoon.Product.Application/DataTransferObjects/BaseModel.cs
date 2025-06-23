using System.Text.Json.Serialization;

namespace Fiap.Hackatoon.Product.Application.DataTransferObjects;

public class BaseModel : Identifier
{
    [JsonPropertyName("created_at")]
    public DateTime CreatedAt { get; set; }

    [JsonPropertyName("removed")] 
    public bool Removed { get; set; } 

    [JsonPropertyName("removed_at")]
    public DateTime? RemovedAt { get; set; }

    public BaseModel() : base() { }
}