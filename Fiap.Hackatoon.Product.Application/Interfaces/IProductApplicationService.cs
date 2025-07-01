using DTO = Fiap.Hackatoon.Product.Application.DataTransferObjects;

namespace Fiap.Hackatoon.Product.Application.Interfaces;

public interface IProductApplicationService : IDisposable
{   
    Task<DTO.Product> GetById(Guid id);
    Task<List<DTO.Product>> GetByType(string nameOrCode);
    Task Add(DTO.Product model);
    Task Update(DTO.Product model);
    Task Delete(Guid id);
}