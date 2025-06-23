using System.ComponentModel.DataAnnotations;
using DO = Fiap.Hackatoon.Product.Domain.Entities;
using Fiap.Hackatoon.Product.Domain.Interfaces;
using Fiap.Hackatoon.Product.Domain.Interfaces.Infrastructure;

namespace Fiap.Hackatoon.Product.Domain.Services;

public class OrderStatusService(IOrderStatusRepository orderStatusRepository) : BaseService<DO.OrderStatus>(orderStatusRepository), IOrderStatusService
{
    private readonly IOrderStatusRepository _orderStatusRepository = orderStatusRepository;

    public async Task<DO.OrderStatus> GetById(Guid id, bool include, bool tracking)
    {
        var entity = await _orderStatusRepository.GetById(id, include, tracking);

        if (entity == null)
            throw new ValidationException("O status do pedido não existe.");

        return entity;
    }

    public override async Task<DO.OrderStatus> Add(DO.OrderStatus entity)
    {
        var product = await _orderStatusRepository.GetById(entity.Id);

        if (product != null)
            throw new ValidationException("O status do pedido já existe.");
        
        return await base.Add(entity);
    }

    public override async Task<DO.OrderStatus> Update(DO.OrderStatus entity)
    {
        return await base.Update(entity);
    }

    public async Task Remove(Guid id)
    {
        var entity = await _orderStatusRepository.GetById(id, false, true);
        if (entity == null)
            throw new Exception("O status do pedido não existe.");

        await base.Remove(entity);
    }
}