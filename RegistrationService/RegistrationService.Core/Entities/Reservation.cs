using Cinema.Common.Entities.ReservationService;
namespace RegistrationService.Core.Entities
{
    public class Reservation : BaseEntity
    {
        public Guid UserId { get; set; }

        public List<Guid> SeatId { get; set; } = [];

        public Guid ProjectionId { get; set; }

        public ReservationStatus ReservationStatus { get; set; }

        public DateTime TimeOfCreation { get; set; }

        public Reservation()
        {

        }
        public Reservation(Guid userId, List<Guid> seatId, Guid projectionId, ReservationStatus reservationStatus)
        {
            UserId = userId;
            SeatId = seatId;
            ProjectionId = projectionId;
            ReservationStatus = reservationStatus;
            TimeOfCreation = DateTime.Now;
        }
        public void UpdateReservation(Guid userId, List<Guid> seatId, Guid projectionId, ReservationStatus reservationStatus)
        {
            UserId = Guard.Against.NullOrEmpty(userId, nameof(userId));
            SeatId = Guard.Against.Null(seatId, nameof(seatId));
            ProjectionId = Guard.Against.NullOrEmpty(projectionId, nameof(projectionId));
            ReservationStatus = (ReservationStatus)Guard.Against.OutOfRange((int)reservationStatus, nameof(reservationStatus), 0, 1, "Invalid");
        }
    }
}
