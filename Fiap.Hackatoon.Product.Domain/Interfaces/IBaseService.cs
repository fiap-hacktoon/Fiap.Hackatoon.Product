using Fiap.Hackatoon.Product.Domain.Entities.Interfaces;
using Fiap.Hackatoon.Product.Domain.Interfaces.Infrastructure;

namespace Fiap.Hackatoon.Product.Domain.Interfaces;

public interface IBaseService<T> : IRepository<T> where T : class, IBaseEntity
{
    Task Remove(T entity);
}