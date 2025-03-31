using RegistrationService.UseCases.Reservations.Delete;

namespace RegistrationService.API.Reservation.Delete
{
    public class Delete(IMediator mediator)
        : Endpoint<DeleteReservationRequest>
    {
        public override void Configure()
        {
            Delete(DeleteReservationRequest.Route);
            AllowAnonymous();
        }
        public override async Task HandleAsync(DeleteReservationRequest req, CancellationToken ct)
        {
            var command = new DeleteReservationCommand(req.Id);
            var result = await mediator.Send(command, ct);

            if (result.IsSuccess)
            {
                await SendNoContentAsync(ct);
            }
        }
    }
}
