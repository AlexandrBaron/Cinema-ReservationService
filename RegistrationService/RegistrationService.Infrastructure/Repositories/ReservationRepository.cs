using Microsoft.Extensions.Options;
using RegistrationService.Shared;
using ReservationStatus = Cinema.Common.Entities.ReservationService.ReservationStatus;

namespace RegistrationService.Infrastructure.Repositories
{
    public class ReservationRepository(AppDbContext dbContext, IOptions<ReservationOptions> options) : EfRepository<Reservation>(dbContext), IReservationRepository
    {
        private readonly AppDbContext _dbContext = dbContext;
        private readonly IOptions<ReservationOptions> _options = options;

        public List<Reservation> GetByProjectionId(Guid projectionId)
        {
            var expiration = _options.Value.ExpirationMinutes.Development;
            var threshold = DateTime.Now.AddMinutes(-expiration);

            var result = _dbContext.Set<Reservation>()
                .Where(r =>
                    r.ProjectionId == projectionId &&
                    (
                        r.ReservationStatus == ReservationStatus.Confirmed ||
                        (r.ReservationStatus == ReservationStatus.NotConfirmed && r.TimeOfCreation >= threshold)
                    ))
                .ToList();

            return result;
        }

        public async Task<List<Reservation>> GetReservationsOlderThanAsync(TimeSpan timeSpan)
        {
            var tresholdTime = DateTime.Now - timeSpan;
            return await _dbContext.Reservations
                .Where(r => r.TimeOfCreation < tresholdTime)
                .ToListAsync();
        }
    }
}
