namespace RegistrationService.UseCases.Reservations.Delete
{
    public class DeleteReservationHandler(IRepository<Reservation> _repository)
        : ICommandHandler<DeleteReservationCommand, Result<string>>
    {
        public async Task<Result<string>> Handle(DeleteReservationCommand request, CancellationToken cancellationToken)
        {
            var reservationToDelete = await _repository.GetByIdAsync(request.Id, cancellationToken);
            if (reservationToDelete == null)
            {
                throw new ArgumentNullException("Reservation with this Id does not exists");
            }
            await _repository.DeleteAsync(reservationToDelete, cancellationToken);
            await _repository.SaveChangesAsync(cancellationToken);

            return Result.Success("Reservation deleted");
        }
    }
}
