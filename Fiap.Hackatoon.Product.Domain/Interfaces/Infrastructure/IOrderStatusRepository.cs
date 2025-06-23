using DO = Fiap.Hackatoon.Product.Domain.Entities;

namespace Fiap.Hackatoon.Product.Domain.Interfaces.Infrastructure;

public interface IOrderStatusRepository : IRepository<DO.OrderStatus>
{
    Task<DO.OrderStatus> GetById(Guid id, bool include = false, bool tracking = false);
}