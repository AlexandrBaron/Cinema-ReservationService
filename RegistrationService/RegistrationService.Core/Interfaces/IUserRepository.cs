using User = RegistrationService.Core.Entities.User;

namespace RegistrationService.Core.Interfaces
{
    public interface IUserRepository : IRepository<User>
    {
    }
}
