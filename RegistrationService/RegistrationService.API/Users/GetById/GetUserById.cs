using RegistrationService.UseCases.Users.GetById;

namespace RegistrationService.API.Users.GetById
{
    public class GetUserById(IMediator mediator)
        : Endpoint<GetUserByIdRequest, User>
    {
        public override void Configure()
        {
            Get(GetUserByIdRequest.Route);
            AllowAnonymous();
        }
        public override async Task HandleAsync(GetUserByIdRequest req, CancellationToken ct)
        {
            var result = await mediator.Send(new GetByIdCommand(req.Id), ct);
            var content = result.Value;

            if (result.IsSuccess)
            {
                Response = new User { Id = content.Id, Email = content.Email, PhoneNumber = content.PhoneNumber };
                return;
            }

        }
    }
}
