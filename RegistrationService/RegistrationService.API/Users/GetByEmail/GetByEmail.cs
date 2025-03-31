using RegistrationService.UseCases.Users.GetByEmail;

namespace RegistrationService.API.Users.GetByEmail
{
    public class GetByEmail(IMediator mediator)
        : Endpoint<GetUsersByEmailRequest, Guid>
    {
        public override void Configure()
        {
            Get(GetUsersByEmailRequest.Route);
            AllowAnonymous();
        }
        public override async Task HandleAsync(GetUsersByEmailRequest req, CancellationToken ct)
        {
            var result = await mediator.Send(new GetByEmailCommand(req.Email), ct);

            if (result.IsSuccess)
            {
                Response = result;
                return;
            }
            else
            {
                Response = Guid.Empty;
            }
        }
    }
}
