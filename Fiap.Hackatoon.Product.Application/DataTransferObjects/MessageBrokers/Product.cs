namespace Fiap.Hackatoon.Product.Application.DataTransferObjects.MessageBrokers;


public record Product(
    Guid? Id,
    string? Name,
    string? Description,
    decimal? Price,
    int? StockQuantity,
    int? Status,
    Guid? TypeId
);
