namespace Fiap.Hackatoon.Product.Application.Interfaces.RabbitMQ;

public interface IBaseRabbitMQConsumer : IDisposable
{
    Task Consumer(string message, string rountingKey);
}