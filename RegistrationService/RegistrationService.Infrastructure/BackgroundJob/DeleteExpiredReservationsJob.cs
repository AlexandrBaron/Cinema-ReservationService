using Microsoft.Extensions.Options;
using Quartz;

namespace RegistrationService.Infrastructure.BackgroundJob
{
    public class DeleteExpiredReservationsJob : IJob
    {
        private readonly DeleteExpiredReservations _deleteExpiredReservations;
        private readonly ReservationSettings _reservationSettings;

        public DeleteExpiredReservationsJob(DeleteExpiredReservations deleteExpiredReservations, IOptions<ReservationSettings> reservationSettings)
        {
            _reservationSettings = reservationSettings.Value;
            _deleteExpiredReservations = deleteExpiredReservations;
        }

        public async Task Execute(IJobExecutionContext context)
        {

            TimeSpan expirationTime = TimeSpan.FromMinutes(_reservationSettings.ExpirationTimeInMinutes);
            await _deleteExpiredReservations.ExecuteAsync(expirationTime);
        }
    }
}
