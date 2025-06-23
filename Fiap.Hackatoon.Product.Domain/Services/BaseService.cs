using System.Linq.Expressions;
using Fiap.Hackatoon.Product.Domain.Entities.Interfaces;
using Fiap.Hackatoon.Product.Domain.Interfaces;
using Fiap.Hackatoon.Product.Domain.Interfaces.Infrastructure;

namespace Fiap.Hackatoon.Product.Domain.Services;

public abstract class BaseService<T>(IRepository<T> repository) : IBaseService<T> where T : class, IBaseEntity
{
    protected readonly IRepository<T> _repository = repository;

    public virtual async Task<T> Add(T entity)
    {
        entity.PrepareToInsert();
        return await _repository.Add(entity);
    }

    public async Task Delete(Guid id)
    {
        await _repository.Delete(id);
    }

    public async Task Remove(T entity)
    {
        entity.PrepareToRemove();
        await _repository.Update(entity);
    }

    public IEnumerable<T> GetAll()
    {
        return _repository.GetAll();
    }

    public virtual async Task<T> Update(T entity)
    {
        entity.PrepareToUpdate();
        return await _repository.Update(entity);
    }

    public IQueryable<T> FindBy(Expression<Func<T, bool>> expression)
    {
        return _repository.FindBy(expression);
    }

    public void Dispose()
    {
        _repository.Dispose();

        GC.SuppressFinalize(this);
    }
}