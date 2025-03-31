namespace RegistrationService.UseCases.Reservations.Delete;
public record DeleteReservationCommand(Guid Id) : ICommand<Result<string>>;
