namespace RegistrationService.UseCases.Users.GetByEmail
{
    public record GetByEmailCommand(string email) : ICommand<Result<Guid>>
    {

    }
}
