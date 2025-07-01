using Fiap.Hackatoon.Product.Domain.Interfaces.RabbitMQ;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace Fiap.Hackatoon.Product.Infrastructure.External;

public class RabbitMQPublisher : IRabbitMQPublisher
{
    private readonly RabbitMQOptions _options;
    private readonly IConnection _connection;

    public RabbitMQPublisher(IOptions<RabbitMQOptions> options)
    {
        _options = options.Value;
        var factory = new ConnectionFactory()
        {
            HostName = _options.HostName,
            UserName = _options.UserName,
            Password = _options.Password,
            VirtualHost = _options.VirtualHost,
            Port = _options.Port
        };

        _connection = factory.CreateConnectionAsync().Result;
    }

    public async Task PublishAsync<T>(T message, string routingKey)
    {
        await using var connection = await CreateConnectionAsync();
        await using var channel = await connection.CreateChannelAsync();

        await EnsureDeadLetterInfrastructureAsync(channel, routingKey);
        await EnsureMainExchangeAndQueueAsync(channel, routingKey);

        var json = JsonSerializer.Serialize(message);
        var body = Encoding.UTF8.GetBytes(json);

        await channel.BasicPublishAsync(
            exchange: _options.DefaultExchangeName,
            routingKey: routingKey,
            mandatory: false,
            body: body);
    }

    private async Task<IConnection> CreateConnectionAsync()
    {
        var factory = new ConnectionFactory
        {
            HostName = _options.HostName,
            UserName = _options.UserName,
            Password = _options.Password,
            VirtualHost = _options.VirtualHost,
            Port = _options.Port
        };

        return await factory.CreateConnectionAsync();
    }

    private async Task EnsureDeadLetterInfrastructureAsync(IChannel channel, string routingKey)
    {
        string dlxExchange = $"dlx.{_options.DefaultExchangeName}";
        string dlqQueue = $"dlq.queue.{routingKey}";

        await channel.ExchangeDeclareAsync(
            exchange: dlxExchange,
            type: _options.DefaultExchangeType,
            durable: true,
            autoDelete: false);

        await channel.QueueDeclareAsync(
            queue: dlqQueue,
            durable: true,
            exclusive: false,
            autoDelete: false);

        await channel.QueueBindAsync(
            queue: dlqQueue,
            exchange: dlxExchange,
            routingKey: routingKey);
    }

    private async Task EnsureMainExchangeAndQueueAsync(IChannel channel, string routingKey)
    {
        string mainExchange = _options.DefaultExchangeName;
        string mainQueue = $"queue.{routingKey}";
        string dlxExchange = $"dlx.{mainExchange}";

        await channel.ExchangeDeclareAsync(
            exchange: mainExchange,
            type: _options.DefaultExchangeType,
            durable: true,
            autoDelete: false);

        var queueArgs = new Dictionary<string, object>
        {
            { "x-dead-letter-exchange", dlxExchange },
            { "x-dead-letter-routing-key", routingKey }
        };

        await channel.QueueDeclareAsync(
            queue: mainQueue,
            durable: true,
            exclusive: false,
            autoDelete: false,
            arguments: queueArgs);

        await channel.QueueBindAsync(
            queue: mainQueue,
            exchange: mainExchange,
            routingKey: routingKey);
    }
}