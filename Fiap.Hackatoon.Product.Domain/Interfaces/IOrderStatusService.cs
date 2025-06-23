using DO = Fiap.Hackatoon.Product.Domain.Entities;

namespace Fiap.Hackatoon.Product.Domain.Interfaces;

public interface IOrderStatusService : IBaseService<DO.OrderStatus>
{
    Task Remove(Guid id);
}