using RegistrationService.Infrastructure.Repositories;
using RegistrationService.Infrastructure.TokenService;


namespace RegistrationService.API.Reservation.Update
{
    public class Confirm(IMediator mediator, TokenService tokenService, ReservationRepository reservationRepository)
        : Endpoint<ConfirmReservationRequest>
    {

        public override void Configure()
        {
            Put("/Reservation/Confirm");
            AllowAnonymous();
        }
        public override async Task HandleAsync(ConfirmReservationRequest req, CancellationToken ct)
        {
            var reservationId = tokenService.ValidateToken(req.tokenQuery.Token);
            if (reservationId == null)
            {
                await SendAsync(new { Error = "Invalid or expired Token" }, 400, ct);
                return;
            }

            var reservation = await reservationRepository.GetByIdAsync(reservationId.Value);
            if (reservation == null)
            {
                await SendNotFoundAsync(ct);
                return;
            }

            if (reservation.ReservationStatus == ReservationStatus.Confirmed)
            {
                await SendAsync(new { Message = " Reservation is Already confirmed" }, 200, ct);
                return;
            }

            reservation.ReservationStatus = ReservationStatus.Confirmed;
            await reservationRepository.UpdateAsync(reservation);

            await SendNoContentAsync(ct);

        }
    }
}
