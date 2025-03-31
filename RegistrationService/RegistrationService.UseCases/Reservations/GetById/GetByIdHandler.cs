namespace RegistrationService.UseCases.Reservations.GetById
{
    public class GetByIdHandler(IRepository<Reservation> _repository)
        : ICommandHandler<GetByICommand, Result<Reservation>>
    {
        public async Task<Result<Reservation>> Handle(GetByICommand request, CancellationToken cancellationToken)
        {
            var reservation = await _repository.GetByIdAsync(request.Id);

            if (reservation == null)
            {
                return Result.Error("There is no reservation with this Id");
            }
            return Result.Success(reservation);
        }
    }
}
