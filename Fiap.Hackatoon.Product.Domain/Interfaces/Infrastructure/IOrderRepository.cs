using DO = Fiap.Hackatoon.Product.Domain.Entities;

namespace Fiap.Hackatoon.Product.Domain.Interfaces.Infrastructure;

public interface IOrderRepository : IRepository<DO.Order>
{
    Task<DO.Order> GetById(Guid id, bool include = false, bool tracking = false);
}