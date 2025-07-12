namespace Fiap.Hackatoon.Product.Domain.Settings;

public class GlobalSettings
{
    public RabbitMQSettings RabbitMQ { get; set; }
    public ElasticSearchSettings ElasticSearch { get; set; }
    public TokenSettings Token { get; set; }
}