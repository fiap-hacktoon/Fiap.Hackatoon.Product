using Fiap.Hackatoon.Product.Domain.Entities.Interfaces;

namespace Fiap.Hackatoon.Product.Domain.Entities;

public abstract class BaseEntity : Identifier, IBaseEntity
{
    public DateTime CreatedAt { get; set; }
    public bool Removed { get; set; }
    public DateTime? RemovedAt { get; set; }

    public virtual void PrepareToInsert()
    {
        Id = Guid.NewGuid();
        CreatedAt = DateTime.Now;
    }

    public virtual void PrepareToUpdate() { }

    public virtual void PrepareToRemove()
    {
        Removed = true;
        RemovedAt = DateTime.Now;
    }
}