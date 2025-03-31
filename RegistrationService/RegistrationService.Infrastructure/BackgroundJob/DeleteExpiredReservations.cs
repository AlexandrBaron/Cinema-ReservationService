using RegistrationService.Infrastructure.Repositories;

namespace RegistrationService.Infrastructure.BackgroundJob
{
    public class DeleteExpiredReservations(ReservationRepository repository)
    {
        private readonly ReservationRepository _repository = repository;

        public async Task ExecuteAsync(TimeSpan expirationTime)
        {
            var expiredReservations = await _repository.GetReservationsOlderThanAsync(expirationTime);

            foreach (var reservation in expiredReservations)
            {
                await _repository.DeleteAsync(reservation);
            }
        }
    }
}
