namespace RegistrationService.API.Reservation.GetOccupiedSeatsProjectionId
{
    public class GetOccupiedSeatsProjectionIdResponse(List<Guid> occupiedSeats)
    {
        public List<Guid> OccupiedSeats { get; set; } = occupiedSeats;
    }
}