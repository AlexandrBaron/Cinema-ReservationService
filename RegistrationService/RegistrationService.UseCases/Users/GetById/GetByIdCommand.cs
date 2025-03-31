namespace RegistrationService.UseCases.Users.GetById
{
    public record GetByIdCommand(Guid Id) : ICommand<Result<User>>
    {

    }
}
