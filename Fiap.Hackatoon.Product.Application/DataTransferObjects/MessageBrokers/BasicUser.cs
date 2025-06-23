namespace Fiap.Hackatoon.Product.Application.DataTransferObjects.MessageBrokers;

public record BasicUser(
    string Name, 
    string Email,
    string Password
);