using System.Text.Json;
using Elastic.Clients.Elasticsearch;
using Elastic.Clients.Elasticsearch.QueryDsl;
using Fiap.Hackatoon.Product.Domain.Interfaces.ElasticSearch;
using Fiap.Hackatoon.Product.Domain.Views.ElasticSearch;

namespace Fiap.Hackatoon.Product.Infrastructure.ElasticSearch.Services;

public class ProductElasticSearchService : IProductElasticSearchService, IDisposable
{
    private readonly IElasticSearchService<ProductByType> _elastic;
    private readonly JsonSerializerOptions _jsonOptions;
    private readonly string _indexName = "products";

    public ProductElasticSearchService(IElasticSearchService<ProductByType> elastic)
    {
        _elastic = elastic;
        _jsonOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = true
        };
    }

    public Task<bool> Create(ProductByType register, IndexName index)
    {
        return _elastic.Create(register, _indexName);
    }

    public Task Update(ProductByType register, string indexName)
    {
        return _elastic.Update(register, _indexName);
    }

    public Task<IReadOnlyCollection<ProductByType>> Get(int page, int size, IndexName index)
    {
        return _elastic.Get(page, size, _indexName);
    }

    public Task<IReadOnlyCollection<ProductByType>> Search(IndexName index, Func<QueryDescriptor<ProductByType>, QueryDescriptor<ProductByType>> query, int page = 0, int size = 10)
    {
        return _elastic.Search(_indexName, query, page, size);
    }

    public Task<ProductByType> GetByIdAsync(string index, Guid id)
    {
        return _elastic.GetByIdAsync(_indexName, id);
    }

    public async Task<List<ProductByType>> GetByTypeCode(string typeCodeOrName, int page = 0, int size = 20)
    {
        var products = await _elastic.Search(
            index: _indexName,
            page: page,
            size: size,
            query: q => q
                .Bool(b => b
                    .Should(
                        s => s.Match(m => m.Field("TypeCode").Query(typeCodeOrName)),
                        s => s.Match(m => m.Field("TypeName").Query(typeCodeOrName))
                    )
                )
        );

        return products?.Select(p => new ProductByType
        {
            Id = p.Id,
            ProductName = p.ProductName,
            TypeId = p.TypeId,
            TypeCode = p.TypeCode,
            TypeName = p.TypeName,
            TypeDescription = p.TypeDescription,
            Price = p.Price,
            StockQuantity = p.StockQuantity,
            Status = (int)p.Status,
            Description = p.Description
        }).ToList() ?? new List<ProductByType>();
    }

    public void Dispose()
    {
        _elastic.Dispose();

        GC.SuppressFinalize(this);
    }
}
