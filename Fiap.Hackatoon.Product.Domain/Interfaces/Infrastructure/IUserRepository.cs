using Fiap.Hackatoon.Product.Domain.Entities;

namespace Fiap.Hackatoon.Product.Domain.Interfaces.Infrastructure;

public interface IUserRepository : IRepository<User>
{
    Task<User> GetById(Guid id, bool include = false, bool tracking = false);
}