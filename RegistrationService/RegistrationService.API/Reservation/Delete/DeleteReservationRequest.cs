namespace RegistrationService.API.Reservation.Delete
{
    public class DeleteReservationRequest
    {
        public const string Route = "/Reservation/Delete{Id:Guid}";
        public static string BuildRoute(Guid Id) => Route.Replace("{Id:Guid}", Id.ToString());
        public Guid Id { get; set; }
    }
}