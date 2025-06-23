using System.ComponentModel.DataAnnotations;
using DO = Fiap.Hackatoon.Product.Domain.Entities;
using Fiap.Hackatoon.Product.Domain.Interfaces;
using Fiap.Hackatoon.Product.Domain.Interfaces.Infrastructure;

namespace Fiap.Hackatoon.Product.Domain.Services;

public class OrderService(IOrderRepository orderRepository) : BaseService<DO.Order>(orderRepository), IOrderService
{
    private readonly IOrderRepository _orderRepository = orderRepository;

    public async Task<DO.Order> GetById(Guid id, bool include, bool tracking)
    {
        var entity = await _orderRepository.GetById(id, include, tracking);

        if (entity == null)
            throw new ValidationException("O pedido não existe.");

        return entity;
    }

    public override async Task<DO.Order> Add(DO.Order entity)
    {
        var product = await _orderRepository.GetById(entity.Id);

        if (product != null)
            throw new ValidationException("O pedido já existe.");
        
        return await base.Add(entity);
    }

    public override async Task<DO.Order> Update(DO.Order entity)
    {
        return await base.Update(entity);
    }

    public async Task Remove(Guid id)
    {
        var entity = await _orderRepository.GetById(id, false, true);
        if (entity == null)
            throw new Exception("O pedido não existe.");

        await base.Remove(entity);
    }
}