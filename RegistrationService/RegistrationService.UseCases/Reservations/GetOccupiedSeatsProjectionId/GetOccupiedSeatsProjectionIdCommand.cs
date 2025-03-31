namespace RegistrationService.UseCases.Reservations.GetOccupiedSeatsProjectionId
{
    public record GetOccupiedSeatsProjectionIdCommand(Guid projectionId) : ICommand<Result<List<Guid>>>
    {
    }
}
