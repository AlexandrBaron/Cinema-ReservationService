namespace RegistrationService.UseCases.Reservations.GetById
{
    public record GetByICommand(Guid Id) : ICommand<Result<Reservation>>
    {

    }
}
