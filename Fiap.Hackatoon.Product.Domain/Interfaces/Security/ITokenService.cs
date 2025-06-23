using Fiap.Hackatoon.Product.Domain.Entities;

namespace Fiap.Hackatoon.Product.Domain.Interfaces.Security;

public interface ITokenService
{
    string GenerateToken(User user, bool force = false);
}