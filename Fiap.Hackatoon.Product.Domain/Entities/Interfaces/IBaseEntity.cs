namespace Fiap.Hackatoon.Product.Domain.Entities.Interfaces;

public interface IBaseEntity : IIdentifier
{
    public DateTime CreatedAt { get; set; }
    public bool Removed { get; set; }
    public DateTime? RemovedAt { get; set; }

    public void PrepareToInsert();

    public void PrepareToUpdate();

    public void PrepareToRemove();
}