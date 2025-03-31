namespace RegistrationService.Infrastructure.RabbitMQ
{
    public class RabbitMQMessageModel
    {
        public Guid Id = Guid.NewGuid();
        public Guid ReservationId { get; set; }
        public required string Message { get; set; }
        public string ConfirmationLink { get; set; }

        public DateTime CreatedAt = DateTime.Now;
    }
}
