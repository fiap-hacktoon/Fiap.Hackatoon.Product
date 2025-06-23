using DO = Fiap.Hackatoon.Product.Domain.Entities;

namespace Fiap.Hackatoon.Product.Domain.Interfaces;

public interface IUserService : IBaseService<DO.User>
{
    Task Remove(Guid id);
}