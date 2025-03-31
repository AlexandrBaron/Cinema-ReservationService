using Microsoft.EntityFrameworkCore;
using RegistrationService.Infrastructure.Data;

namespace RegistrationService.UseCases.Reservations.Create
{
    public class CreateReservationHandler(IRepository<Reservation> _repository, AppDbContext dbContext)
        : ICommandHandler<CreateReservationCommand, Result<Guid>>
    {
        public async Task<Result<Guid>> Handle(CreateReservationCommand request, CancellationToken cancellationToken)
        {

            var existingReservation = await dbContext.Reservations
                .Where(r => r.ProjectionId == request.ProjectionId && r.SeatId == request.SeatIds)
                .FirstOrDefaultAsync(cancellationToken);

            if (existingReservation != null)
            {
                return Result.Error("The seat is already reserved for this projection.");
            }

            await using var transaction = await dbContext.Database.BeginTransactionAsync(cancellationToken);
            try
            {
                var reservation = new Reservation()
                {
                    ProjectionId = request.ProjectionId,
                    UserId = request.UserId,
                    SeatId = request.SeatIds,
                    ReservationStatus = 0,
                    TimeOfCreation = DateTime.Now,
                };

                var result = await _repository.AddAsync(reservation, cancellationToken);
                await _repository.SaveChangesAsync(cancellationToken);
                await transaction.CommitAsync(cancellationToken);
                return Result.Success(result.Id);
            }

            catch (Exception)
            {
                await transaction.RollbackAsync(cancellationToken);
                throw;
            }
        }
    }
}
