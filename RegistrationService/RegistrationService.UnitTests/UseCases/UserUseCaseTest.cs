using Microsoft.EntityFrameworkCore;
using RegistrationService.Core.Entities;
using RegistrationService.Infrastructure.Data;
using RegistrationService.Infrastructure.Repositories;
using RegistrationService.UseCases.Users.Create;
using RegistrationService.UseCases.Users.Delete;
using RegistrationService.UseCases.Users.GetById;

namespace RegistrationService.UnitTests.UseCases
{
    public class UserUseCaseTest
    {
        private readonly AppDbContext dbContext;
        private readonly UserRepository repository;

        private readonly CreateUserHandler createHandler;
        private readonly DeleteUserHandler deleteHandler;
        private readonly GetByIdHandler getByEmailHandler;

        private readonly CancellationToken cancellationToken;

        Guid userId;
        string userEmail;

        public UserUseCaseTest()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            dbContext = new AppDbContext(options);
            repository = new UserRepository(dbContext);

            createHandler = new CreateUserHandler(repository);
            deleteHandler = new DeleteUserHandler(repository);
            getByEmailHandler = new GetByIdHandler(repository);
            deleteHandler = new DeleteUserHandler(repository);
            getByEmailHandler = new GetByIdHandler(repository);

            SeedDatabase();
        }

        private void SeedDatabase()
        {
            userEmail = "testuser@email.com";
            var user = dbContext.Set<User>().Add(new User(userEmail, 111));
            dbContext.SaveChanges();
            userId = user.Entity.Id;
        }

        [Fact]
        public async Task CreateUser_ShouldReturnCorrectUser()
        {
            // Arrange
            var userEmail = "user@email.com";
            var userPhoneNumber = 111;
            CreateUserCommand command = new CreateUserCommand(userEmail, userPhoneNumber);

            // Act
            var result = await createHandler.Handle(command, cancellationToken);
            var returnUser = await repository.GetByIdAsync(result.Value);

            // Assert
            Assert.Equal(returnUser.Email, userEmail);
        }
        [Fact]
        public async Task DeleteExistingUser_ShouldReturnCorrectMessage()
        {
            // Arrange
            DeleteUserCommand command = new DeleteUserCommand(userId);

            // Act
            var result = await deleteHandler.Handle(command, cancellationToken);

            // Arrange
            Assert.Equal("User Deleteed", result.Value);
        }
        [Fact]
        public async Task DeleteNonExistingUser_ShouldReturnCorrectMessage()
        {
            // Arrange
            DeleteUserCommand command = new DeleteUserCommand(Guid.NewGuid());

            // Act
            var func = async () => await deleteHandler.Handle(command, cancellationToken);

            // Arrange
            await Assert.ThrowsAsync<ArgumentException>(func);
        }
        //[Fact]
        //public async Task GetUserByEmail_ShouldReturnCorrectUser()
        //{
        //    // Arrange
        //    GetByEmailCommand command = new GetByEmailCommand(userEmail);

        //    // Act
        //    var result = await getByEmailHandler.Handle(command, cancellationToken);

        //    // Assert
        //    Assert.Equal(userId, result.Value);
        //}
        //[Fact]
        //public async Task GetNonExistingUserByEmail_ShouldThrowException()
        //{
        //    // Arrange
        //    GetByEmailCommand command = new GetByEmailCommand("user@email.com");

        //    // Act
        //    var func = () => getByEmailHandler.Handle(command, cancellationToken);

        //    // Assert
        //    await Assert.ThrowsAsync<ArgumentNullException>(func);
        //}
    }
}
