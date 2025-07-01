using DO = Fiap.Hackatoon.Product.Domain.Entities;

namespace Fiap.Hackatoon.Product.Domain.Interfaces;

public interface IProductService : IBaseService<DO.Product>
{
    Task<DO.Product> GetById(Guid id, bool include, bool tracking);
    Task<List<DO.Product>> GetByType(string nameOrCode);
}