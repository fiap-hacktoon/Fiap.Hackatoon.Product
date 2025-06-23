using DO = Fiap.Hackatoon.Product.Domain.Entities;

namespace Fiap.Hackatoon.Product.Domain.Interfaces.Infrastructure;

public interface IClientRepository : IRepository<DO.Client>
{
    Task<DO.Client> GetById(Guid id, bool include = false, bool tracking = false);
}