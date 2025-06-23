using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Fiap.Hackatoon.Product.Application.DataTransferObjects;

public class UserLogin : Identifier
{
    [JsonPropertyName("email")]
    [EmailAddress(ErrorMessage = "Email inválido.")]
    [Required(ErrorMessage = "O email é obrigatório.")]
    public string? Email { get; set; }

    [JsonPropertyName("password")]
    [Required(ErrorMessage = "A senha é obrigatória.")]
    public string? Password { get; set; }
}