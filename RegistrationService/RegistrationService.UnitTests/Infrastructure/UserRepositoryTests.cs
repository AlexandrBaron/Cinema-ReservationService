using Microsoft.EntityFrameworkCore;
using RegistrationService.Core.Entities;
using RegistrationService.Infrastructure.Data;
using RegistrationService.Infrastructure.Repositories;

namespace RegistrationService.UnitTests.Infrastructure
{
    public class UserRepositoryTests
    {
        private readonly AppDbContext _dbContext;
        private readonly UserRepository repository;
        private readonly List<Guid> _userIds;

        public UserRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _dbContext = new AppDbContext(options);
            repository = new UserRepository(_dbContext);

            _userIds = new List<Guid>();

            SeedDatabase();
        }

        private void SeedDatabase()
        {
            for (var i = 1; i <= 10; i++)
            {
                var insertedUser = _dbContext.Set<User>().Add(new User($"user{i}@email.com", 111));
                _dbContext.SaveChanges();
                _userIds.Add(insertedUser.Entity.Id);
            }
        }
        [Fact]
        public async Task GetUserByEmail_ShoulReturnCorrectUser()
        {
            for (var i = 1; i <= 10; i++)
            {
                // Act
                var result = await repository.GetUserByEmail($"user{i}@email.com");

                // Assert
                Assert.Equal(_userIds[i - 1], result.Id);
            }
        }

        [Fact]
        public async Task GetUserByNonRegisteredEmail_ShouldReturnNull()
        {
            for (var i = 1; i <= 10; i++)
            {
                // Act
                var result = await repository.GetUserByEmail($"testUser@email.com");

                // Assert
                Assert.Equal(result, null);
            }
        }
        [Fact]
        public void UserExistsByEmail_ShouldReturnTrue()
        {
            for (var i = 1; i <= 20; i++)
            {
                var result = repository.ExistsWithEmail($"user{i}@email.com");
                if (i <= 10)
                {
                    Assert.True(result);
                }
                else
                {
                    Assert.False(result);
                }
            }
        }
    }
}