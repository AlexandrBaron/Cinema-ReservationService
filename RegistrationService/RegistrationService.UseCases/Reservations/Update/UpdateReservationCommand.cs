using ReservationStatus = Cinema.Common.Entities.ReservationService.ReservationStatus;
namespace RegistrationService.UseCases.Registrations.Update;
public record UpdateReservationCommand(Guid Id, ReservationStatus reservationStatus) : ICommand<Result>;


