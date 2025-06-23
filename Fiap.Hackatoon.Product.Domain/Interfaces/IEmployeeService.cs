using DO = Fiap.Hackatoon.Product.Domain.Entities;

namespace Fiap.Hackatoon.Product.Domain.Interfaces;

public interface IEmployeeService : IBaseService<DO.Employee>
{
    Task Remove(Guid id);
}