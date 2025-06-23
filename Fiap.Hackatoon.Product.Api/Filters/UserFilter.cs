using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;
using Fiap.Hackatoon.Product.Application.Interfaces;
using Fiap.Hackatoon.Product.Application.Services;
using Fiap.Hackatoon.Product.Domain.Entities;
using Fiap.Hackatoon.Product.Domain.Settings;
using Fiap.Hackatoon.Product.Application.DataTransferObjects;
using Fiap.Hackatoon.Product.Infrastructure.Helper;


namespace Fiap.Hackatoon.Product.Api.Filters;

public class UserFilter(IOptions<TokenSettings> options) : IAuthorizationFilter
{
    private readonly TokenSettings _settings = options.Value;

    public async void OnAuthorization(AuthorizationFilterContext context)
    {
        if (context.Filters.Any(f => f is SkipUserFilterAttribute))
            return;
        
        var user = context.HttpContext.User;
        var userId = user?.Claims?.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(userId))
        {
            context.Result = new UnauthorizedResult();
            return;
        }

        var token = context.HttpContext.Request.Headers.Authorization.FirstOrDefault();
        if (token != null)
        {
            var userLogin = new UserLogin
            {
                Id = Guid.Parse(userId),
                Email = user?.Claims?.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value,
                Password = user?.Claims?.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value
            };
            
            var timeUntilExpiration = TokenHelper.GetTimeUntilExpiration(token, _settings.Key);

            if (timeUntilExpiration.HasValue && timeUntilExpiration.Value.TotalMinutes <= 0)
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            if (context.HttpContext.User?.Claims?.Count() <= 0)
            {
                context.Result = new UnauthorizedResult();
                return;
            }
        }
        else
        {
            context.Result = new UnauthorizedResult();
            return;
        }
    }
}
