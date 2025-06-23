using System.Text.Json.Serialization;

namespace Fiap.Hackatoon.Product.Application.DataTransferObjects;

public class Identifier
{
    [JsonPropertyName("id")]
    public Guid? Id { get; set; }

    public Identifier() { }
}