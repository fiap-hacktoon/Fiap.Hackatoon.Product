using System.ComponentModel.DataAnnotations;
using DO = Fiap.Hackatoon.Product.Domain.Entities;
using Fiap.Hackatoon.Product.Domain.Interfaces;
using Fiap.Hackatoon.Product.Domain.Interfaces.Infrastructure;

namespace Fiap.Hackatoon.Product.Domain.Services;

public class ProductService(IProductRepository productRepository) : BaseService<DO.Product>(productRepository), IProductService
{
    private readonly IProductRepository _productRepository = productRepository;

    public async Task<DO.Product> GetById(Guid id, bool include, bool tracking)
    {
        var entity = await _productRepository.GetById(id, include, tracking);

        if (entity == null)
            throw new ValidationException("O produto não existe.");

        return entity;
    }

    public override async Task<DO.Product> Add(DO.Product entity)
    {
        var product = await _productRepository.GetById(entity.Id);

        if (product != null)
            throw new ValidationException("O produto já existe.");
        
        return await base.Add(entity);
    }

    public override async Task<DO.Product> Update(DO.Product entity)
    {
        return await base.Update(entity);
    }

    public async Task Remove(Guid id)
    {
        var entity = await _productRepository.GetById(id, false, true);
        if (entity == null)
            throw new Exception("O produto não existe.");

        await base.Remove(entity);
    }
}