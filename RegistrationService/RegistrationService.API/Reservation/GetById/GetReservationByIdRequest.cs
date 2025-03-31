namespace RegistrationService.API.Reservation.GetById
{
    public class GetReservationByIdRequest
    {
        public const string Route = "/Reservation/GetById/{Id:Guid}";
        public static string BuildRoute(Guid Id) => Route.Replace("{Id}", Id.ToString());
        public required Guid Id { get; set; }
    }
}