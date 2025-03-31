using RegistrationService.UseCases.Registrations.Update;

namespace RegistrationService.UseCases.Reservations.Update
{
    public class UpdateReservationHandler(IRepository<Reservation> _repository)
        : ICommandHandler<UpdateReservationCommand, Result>
    {
        public async Task<Result> Handle(UpdateReservationCommand request, CancellationToken cancellationToken)
        {
            var reservationToUpdate = await _repository.GetByIdAsync(request.Id, cancellationToken);

            if (reservationToUpdate.SeatId is null)
            {
                throw new ArgumentException("There are no seats selected");
            }

            reservationToUpdate.ReservationStatus = request.reservationStatus;

            await _repository.UpdateAsync(reservationToUpdate, cancellationToken);
            await _repository.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
