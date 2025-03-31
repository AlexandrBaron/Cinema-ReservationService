using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using RegistrationService.Infrastructure.BackgroundJob;
using RegistrationService.Infrastructure.Data;
using RegistrationService.Infrastructure.Repositories;
using RegistrationService.Shared;
using Reservation = RegistrationService.Core.Entities.Reservation;
using ReservationStatus = Cinema.Common.Entities.ReservationService.ReservationStatus;


namespace RegistrationService.UnitTests.Infrastructure
{
    public class ReservationRepositoryTests
    {
        private readonly ReservationRepository repository;
        private readonly AppDbContext appDbContext;
        private readonly List<Guid> userIds;
        private readonly List<Guid> seatIds;
        private readonly List<Guid> reservationsId;
        private readonly Guid projection1Id;
        private readonly Guid projection2Id;
        private IOptions<ReservationSettings> reservationSettings;
        private readonly DeleteExpiredReservations deleteExpiredReservations;

        public ReservationRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            var reservationOptions = Options.Create(new ReservationOptions
            {
                ExpirationMinutes = new ExpirationMinutesConfig
                {
                    Development = -0.15,
                    Production = -30
                }
            });



            appDbContext = new AppDbContext(options);
            repository = new ReservationRepository(appDbContext, reservationOptions);

            userIds = new List<Guid>();
            seatIds = new List<Guid>();

            projection1Id = Guid.NewGuid();
            projection2Id = Guid.NewGuid();
            reservationsId = new List<Guid>();

            reservationSettings = Options.Create(new ReservationSettings
            {
                ExpirationTimeInMinutes = 30
            });
            deleteExpiredReservations = new DeleteExpiredReservations(repository);

            SeedDatabase();
        }

        public void SeedDatabase()
        {
            for (int i = 0; i < 10; i++)
            {
                userIds.Add(Guid.NewGuid());
                seatIds.Add(Guid.NewGuid());
                if (i < 5)
                {
                    var result = appDbContext.Set<Reservation>().Add(new Reservation(userIds[i], new List<Guid>() { seatIds[i] }, projection1Id, ReservationStatus.NotConfirmed));
                    appDbContext.SaveChanges();
                    reservationsId.Add(result.Entity.Id);
                }
                appDbContext.Set<Reservation>().Add(new Reservation(userIds[i], new List<Guid>() { seatIds[i] }, projection2Id, ReservationStatus.NotConfirmed));
                appDbContext.SaveChanges();
            }
        }
        [Fact]
        public void GetAllReservationsByProjectionId_ShouldReturnCrrectReservations()
        {
            var result = repository.GetByProjectionId(projection1Id);

            for (int i = 0; i < result.Count; i++)
            {
                Assert.Equal(result[i].Id, reservationsId[i]);
            }
        }
        [Fact]
        public void GetAllReservationsByEmptyProjectionId_ShouldReturnZeroReservations()
        {
            var result = repository.GetByProjectionId(Guid.NewGuid());

            Assert.Equal(0, result.Count);
        }

        [Fact]
        public async Task Execute_ShouldDeleteExpiredReservations()
        {
            // Arrange
            var expirationTimeInMinutes = 30;
            reservationSettings = Options.Create(new ReservationSettings { ExpirationTimeInMinutes = expirationTimeInMinutes });

            var pastTime = DateTime.UtcNow.AddMinutes(-expirationTimeInMinutes - 1);
            var recentTime = DateTime.UtcNow;


            appDbContext.Set<Reservation>().AddRange(new List<Reservation>
            {
                new Reservation(Guid.NewGuid(), new List<Guid> { Guid.NewGuid() }, projection1Id, ReservationStatus.NotConfirmed) { TimeOfCreation = pastTime },
                new Reservation(Guid.NewGuid(), new List<Guid> { Guid.NewGuid() }, projection1Id, ReservationStatus.NotConfirmed) { TimeOfCreation = pastTime }
            });


            appDbContext.Set<Reservation>().AddRange(new List<Reservation>
            {
                new Reservation(Guid.NewGuid(), new List<Guid> { Guid.NewGuid() }, projection1Id, ReservationStatus.NotConfirmed) { TimeOfCreation = recentTime }
            });

            appDbContext.SaveChanges();

            var job = new DeleteExpiredReservationsJob(deleteExpiredReservations, reservationSettings);

            // Act
            await job.Execute(null);

            // Assert
            var remainingReservations = appDbContext.Set<Reservation>().ToList();
            Assert.DoesNotContain(remainingReservations, r => r.TimeOfCreation <= pastTime);
        }



    }
}
