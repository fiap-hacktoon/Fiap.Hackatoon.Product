using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using Fiap.Hackatoon.Product.Domain.Entities;
using Fiap.Hackatoon.Product.Domain.Enums;

namespace Fiap.Hackatoon.Product.Infrastructure.Helper;

public class TokenHelper
{
    public static TimeSpan? GetTimeUntilExpiration(string token, string jwtKey)
    {
        token = GetToken(token);
        var key = new SymmetricSecurityKey(Convert.FromBase64String(jwtKey));
        var handler = new JwtSecurityTokenHandler();
        var validationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = key,
            ValidateIssuer = false,
            ValidateAudience = false,
            ClockSkew = TimeSpan.Zero
        };

        try
        {
            var principal = handler.ValidateToken(token, validationParameters, out SecurityToken validatedToken);

            if (validatedToken is JwtSecurityToken jwtToken)
            {
                var expiration = jwtToken.ValidTo;
                return expiration - DateTime.UtcNow;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Token validation error: {ex.Message}");
        }

        return null;
    }

    public static User GetByToken(string token, string jwtKey)
    {
        token = GetToken(token);
        var key = new SymmetricSecurityKey(Convert.FromBase64String(jwtKey));
        var handler = new JwtSecurityTokenHandler();
        var validationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = key,
            ValidateIssuer = false,
            ValidateAudience = false,
            ClockSkew = TimeSpan.Zero
        };

        try
        {
            var principal = handler.ValidateToken(token, validationParameters, out SecurityToken validatedToken);

            if (validatedToken is JwtSecurityToken jwtToken)
            {
                var userData = new User
                {
                    Id = Guid.Parse(principal.FindFirst(ClaimTypes.NameIdentifier).Value),
                    Name = principal.FindFirst(ClaimTypes.Name).Value,
                    Email = principal.FindFirst(ClaimTypes.Email).Value,
                    TypeRole = (TypeRole)Enum.Parse(typeof(TypeRole), principal.FindFirst("TypeRole").Value)
                };
                return userData;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Token validation error: {ex.Message}");
        }

        return null;
    }

    private static string GetToken(string token)
    {
        if (token.StartsWith("Bearer "))
            token = token.Substring("Bearer ".Length).Trim();

        return token;
    }
}