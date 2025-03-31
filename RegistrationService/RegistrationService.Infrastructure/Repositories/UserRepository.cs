
namespace RegistrationService.Infrastructure.Repositories
{
    public class UserRepository(AppDbContext dbContext) : EfRepository<User>(dbContext), IUserRepository
    {
        private readonly AppDbContext _dbContext = dbContext;

        public bool ExistsWithEmail(string email)
        {
            return _dbContext.Set<User>().Any(x => x.Email == email);
        }

        public async Task<User?> GetUserByEmail(string email)
        {
            return _dbContext.Set<User>().Where(x => x.Email == email).SingleOrDefault();
        }
    }
}
