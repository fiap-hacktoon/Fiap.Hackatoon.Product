using Fiap.Hackatoon.Product.Domain.Enums;
using Microsoft.AspNetCore.Authorization;

namespace Fiap.Hackatoon.Product.Api.Authorization;

public class RolesRequirement(TypeRole permission) : IAuthorizationRequirement
{
    public TypeRole Permission { get; } = permission;
}