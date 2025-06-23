using Fiap.Hackatoon.Product.Application.Interfaces.RabbitMQ;
using DTO = Fiap.Hackatoon.Product.Application.DataTransferObjects;

namespace Fiap.Hackatoon.Product.Application.Interfaces;

public interface IProductApplicationService : IBaseRabbitMQConsumer
{   
    Task<DTO.Product> GetById(Guid id);
    Task<DTO.Product> Add(DTO.Product model);
    Task<DTO.Product> Update(DTO.Product model);
    Task<bool> Delete(Guid id);
}