using Fiap.Hackatoon.Product.Domain.Entities.Interfaces;

namespace Fiap.Hackatoon.Product.Domain.Entities;

public abstract class Identifier : IIdentifier
{
    public Guid Id { get; set; }
}