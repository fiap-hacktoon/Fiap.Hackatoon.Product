using Fiap.Hackatoon.Product.Domain.Settings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Prometheus;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.Globalization;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json.Converters;
using System.Reflection;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using Fiap.Hackatoon.Product.Api.Authorization;
using Fiap.Hackatoon.Product.Domain.Constants;
using Fiap.Hackatoon.Product.Domain.Enums;
using Fiap.Hackatoon.Product.Api.Filters;
using Fiap.Hackatoon.Product.Application.DataTransferObjects;
using Fiap.Hackatoon.Product.Api.Logging;
using Fiap.Hackatoon.Product.Domain.Interfaces.Infrastructure;
using Fiap.Hackatoon.Product.Domain.Interfaces;
using Fiap.Hackatoon.Product.Domain.Services;
using Fiap.Hackatoon.Product.Domain.Interfaces.Security;
using Fiap.Hackatoon.Product.Domain.Services.Security;
using Fiap.Hackatoon.Product.Application.Interfaces;
using Fiap.Hackatoon.Product.Application.Services;
using Fiap.Hackatoon.Product.Infrastructure.Data;
using Fiap.Hackatoon.Product.Infrastructure.Data.Repositories;
using Fiap.Hackatoon.Product.Infrastructure.External;
using Fiap.Hackatoon.Product.Domain.Interfaces.RabbitMQ;
using Fiap.Hackatoon.Product.Domain.Interfaces.ElasticSearch;
using Fiap.Hackatoon.Product.Infrastructure.ElasticSearch.Services;

var builder = WebApplication.CreateBuilder(args);
var env = builder.Environment;

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();

builder.Configuration
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
    .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables();

var jwtKeyConfig = builder.Configuration["Token:Key"];
if (string.IsNullOrEmpty(jwtKeyConfig))
    throw new InvalidOperationException("Token:Key configuration is missing or empty.");

builder.Services.Configure<GlobalSettings>(builder.Configuration);
builder.Services.AddHealthChecks().ForwardToPrometheus();
// builder.WebHost.UseUrls("https://0.0.0.0:5055");

builder.Services.AddAuthentication(o =>
{
    o.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, o =>
{
    o.RequireHttpsMetadata = false;
    o.SaveToken = true;
    o.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Convert.FromBase64String(jwtKeyConfig)),
        RequireExpirationTime = true,
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true
    };
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicyWithPermission(AppConstants.Policies.Administrator, TypeRole.Administrator)
            .AddPolicyWithPermission(AppConstants.Policies.Attendant, TypeRole.Attendant)
            .AddPolicyWithPermission(AppConstants.Policies.Client, TypeRole.Client)
            .AddPolicyWithPermission(AppConstants.Policies.Kitchen, TypeRole.Kitchen)
            .AddPolicyWithPermission(AppConstants.Policies.Manager, TypeRole.Manager);
}).AddAuthorizationBuilder();

builder.Services.AddControllers(options => options.Filters.Add<UserFilter>()).AddNewtonsoftJson(options =>
{
    var settings = options.SerializerSettings;
    settings.NullValueHandling = NullValueHandling.Ignore;
    settings.FloatFormatHandling = FloatFormatHandling.DefaultValue;
    settings.FloatParseHandling = FloatParseHandling.Double;
    settings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
    settings.DateFormatString = "yyyy-MM-ddTHH:mm:ss";
    settings.Culture = new CultureInfo("en-US");
    settings.Converters.Add(new StringEnumConverter());
    settings.ContractResolver = new DefaultContractResolver() { NamingStrategy = new SnakeCaseNamingStrategy() };
});

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Hackatoon Product API", Version = "v1" });

    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);

    c.CustomSchemaIds(type => 
    {
        var namingStrategy = new SnakeCaseNamingStrategy();
        return namingStrategy.GetPropertyName(type.Name, false);
    });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization Header - utilizado com Bearer Authentication. \r\n\r\n Insira 'Bearer' [espaço] e então seu token na caixa abaixo.\r\n\r\nExemplo: (informar sem as aspas): 'Bearer 1234sdfgsdf' ",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

builder.Services.AddAutoMapper((sp, cfg) =>
{
    cfg.AllowNullDestinationValues = true;
    cfg.AllowNullCollections = true;
    cfg.ConstructServicesUsing(sp.GetService);
}, Assembly.GetAssembly(typeof(BaseModel)));

builder.Logging.ClearProviders();
builder.Logging.AddProvider(new CustomLoggerProvider(new CustomLoggerProviderConfiguration
{
    LogLevel = LogLevel.Information
}));

builder.Services.AddDbContext<ApplicationDBContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("SQLConnection"));
    options.LogTo(message => Debug.WriteLine(message), LogLevel.Information);
    options.EnableSensitiveDataLogging();
});

builder.Services.AddMemoryCache();

#region Repositories

builder.Services.AddScoped<IProductRepository, ProductRepository>();

#endregion

#region Services

builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<ITokenService, TokenService>();

#endregion

#region Application Services

builder.Services.AddScoped<IProductApplicationService, ProductApplicationService>();

#endregion

#region Authorization

builder.Services.AddSingleton<IAuthorizationHandler, RolesAuthorizationHandler>();

#endregion

#region Filters

builder.Services.AddScoped<IAuthorizationFilter, UserFilter>();

#endregion

#region RabbitMQProducer

builder.Services.Configure<RabbitMQOptions>(
    builder.Configuration.GetSection("RabbitMQ"));

builder.Services.AddSingleton<IRabbitMQPublisher, RabbitMQPublisher>();

#endregion

#region ElasticSearch

builder.Services.Configure<ElasticSearchSettings>(
    builder.Configuration.GetSection("ElasticSearch")
);

builder.Services.AddScoped(typeof(IElasticSearchService<>), typeof(ElasticSearchService<>));
builder.Services.AddScoped<IProductElasticSearchService, ProductElasticSearchService>();

#endregion

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI(c => 
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "FIAP Hackatoon Product API");
    c.RoutePrefix = "swagger";
});

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<ApplicationDBContext>();

    context.Database.Migrate();
}

app.UseHealthChecks("/health");
app.UseHttpMetrics();
app.MapMetrics();

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
