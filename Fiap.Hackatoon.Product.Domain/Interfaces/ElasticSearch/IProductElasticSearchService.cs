#nullable enable
using DO = Fiap.Hackatoon.Product.Domain.Entities;
using VIEW = Fiap.Hackatoon.Product.Domain.Views.ElasticSearch;

namespace Fiap.Hackatoon.Product.Domain.Interfaces.ElasticSearch;

public interface IProductElasticSearchService : IElasticSearchService<VIEW.ProductByType>
{
    Task<List<VIEW.ProductByType>> GetByTypeCode(string typeCodeOrName, int page = 0, int size = 20);
}