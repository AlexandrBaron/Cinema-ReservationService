using RegistrationService.UseCases.Users.GetById;

namespace RegistrationService.API.Users.GetById
{
    public class GetUserById(IMediator mediator)
        : Endpoint<GetUserByIdRequest, GetUserByIdResponse>
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
                Response = new GetUserByIdResponse(content.Id, content.Email, content.PhoneNumber);
                return;
            }

        }
    }
}
