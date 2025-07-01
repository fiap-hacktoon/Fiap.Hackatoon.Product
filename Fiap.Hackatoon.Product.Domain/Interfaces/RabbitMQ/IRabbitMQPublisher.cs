namespace Fiap.Hackatoon.Product.Domain.Interfaces.RabbitMQ;

public interface IRabbitMQPublisher
{
    Task PublishAsync<T>(T message, string routingKey);
}