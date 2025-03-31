using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using RegistrationService.Core.Entities;
using RegistrationService.Infrastructure.Data;
using RegistrationService.Infrastructure.Repositories;
using RegistrationService.Shared;
using RegistrationService.UseCases.Registrations.Update;
using RegistrationService.UseCases.Reservations.Create;
using RegistrationService.UseCases.Reservations.Delete;
using RegistrationService.UseCases.Reservations.GetOccupiedSeatsProjectionId;
using RegistrationService.UseCases.Reservations.Update;
using ReservationStatus = Cinema.Common.Entities.ReservationService.ReservationStatus;
namespace RegistrationService.UnitTests.UseCases
{
    public class ReservationUseCaseTest
    {
        private readonly AppDbContext dbContext;
        private readonly IRepository<Reservation> repository;
        private readonly ReservationRepository reservationRepository;
        private readonly CreateReservationHandler createHandler;
        private readonly DeleteReservationHandler deleteHandler;
        private readonly GetOccupiedSeatsProjectionIdHandler occupiedSeatsHandler;
        private readonly UpdateReservationHandler updateHandler;
        private readonly Guid userId;
        private readonly Guid projectionId;
        private readonly Guid seatId;
        private Guid reservationId;
        private readonly CancellationToken cancellationToken;

        public ReservationUseCaseTest()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            var reservationOptions = Options.Create(new ReservationOptions
            {
                ExpirationMinutes = new ExpirationMinutesConfig()
                {
                    Development = -0.15,
                    Production = -30
                }
            });
            dbContext = new AppDbContext(options);
            repository = new ReservationRepository(dbContext, reservationOptions);
            reservationRepository = new ReservationRepository(dbContext, reservationOptions);
            createHandler = new CreateReservationHandler(repository, dbContext);
            repository = new ReservationRepository(dbContext, reservationOptions);
            reservationRepository = new ReservationRepository(dbContext, reservationOptions);
            createHandler = new CreateReservationHandler(repository, dbContext);
            deleteHandler = new DeleteReservationHandler(repository);
            occupiedSeatsHandler = new GetOccupiedSeatsProjectionIdHandler(reservationRepository);
            updateHandler = new UpdateReservationHandler(repository);

            userId = Guid.NewGuid();
            projectionId = Guid.NewGuid();
            seatId = Guid.NewGuid();

            SeedDatabase();
        }

        private void SeedDatabase()
        {
            var reservation1 = new Reservation(userId, new List<Guid>() { seatId }, projectionId, ReservationStatus.NotConfirmed);
            var reservation2 = new Reservation(userId, new List<Guid>() { Guid.NewGuid() }, Guid.NewGuid(), ReservationStatus.NotConfirmed);

            var reservationEntity = dbContext.Set<Reservation>().Add(reservation1);
            reservationId = reservationEntity.Entity.Id;
            dbContext.SaveChanges();
        }

        [Fact]
        public async Task Handle_SeatAlreadyReserved_ReturnsError()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var seatIds = new List<Guid> { seatId };
            var projectionId = this.projectionId;

            var request = new CreateReservationCommand(
                userId,
                seatIds,
                projectionId
                );

            var handler = new CreateReservationHandler(repository, dbContext);

            // Act
            var result = await handler.Handle(request, cancellationToken);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(Guid.Empty, result.Value);
        }

        [Fact]
        public async Task DeleteReservation_ShouldReturnCorrectAmountOdReservations()
        {
            // Arrange
            DeleteReservationCommand command = new DeleteReservationCommand(reservationId);

            // Act
            var result = await deleteHandler.Handle(command, cancellationToken);

            // Assert
            Assert.Equal("Reservation deleted", result.Value);
        }
        [Fact]
        public async Task DeleteNonExistingReservation_ShouldThrowExceptionWithMessage()
        {
            // Arrange
            DeleteReservationCommand command = new DeleteReservationCommand(Guid.NewGuid());

            // Act
            var func = async () => await deleteHandler.Handle(command, cancellationToken);

            // Assert
            await Assert.ThrowsAsync<ArgumentNullException>(func);
        }
        //[Fact]
        //public async Task GetOccupiedSeatsByProjectionId_ShouldReturnCorrectSeats()
        //{
        //    // Arrange
        //    GetOccupiedSeatsProjectionIdCommand command = new GetOccupiedSeatsProjectionIdCommand(projectionId);

        //    // Act
        //    var result = await occupiedSeatsHandler.Handle(command, cancellationToken);

        //    // Assert
        //    Assert.True(result.Value.Contains(seatId) && result.Value.Count == 1);
        //}
        [Fact]
        public async Task UpdateReservation_ShouldReturnCorrectStatus()
        {
            // Arrange
            UpdateReservationCommand command = new(reservationId, ReservationStatus.Confirmed);

            // Act
            var handled = await updateHandler.Handle(command, cancellationToken);
            var result = dbContext.Set<Reservation>().SingleOrDefault(r => r.Id == reservationId);

            // Assert
            Assert.Equal(ReservationStatus.Confirmed, result.ReservationStatus);
        }
    }
}
