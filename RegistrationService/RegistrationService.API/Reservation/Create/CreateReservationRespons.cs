namespace RegistrationService.API.Reservation.Create
{
    public class CreateReservationRespons(Guid id, Guid userId, List<Guid> seatId, Guid projectionId)
    {
        public Guid Id { get; set; } = id;
        public Guid UserId { get; set; } = userId;
        public List<Guid> SeatId { get; set; } = seatId;
        public Guid ProjectionId { get; set; } = projectionId;
    }
}