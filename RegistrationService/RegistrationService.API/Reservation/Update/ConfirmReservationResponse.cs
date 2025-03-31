namespace RegistrationService.API.Reservation.Update
{
    public class ConfirmReservationResponse(Guid id, ReservationStatus reservationStatus)
    {
        public Guid Id { get; set; } = id;
        public ReservationStatus Rs { get; set; } = reservationStatus;
    }
}