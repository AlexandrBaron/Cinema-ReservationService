using RegistrationService.Infrastructure.Repositories;

namespace RegistrationService.UseCases.Reservations.GetOccupiedSeatsProjectionId
{
    public class GetOccupiedSeatsProjectionIdHandler(ReservationRepository _repository)
        : ICommandHandler<GetOccupiedSeatsProjectionIdCommand, Result<List<Guid>>>
    {
        public async Task<Result<List<Guid>>> Handle(GetOccupiedSeatsProjectionIdCommand request, CancellationToken cancellationToken)
        {
            var reservationsOnProjection = _repository.GetByProjectionId(request.projectionId);

            if (reservationsOnProjection == null)
            {
                throw new ArgumentException("There are no reservation with this projection Id");
            }

            var occupiedSeat = new List<Guid>();

            foreach (var item in reservationsOnProjection)
            {

                occupiedSeat.AddRange(item.SeatId);
            }

            return Result.Success(occupiedSeat);
        }
    }
}
