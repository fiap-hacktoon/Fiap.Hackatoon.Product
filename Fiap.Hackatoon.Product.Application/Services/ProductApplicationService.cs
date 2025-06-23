using System.Text.Json;
using AutoMapper;
using DO = Fiap.Hackatoon.Product.Domain.Entities;
using MSG = Fiap.Hackatoon.Product.Application.DataTransferObjects.MessageBrokers;
using Fiap.Hackatoon.Product.Application.Interfaces;
using DTO = Fiap.Hackatoon.Product.Application.DataTransferObjects;
using Fiap.Hackatoon.Product.Domain.Interfaces;
using Fiap.Hackatoon.Product.Domain.Constants;

namespace Fiap.Hackatoon.Product.Application.Services;

public class ProductApplicationService(IProductService productService, IMapper mapper) : IProductApplicationService
{
    private readonly IProductService _productService = productService;
    private readonly IMapper _mapper = mapper;

    public async Task<DTO.Product> Add(DTO.Product model)
    {
        var product = _mapper.Map<DO.Product>(model);

        product = await _productService.Add(product);

        return _mapper.Map<DTO.Product>(product);
    }

    public async Task<DTO.Product> Update(DTO.Product model)
    {
        if (!model.Id.HasValue)
            throw new Exception("Dados inválidos para atualização do produto.");

        var product = await _productService.GetById(model.Id.Value, include: false, tracking: true);
        if (product == null)
            throw new Exception("O produto não existe.");
            
        _mapper.Map(model, product);

        product = await _productService.Update(product);

        return _mapper.Map<DTO.Product>(product);
    }

    public async Task<DTO.Product> Add(MSG.Product model)
    {
        var product = _mapper.Map<DO.Product>(model);

        product = await _productService.Add(product);

        return _mapper.Map<DTO.Product>(product);
    }

    public async Task<DTO.Product> Update(MSG.Product model)
    {
        var product = await _productService.GetById(model.Id, include: false, tracking: true);
        if (product == null)
            throw new Exception("O contato não existe.");
            
        _mapper.Map(model, product);

        product = await _productService.Update(product);

        return _mapper.Map<DTO.Product>(product);
    }

    public async Task<DTO.Product> GetById(Guid id)
    {
        var contact = await _productService.GetById(id, include: false, tracking: false);
        return _mapper.Map<DTO.Product>(contact);
    }

    public async Task<bool> Delete(Guid id)
    {
        var product = await _productService.GetById(id, include: false, tracking: true);
        if (product == null)
            throw new Exception("O produto não existe.");

        await _productService.Remove(product);

        return true;
    }

    public async Task Consumer(string message, string rountingKey)
    {
        switch(rountingKey)
        {
            case AppConstants.Routes.RabbitMQ.ProductInsert:
                var contactInsert = JsonSerializer.Deserialize<MSG.Product>(message) ?? throw new Exception("Mensagem inválida para inserção de produto.");
                await Add(contactInsert);
                break;

            case AppConstants.Routes.RabbitMQ.ProductUpdate:
                var contactUpdate = JsonSerializer.Deserialize<MSG.Product>(message) ?? throw new Exception("Mensagem inválida para atualização de produto.");
                await Update(contactUpdate);
                break;
            
            case AppConstants.Routes.RabbitMQ.ProductDelete:
                var contactDelete = JsonSerializer.Deserialize<MSG.Product>(message) ?? throw new Exception("Mensagem inválida para exclusão de produto.");
                if (contactDelete == null || contactDelete.Id == Guid.Empty)
                    throw new Exception("O produto não existe.");
                
                await Delete(contactDelete.Id);
                break;
        }
    }

    public void Dispose()
    {
        _productService.Dispose();

        GC.SuppressFinalize(this);
    }
}
