namespace RegistrationService.API.Reservation.GetById
{
    public class GetReservationByIdReservationResponse(Guid id, Guid userId, List<Guid> seatId, Guid projectionId, ReservationStatus reservationStatus, DateTime timeOfCreation)
    {
        public Guid Id { get; set; } = id;
        public Guid UserId { get; set; } = userId;
        public List<Guid> SeatId { get; set; } = seatId;
        public Guid ProjectionId { get; set; } = projectionId;
        public ReservationStatus ReservationStatus { get; set; } = reservationStatus;
        public DateTime TimeOfCreation { get; set; } = timeOfCreation;
    }
}