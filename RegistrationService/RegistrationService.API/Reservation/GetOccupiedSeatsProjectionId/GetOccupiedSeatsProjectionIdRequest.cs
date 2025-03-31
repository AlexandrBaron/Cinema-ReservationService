namespace RegistrationService.API.Reservation.GetOccupiedSeatsProjectionId
{
    public class GetOccupiedSeatsProjectionIdRequest
    {
        public const string Route = "/Reservation/GetOccupiedSeatsByProjectionId/{Id}";
        public static string BuildRoute(Guid Id) => Route.Replace("{Id}", Id.ToString());
        public Guid Id { get; set; }

        public GetOccupiedSeatsProjectionIdRequest() { }

        public GetOccupiedSeatsProjectionIdRequest(Guid projectionId)
        {
            Id = projectionId;
        }
    }
}