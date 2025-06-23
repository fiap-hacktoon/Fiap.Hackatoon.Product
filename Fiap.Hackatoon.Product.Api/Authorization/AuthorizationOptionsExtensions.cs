using Fiap.Hackatoon.Product.Domain.Enums;
using Microsoft.AspNetCore.Authorization;

namespace Fiap.Hackatoon.Product.Api.Authorization;

public static class AuthorizationOptionsExtensions
{
    public static AuthorizationOptions AddPolicyWithPermission(this AuthorizationOptions options, string policyName, TypeRole permission)
    {
        options.AddPolicy(policyName, policy => policy.Requirements.Add(new RolesRequirement(permission)));
        return options;
    }
}