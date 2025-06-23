using DO = Fiap.Hackatoon.Product.Domain.Entities;

namespace Fiap.Hackatoon.Product.Domain.Interfaces;

public interface IOrderProductService : IBaseService<DO.OrderProduct>
{
    Task Remove(Guid id);
}