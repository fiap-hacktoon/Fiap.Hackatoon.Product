using System.ComponentModel.DataAnnotations;

namespace Fiap.Hackatoon.Product.Application.DataTransferObjects;

public class Product : BaseModel
{
    [Required(ErrorMessage = "O tipo do produto é obrigatório.")]
    [RegularExpression(@"^(?!00000000-0000-0000-0000-000000000000).*$", ErrorMessage = "O tipo do produto é obrigatório.")]
    public Guid TypeId { get; set; }

    public ProductType? Type { get; set; } = null;

    [Required(ErrorMessage = "O campo nome é obrigatório.")]
    [StringLength(255, ErrorMessage = "O nome deve ter no máximo 255 caracteres.")]
    public string Name { get; set; } = null!;

    [Required(ErrorMessage = "O campo descrição é obrigatório.")]
    [StringLength(500, ErrorMessage = "A descrição deve ter no máximo 500 caracteres.")]
    public string Description { get; set; } = null!;

    [Required(ErrorMessage = "O preço é obrigatório.")]
    [Range(0.01, double.MaxValue, ErrorMessage = "O preço deve ser maior que zero.")]
    public decimal Price { get; set; }

    [Required(ErrorMessage = "O campo quantidade em estoque é obrigatório.")]
    [Range(0, int.MaxValue, ErrorMessage = "A quantidade em estoque não pode ser negativa.")]
    public int StockQuantity { get; set; }

    public int Status { get; set; }

    public Product() : base() { }
}