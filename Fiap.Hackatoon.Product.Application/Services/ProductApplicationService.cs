using AutoMapper;
using MSG = Fiap.Hackatoon.Product.Application.DataTransferObjects.MessageBrokers;
using Fiap.Hackatoon.Product.Application.Interfaces;
using DTO = Fiap.Hackatoon.Product.Application.DataTransferObjects;
using Fiap.Hackatoon.Product.Domain.Interfaces;
using Fiap.Hackatoon.Product.Domain.Constants;
using Fiap.Hackatoon.Product.Domain.Interfaces.RabbitMQ;
using Fiap.Hackatoon.Product.Application.DataTransferObjects;

namespace Fiap.Hackatoon.Product.Application.Services;

public class ProductApplicationService(IProductService productService, IMapper mapper, IRabbitMQPublisher rabbitMqService) : IProductApplicationService
{
    private readonly IProductService _productService = productService;
    private readonly IRabbitMQPublisher _rabbitMqService = rabbitMqService;
    private readonly IMapper _mapper = mapper;

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
        var product = await _productService.GetById(id, include: false, tracking: false);
        return _mapper.Map<DTO.Product>(product);
    }

    public async Task<List<DTO.Product>> GetByType(string nameOrCode)
    {
        var products = await _productService.GetByType(nameOrCode);
        return _mapper.Map<List<DTO.Product>>(products);
    }

    public void Dispose()
    {
        _productService.Dispose();

        GC.SuppressFinalize(this);
    }
}
