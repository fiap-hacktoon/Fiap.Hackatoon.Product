namespace Fiap.Hackatoon.Product.Domain.Constants;

public class AppConstants
{
    public static class Policies
    {
        public const string Administrator = "Administrator";
        public const string Client = "Client";
        public const string Manager = "Manager";
        public const string Attendant = "Attendant";
        public const string Kitchen = "Kitchen";
    }

    public static class Routes
    {
        public static class RabbitMQ
        {
            public const string ProductInsert = "product.insert";
            public const string ProductUpdate = "product.update";
            public const string ProductDelete = "product.delete";
        }
    }
}