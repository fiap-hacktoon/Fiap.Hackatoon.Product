using System.ComponentModel.DataAnnotations;
using DO = Fiap.Hackatoon.Product.Domain.Entities;
using Fiap.Hackatoon.Product.Domain.Interfaces;
using Fiap.Hackatoon.Product.Domain.Interfaces.Infrastructure;

namespace Fiap.Hackatoon.Product.Domain.Services;

public class ProductTypeService(IProductTypeRepository productTypeRepository) : BaseService<DO.ProductType>(productTypeRepository), IProductTypeService
{
    private readonly IProductTypeRepository _productTypeRepository = productTypeRepository;

    public async Task<DO.ProductType> GetById(Guid id, bool include, bool tracking)
    {
        var entity = await _productTypeRepository.GetById(id, include, tracking);

        if (entity == null)
            throw new ValidationException("O tipo de produto não existe.");

        return entity;
    }

    public override async Task<DO.ProductType> Add(DO.ProductType entity)
    {
        var product = await _productTypeRepository.GetById(entity.Id);

        if (product != null)
            throw new ValidationException("O tipo de produto já existe.");
        
        return await base.Add(entity);
    }

    public override async Task<DO.ProductType> Update(DO.ProductType entity)
    {
        return await base.Update(entity);
    }

    public async Task Remove(Guid id)
    {
        var entity = await _productTypeRepository.GetById(id, include: false, tracking: true);
        if (entity == null)
            throw new Exception("O tipo de produto não existe.");

        await base.Remove(entity);
    }
}