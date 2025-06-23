using DO = Fiap.Hackatoon.Product.Domain.Entities;

namespace Fiap.Hackatoon.Product.Domain.Interfaces;

public interface IProductTypeService : IBaseService<DO.ProductType>
{
    Task Remove(Guid id);
}