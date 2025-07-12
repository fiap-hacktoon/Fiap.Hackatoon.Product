using AutoMapper;
using MSG = Fiap.Hackatoon.Product.Application.DataTransferObjects.MessageBrokers;
using Fiap.Hackatoon.Product.Application.Interfaces;
using DTO = Fiap.Hackatoon.Product.Application.DataTransferObjects;
using DTOE = Fiap.Hackatoon.Product.Application.DataTransferObjects.ElasticSearch;
using DO = Fiap.Hackatoon.Product.Domain.Entities;
using Fiap.Hackatoon.Product.Domain.Interfaces;
using Fiap.Hackatoon.Product.Domain.Constants;
using Fiap.Hackatoon.Product.Domain.Interfaces.RabbitMQ;
using Fiap.Hackatoon.Product.Application.DataTransferObjects;
using Fiap.Hackatoon.Product.Domain.Interfaces.ElasticSearch;

namespace Fiap.Hackatoon.Product.Application.Services;

public class ProductApplicationService(
    IProductService productService, 
    IMapper mapper, 
    IRabbitMQPublisher rabbitMqService, 
    IProductElasticSearchService elasticSearchService) : IProductApplicationService
{
    private readonly IProductService _productService = productService;
    private readonly IRabbitMQPublisher _rabbitMqService = rabbitMqService;
    private readonly IMapper _mapper = mapper;
    private readonly IProductElasticSearchService _elasticSearchService = elasticSearchService;

    public async Task Add(DTO.Product model)
        => await _rabbitMqService.PublishAsync(_mapper.Map<MSG.Product>(model), AppConstants.Routes.RabbitMQ.ProductInsert);

    public async Task Update(DTO.Product model)
    {
        if (!model.Id.HasValue)
            throw new Exception("Dados inválidos para atualização do produto.");

        var product = await _productService.GetById(model.Id.Value, include: false, tracking: true);
        if (product == null)
            throw new Exception("O produto não existe.");

        await _rabbitMqService.PublishAsync(_mapper.Map<MSG.Product>(model), AppConstants.Routes.RabbitMQ.ProductUpdate);
    }

    public async Task Delete(Guid id)
    {
        var product = await _productService.GetById(id, include: false, tracking: true);
        if (product == null)
            throw new Exception("O produto não existe.");

        await _rabbitMqService.PublishAsync(_mapper.Map<DTO.Identifier>(new Identifier() { Id = product.Id }), AppConstants.Routes.RabbitMQ.ProductDelete);
    }

    public async Task<DTO.Product> GetById(Guid id)
    {
        var existsElastic = await _elasticSearchService.GetByIdAsync(nameof(DO.Product), id);

        if (existsElastic != null)
            return _mapper.Map<DTO.Product>(existsElastic);

        var exists = await _productService.GetById(id, include: false, tracking: false);

        if (exists == null)
            throw new Exception("O produto não existe.");

        return _mapper.Map<DTO.Product>(exists);
    }

    public async Task InsertElasticSearch(string nameOrCode)
    {
        var products = await _productService.GetByType(nameOrCode);

        foreach (var product in products)
            await _elasticSearchService.Create(product, nameof(DO.Product));
    }

    public async Task<List<DTOE.ProductByType>> GetByType(string typeCodeOrName, int page = 0, int size = 20)
    {
        var products = await _elasticSearchService.GetByTypeCode(typeCodeOrName, page, size);
        
        if (products != null && products.Count > 0)
            products = await _productService.GetByType(typeCodeOrName);

        return products != null && products.Count > 0 ? _mapper.Map<List<DTOE.ProductByType>>(products) : new List<DTOE.ProductByType>();
    }

    public void Dispose()
    {
        _productService.Dispose();

        GC.SuppressFinalize(this);
    }
}
