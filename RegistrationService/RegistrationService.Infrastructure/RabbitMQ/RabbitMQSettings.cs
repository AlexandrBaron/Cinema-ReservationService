namespace RegistrationService.Infrastructure.RabbitMQ
{
    public class RabbitMQSettings
    {
        public required string HostName { get; set; }
        public required string NotificationQueueName { get; set; }
        public required string NotificationExchangeName { get; set; }
    }
}
