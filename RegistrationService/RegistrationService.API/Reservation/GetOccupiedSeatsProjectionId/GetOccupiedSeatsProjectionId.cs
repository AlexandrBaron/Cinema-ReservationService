using RegistrationService.UseCases.Reservations.GetOccupiedSeatsProjectionId;

namespace RegistrationService.API.Reservation.GetOccupiedSeatsProjectionId
{
    public class GetOccupiedSeatsProjectionId(IMediator mediator)
        : Endpoint<GetOccupiedSeatsProjectionIdRequest, List<Guid>>
    {
        public override void Configure()
        {
            Get(GetOccupiedSeatsProjectionIdRequest.Route);
            AllowAnonymous();
        }
        public override async Task HandleAsync(GetOccupiedSeatsProjectionIdRequest req, CancellationToken ct)
        {
            var result = await mediator.Send(new GetOccupiedSeatsProjectionIdCommand(req.Id), ct);

            if (result.IsSuccess)
            {
                Response = result;
            }
        }
    }
}
