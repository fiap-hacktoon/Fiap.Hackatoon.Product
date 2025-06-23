using System.Security.Claims;
using Fiap.Hackatoon.Product.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using Fiap.Hackatoon.Product.Infrastructure.Extensions;

namespace Fiap.Hackatoon.Product.Api.Authorization;

public class RolesAuthorizationHandler : AuthorizationHandler<RolesRequirement>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, RolesRequirement requirement)
    {
        var roleClaim = context.User.FindFirst(c => c.Type == ClaimTypes.Role)?.Value;

        if (roleClaim.IsNullOrEmpty() == false && Enum.TryParse<TypeRole>(roleClaim, out var userRoles))
        {
            if (requirement.Permission.HasAnyFlag(userRoles)) 
            {
                context.Succeed(requirement);
            }
        }
        return Task.CompletedTask;
    }
}
