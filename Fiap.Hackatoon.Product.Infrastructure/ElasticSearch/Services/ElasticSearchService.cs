using System.Text;
using System.Text.Json;
using Elastic.Clients.Elasticsearch;
using Fiap.Hackatoon.Product.Domain.Entities.Interfaces;
using Fiap.Hackatoon.Product.Domain.Interfaces.ElasticSearch;
using Fiap.Hackatoon.Product.Domain.Settings;
using Microsoft.Extensions.Options;
using Elastic.Clients.Elasticsearch.QueryDsl;
using Elastic.Transport;

namespace Fiap.Hackatoon.Product.Infrastructure.ElasticSearch.Services;

public class ElasticSearchService<T> : IElasticSearchService<T> where T : class, IBaseEntity
{
    private readonly JsonSerializerOptions _jsonOptions;
    private readonly ElasticSearchSettings _elasticSearchSettings;
    private readonly ElasticsearchClient _elasticClient;

    public ElasticSearchService(IOptions<ElasticSearchSettings> elasticSearchOptions)
    {
        _elasticSearchSettings = elasticSearchOptions.Value;

        var settings = new ElasticsearchClientSettings(
            cloudId: _elasticSearchSettings.CloudId,
            credentials: new ApiKey(_elasticSearchSettings.ApiKey)
        )
            .DefaultFieldNameInferrer(p => p)
            .EnableDebugMode();

        _elasticClient = new ElasticsearchClient(settings);
        

        _jsonOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = true
        };
    }

    public async Task<bool> Create(T register, IndexName index)
    {
        var response = await _elasticClient.IndexAsync(register, idx => idx.Index(index));

        return response.IsValidResponse;
    }

    public async Task Update(T document, string indexName)
    {
        var indexResponse = await _elasticClient.IndexAsync(document, idx => idx.Index(indexName));
        await _elasticClient.IndexAsync(document, idx => idx.Index(indexName));

        if (!indexResponse.IsValidResponse)
        {
            throw new Exception(indexResponse.DebugInformation);
        }
    }

    public async Task<IReadOnlyCollection<T>> Get(int page, int size, IndexName index)
    {
        var response = await _elasticClient.SearchAsync<T>(s => s.Index(index)
                                                          .From(page)
                                                          .Size(size));
        return response.Documents;
    }

    public async Task<IReadOnlyCollection<T>> Search(
        IndexName index,
        Func<QueryDescriptor<T>, 
        QueryDescriptor<T>> queryDescriptorAction,
        int page = 0,
        int size = 10)
    {
        var response = await _elasticClient.SearchAsync<T>(s => s
            .Index(index)
            .From(page)
            .Size(size)
            .Query(q => queryDescriptorAction(q))
        );

        if (response != null && !response.IsValidResponse)
        {
            Console.WriteLine($"Erro na busca: {response.DebugInformation} + {nameof(T)}");
            return new List<T>();
        }

        return response.Documents;
    }

    public async Task<T?> GetByIdAsync(string index, Guid id)
    {
        var response = await _elasticClient.GetAsync<T>(id, idx => idx.Index(index));
        return response.Source;
    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);
    }
}
