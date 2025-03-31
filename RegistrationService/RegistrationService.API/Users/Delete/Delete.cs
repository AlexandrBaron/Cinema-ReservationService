
using RegistrationService.UseCases.Users.Delete;

namespace RegistrationService.API.Users.Delete
{
    public class Delete(IMediator mediator)
        : Endpoint<DeleteUserRequest>
    {
        public override void Configure()
        {
            Delete(DeleteUserRequest.Route);
            AllowAnonymous();
        }
        public override async Task HandleAsync(DeleteUserRequest req, CancellationToken ct)
        {
            var command = new DeleteUserCommand(req.Id);
            var result = await mediator.Send(command, ct);

            if (result.IsSuccess)
            {
                await SendNoContentAsync(ct);
            }
        }
    }
}
