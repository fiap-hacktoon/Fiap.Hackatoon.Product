namespace Fiap.Hackatoon.Product.Infrastructure.External;

public class RabbitMQOptions
{
    public string HostName { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string VirtualHost { get; set; } = "/";
    public int Port { get; set; } = 5672;
    public string DefaultExchangeName { get; set; } = string.Empty;
    public string DefaultExchangeType { get; set; } = string.Empty;
}
