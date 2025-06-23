using DO = Fiap.Hackatoon.Product.Domain.Entities;

namespace Fiap.Hackatoon.Product.Domain.Interfaces;

public interface IOrderService : IBaseService<DO.Order>
{
    Task Remove(Guid id);
}