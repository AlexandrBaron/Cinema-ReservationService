using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;

namespace RegistrationService.Infrastructure.RabbitMQ
{
    public class RabbitMQManager
    {
        private readonly ConnectionFactory _facory;
        private readonly string _notificationQueueName;
        private readonly string _notificationExchangeName;
        private readonly ILogger<RabbitMQManager> _logger;

        public RabbitMQManager(string url, string notificationQueueName, string notificationExchangeName, ILogger<RabbitMQManager> logger)
        {
            _facory = new ConnectionFactory()
            {
                Uri = new Uri(url),
            };
            _notificationQueueName = notificationQueueName;
            _notificationExchangeName = notificationExchangeName;
            _logger = logger;
        }

        public async Task PublishMessageAsync(RabbitMQMessageModel messageModel)
        {

            try
            {
                using var connection = await _facory.CreateConnectionAsync();
                using var channel = await connection.CreateChannelAsync();

                _logger.LogInformation("connectionsuccesfull");

                var routingKey = "reservation.created";
                var jsonMessage = JsonSerializer.Serialize(messageModel);

                await channel.ExchangeDeclareAsync(_notificationExchangeName, "direct", durable: true, autoDelete: false);
                await channel.QueueDeclareAsync(_notificationQueueName, durable: true, exclusive: false, autoDelete: false);
                await channel.QueueBindAsync(_notificationQueueName, _notificationExchangeName, routingKey);

                var body = Encoding.UTF8.GetBytes(jsonMessage);

                await channel.BasicPublishAsync(exchange: _notificationExchangeName, routingKey: routingKey, body: body);
                _logger.LogInformation($"Message successfully published. Massage: {jsonMessage}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error while publishing mesage: {ex.Message}");

            }
        }
    }
}
