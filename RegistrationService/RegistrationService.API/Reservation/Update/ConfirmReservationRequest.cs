namespace RegistrationService.API.Reservation.Update
{
    public class ConfirmReservationRequest : UpdateReservationRequestBase
    {
        [FromQuery]
        public TokenQuery tokenQuery { get; set; }
    }

    public class TokenQuery
    {
        [FromQuery]
        public string Token { get; set; }
        public const string Route = "/Reservation/Confirm";
    }
}