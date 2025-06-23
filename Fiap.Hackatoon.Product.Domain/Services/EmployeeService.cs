using System.ComponentModel.DataAnnotations;
using DO = Fiap.Hackatoon.Product.Domain.Entities;
using Fiap.Hackatoon.Product.Domain.Interfaces;
using Fiap.Hackatoon.Product.Domain.Interfaces.Infrastructure;

namespace Fiap.Hackatoon.Product.Domain.Services;

public class EmployeeService(IEmployeeRepository employeeRepository) : BaseService<DO.Employee>(employeeRepository), IEmployeeService
{
    private readonly IEmployeeRepository _employeeRepository = employeeRepository;

    public async Task<DO.Employee> GetById(Guid id, bool include, bool tracking)
    {
        var entity = await _employeeRepository.GetById(id, include, tracking);

        if (entity == null)
            throw new ValidationException("O funcionário não existe.");

        return entity;
    }

    public override async Task<DO.Employee> Add(DO.Employee entity)
    {
        var product = await _employeeRepository.GetById(entity.Id);

        if (product != null)
            throw new ValidationException("O funcionário já existe.");
        
        return await base.Add(entity);
    }

    public override async Task<DO.Employee> Update(DO.Employee entity)
    {
        return await base.Update(entity);
    }

    public async Task Remove(Guid id)
    {
        var entity = await _employeeRepository.GetById(id, false, true);
        if (entity == null)
            throw new Exception("O funcionário não existe.");

        await base.Remove(entity);
    }
}