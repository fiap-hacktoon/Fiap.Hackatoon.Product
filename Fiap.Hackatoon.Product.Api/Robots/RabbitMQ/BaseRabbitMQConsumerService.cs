
using System.Text;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Fiap.Hackatoon.Product.Domain.Settings;
using Fiap.Hackatoon.Product.Application.Interfaces.RabbitMQ;
using Fiap.Hackatoon.Product.Application.Interfaces;
using Fiap.Hackatoon.Product.Infrastructure.Data;

namespace Fiap.Hackatoon.Product.Api.Robots.RabbitMQ;

public abstract class BaseRabbitMQConsumerService : BackgroundService
{
    private bool _isExchangeDeclared = false;
    private readonly string _exchange;
    private readonly RabbitMQSettings _settings;

    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger _logger;

    public abstract string Queue { get; }
    public abstract string RoutingKey { get; }

    public string QueueError => $"{Queue}_error";
    public string RoutingKeyError => $"{RoutingKey}.error";

    public Dictionary<string, Func<IServiceProvider, IBaseRabbitMQConsumer>> ServiceMaps { get; private set; }

    private IConnection _connection;
    private IModel _channel;

    public BaseRabbitMQConsumerService(IServiceProvider serviceProvider, IOptions<GlobalSettings> option, ILogger logger, string exchange = null)
    {
        // Inicialização de conexão e canal, fora do método ExecuteAsync
        _settings = option?.Value?.RabbitMQ;
        _serviceProvider = serviceProvider;
        _logger = logger;
        _exchange = exchange ?? _settings.DefaultExchangeName;
        ServiceMaps = new Dictionary<string, Func<IServiceProvider, IBaseRabbitMQConsumer>>();
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var subscriberName = GetType().Name;
        _logger.LogInformation("{SubscriberName} started.", subscriberName);

        var factory = new ConnectionFactory()
        {
            HostName = _settings.HostName,
            UserName = _settings.UserName,
            Password = _settings.Password,
            VirtualHost = _settings.VirtualHost,
            Port = _settings.Port
        };

        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();

        if (!_isExchangeDeclared)
        {
            _channel.ExchangeDeclare(_exchange, _settings.DefaultExchangeType, true);
            _isExchangeDeclared = true;
        }

        _channel.QueueDeclare(Queue, true, false, false, null);
        _channel.QueueBind(Queue, _exchange, RoutingKey, null);

        var consumer = new EventingBasicConsumer(_channel);
        consumer.Received += async (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            try
            {
                using var scope = _serviceProvider.CreateScope();

                var service = scope.ServiceProvider.GetRequiredService<IProductApplicationService>();
                if (service != null)
                {
                    using var context = scope.ServiceProvider.GetService<ApplicationDBContext>();

                    await service.Consumer(message, ea.RoutingKey);

                    await context.SaveChangesAsync();

                    _logger.LogInformation("Message received: {Message}", message);
                    _channel.BasicAck(ea.DeliveryTag, false);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error processing message: {message}");
                _channel.BasicNack(ea.DeliveryTag, false, false);
                _channel.BasicPublish(_exchange, RoutingKeyError, null, Encoding.UTF8.GetBytes(message));
            }
        };

        _channel.BasicConsume(Queue, false, consumer);

        return Task.CompletedTask;
    }

    public override void Dispose()
    {
        _channel?.Close();
        _connection?.Close();
        base.Dispose();
    }
}
