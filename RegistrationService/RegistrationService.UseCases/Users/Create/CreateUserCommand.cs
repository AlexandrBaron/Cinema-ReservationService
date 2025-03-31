namespace RegistrationService.UseCases.Users.Create;
public record CreateUserCommand(string Email, int PhoneNumber) : ICommand<Result<Guid>>;
