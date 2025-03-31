using RegistrationService.UseCases.Users.Create;
namespace RegistrationService.API.Users.Create
{
    public class Create(IMediator mediator)
        : Endpoint<CreateUserRequest, CreateUserResponse>
    {
        public override void Configure()
        {
            Post(CreateUserRequest.Route);
            AllowAnonymous();
            Summary(s =>
            {
                s.ExampleRequest = new CreateUserRequest { Email = "New Email", PhoneNumber = 123456789 };
            });
        }

        public override async Task HandleAsync(CreateUserRequest req, CancellationToken ct)
        {

            var result = await mediator.Send(new CreateUserCommand(req.Email!, req.PhoneNumber!), ct);

            if (result.IsSuccess)
            {
                Response = new CreateUserResponse(req.Email, req.PhoneNumber);
                return;
            }
            else
            {
                ThrowError(string.Join(",", result.Errors));
            }
        }
    }
}
