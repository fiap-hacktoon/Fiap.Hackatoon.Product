using System.ComponentModel.DataAnnotations;
using DO = Fiap.Hackatoon.Product.Domain.Entities;
using Fiap.Hackatoon.Product.Domain.Interfaces;
using Fiap.Hackatoon.Product.Domain.Interfaces.Infrastructure;

namespace Fiap.Hackatoon.Product.Domain.Services;

public class OrderProductService(IOrderProductRepository orderProductRepository) : BaseService<DO.OrderProduct>(orderProductRepository), IOrderProductService
{
    private readonly IOrderProductRepository _orderProductRepository = orderProductRepository;

    public async Task<DO.OrderProduct> GetById(Guid id, bool include, bool tracking)
    {
        var entity = await _orderProductRepository.GetById(id, include, tracking);

        if (entity == null)
            throw new ValidationException("O pedido do produto não existe.");

        return entity;
    }

    public override async Task<DO.OrderProduct> Add(DO.OrderProduct entity)
    {
        var product = await _orderProductRepository.GetById(entity.Id);

        if (product != null)
            throw new ValidationException("O pedido do produto já existe.");
        
        return await base.Add(entity);
    }

    public override async Task<DO.OrderProduct> Update(DO.OrderProduct entity)
    {
        return await base.Update(entity);
    }

    public async Task Remove(Guid id)
    {
        var entity = await _orderProductRepository.GetById(id, false, true);
        if (entity == null)
            throw new Exception("O pedido do produto não existe.");

        await base.Remove(entity);
    }
}