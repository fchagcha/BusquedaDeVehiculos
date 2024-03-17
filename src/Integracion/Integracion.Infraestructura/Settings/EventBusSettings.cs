namespace Integracion.Infraestructura.Settings
{
    public class EventBusSettings
    {
        public string EventBusProvider { get; set; }
        public string Exchange { get; set; }
        public string Queue { get; set; }
        public string Type { get; set; }
        public string RoutingKeyPattern { get; set; }
        public RabbitMqSettings RabbitMqSettings { get; set; }
        public AmazonSqsSettings AmazonSqsSettings { get; set; }
        public AzureServiceBusSettings AzureServiceBusSettings { get; set; }
    }

    public class RabbitMqSettings
    {
        public string HostName { get; set; }
        public int Port { get; set; }
        public string UserNameRabbitMq { get; set; }
        public string Password { get; set; }
        public string VirtualHost { get; set; }
    }
    public class AmazonSqsSettings
    {
        public string Region { get; set; }
        public string AccessKey { get; set; }
        public string SecretKey { get; set; }
    }
    public class AzureServiceBusSettings
    {
        public string ConnectionString { get; set; }
    }
}
