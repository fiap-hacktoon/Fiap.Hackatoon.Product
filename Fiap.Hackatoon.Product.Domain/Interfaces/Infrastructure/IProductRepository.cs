using DO = Fiap.Hackatoon.Product.Domain.Entities;
using VIEW = Fiap.Hackatoon.Product.Domain.Views.ElasticSearch;

namespace Fiap.Hackatoon.Product.Domain.Interfaces.Infrastructure;

public interface IProductRepository : IRepository<DO.Product>
{
    Task<DO.Product> GetById(Guid id, bool include = false, bool tracking = false);
    Task<List<VIEW.ProductByType>> GetByType(string nameOrCode);
}