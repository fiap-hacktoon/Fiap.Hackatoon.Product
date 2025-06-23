using DO = Fiap.Hackatoon.Product.Domain.Entities;

namespace Fiap.Hackatoon.Product.Domain.Interfaces.Infrastructure;

public interface IOrderProductRepository : IRepository<DO.OrderProduct>
{
    Task<DO.OrderProduct> GetById(Guid id, bool include = false, bool tracking = false);
}