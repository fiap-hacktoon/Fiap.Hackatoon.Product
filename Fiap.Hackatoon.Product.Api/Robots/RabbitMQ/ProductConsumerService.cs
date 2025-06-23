using Microsoft.Extensions.Options;
using Fiap.Hackatoon.Product.Domain.Settings;
using Fiap.Hackatoon.Product.Application.Interfaces;
using Fiap.Hackatoon.Product.Domain.Constants;

namespace Fiap.Hackatoon.Product.Api.Robots.RabbitMQ;

public class ProductConsumerService : BaseRabbitMQConsumerService
{
    public override string Queue => "hackatoon.product";

    public override string RoutingKey => "product.*";

    public ProductConsumerService(IServiceProvider serviceProvider, IOptions<GlobalSettings> settings, ILogger<ProductConsumerService> logger)
        : base(serviceProvider, settings, logger)
    {
        ServiceMaps.Add(AppConstants.Routes.RabbitMQ.ProductInsert, sp => serviceProvider.GetRequiredService<IProductApplicationService>());
        ServiceMaps.Add(AppConstants.Routes.RabbitMQ.ProductUpdate, sp => serviceProvider.GetRequiredService<IProductApplicationService>());
        ServiceMaps.Add(AppConstants.Routes.RabbitMQ.ProductDelete, sp => serviceProvider.GetRequiredService<IProductApplicationService>());
    }
}