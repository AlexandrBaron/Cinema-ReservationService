namespace RegistrationService.UseCases.Reservations.Create;
public record CreateReservationCommand(Guid UserId, List<Guid> SeatIds, Guid ProjectionId) : ICommand<Result<Guid>>;


