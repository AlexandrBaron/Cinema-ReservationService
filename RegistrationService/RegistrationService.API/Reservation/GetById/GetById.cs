using RegistrationService.UseCases.Reservations.GetById;
using ReservationEntity = RegistrationService.Core.Entities.Reservation;
namespace RegistrationService.API.Reservation.GetById
{
    public class GetById(IMediator mediator)
        : Endpoint<GetReservationByIdRequest, ReservationEntity>
    {
        public override void Configure()
        {
            Get(GetReservationByIdRequest.Route);
            AllowAnonymous();
        }
        public override async Task HandleAsync(GetReservationByIdRequest req, CancellationToken ct)
        {
            var result = await mediator.Send(new GetByICommand(req.Id), ct);

            var content = result.Value;
            Console.WriteLine("ID XXXXXXXXXXXXXXXXXXXXXXXXXXX: " + content.UserId);
            if (result.IsSuccess)
            {
                Response = content;
                return;
            }
        }
    }
}
