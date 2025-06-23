using Microsoft.AspNetCore.Mvc.Filters;

namespace Fiap.Hackatoon.Product.Api.Filters;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class SkipUserFilterAttribute : Attribute, IFilterMetadata { }
