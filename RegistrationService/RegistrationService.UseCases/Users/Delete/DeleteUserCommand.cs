namespace RegistrationService.UseCases.Users.Delete;
public record DeleteUserCommand(Guid Id) : ICommand<Result<string>>
{
}