using DTO = Fiap.Hackatoon.Product.Application.DataTransferObjects;
using DTOE = Fiap.Hackatoon.Product.Application.DataTransferObjects.ElasticSearch;

namespace Fiap.Hackatoon.Product.Application.Interfaces;

public interface IProductApplicationService : IDisposable
{   
    Task<DTO.Product> GetById(Guid id);
    Task<List<DTOE.ProductByType>> GetByType(string nameOrCode, int page = 0, int size = 20);
    Task Add(DTO.Product model);
    Task Update(DTO.Product model);
    Task Delete(Guid id);
    Task InsertElasticSearch(string nameOrCode);
}