using Reservation = RegistrationService.Core.Entities.Reservation;

namespace RegistrationService.Core.Interfaces
{
    public interface IReservationRepository : IRepository<Reservation>
    {
    }
}
