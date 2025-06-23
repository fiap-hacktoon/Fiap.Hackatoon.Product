using DO = Fiap.Hackatoon.Product.Domain.Entities;

namespace Fiap.Hackatoon.Product.Domain.Interfaces.Infrastructure;

public interface IEmployeeRepository : IRepository<DO.Employee>
{
    Task<DO.Employee> GetById(Guid id, bool include = false, bool tracking = false);
}