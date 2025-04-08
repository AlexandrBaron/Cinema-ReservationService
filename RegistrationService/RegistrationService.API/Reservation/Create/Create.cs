using System.Data;
using RegistrationService.Infrastructure.RabbitMQ;
using RegistrationService.Infrastructure.Repositories;
using RegistrationService.Infrastructure.TokenService;
using RegistrationService.UseCases.Reservations.Create;

namespace RegistrationService.API.Reservation.Create
{
    public class Create(IMediator mediator, RabbitMQManager rabbitMQManager, TokenService tokenService, UserRepository userRepository, MovieServiceHttpClient movieClient, CinemaServiceHttpClient cinemaClient)
        : Endpoint<CreateReservationRequest, CreateReservationRespons>
    {
        public override void Configure()
        {
            Post(CreateReservationRequest.Route);
            AllowAnonymous();
            Summary(s =>
            {
                s.ExampleRequest = new CreateReservationRequest { };
            });
        }

        public override async Task HandleAsync(CreateReservationRequest req, CancellationToken ct)
        {
            try
            {
                var result = await mediator.Send(new CreateReservationCommand(req.UserId, req.SeatId, req.ProjectionId), ct);

                if (result.IsSuccess)
                {
                    Response = new CreateReservationRespons(result, req.UserId, req.SeatId, req.ProjectionId);

                    string token = tokenService.GenerateToken(result.Value);

                    string confirmationLink = $"https://localhost:7165/Reservation/Confirm?token={token}";

                    var user = await userRepository.GetByIdAsync(req.UserId);
                    var userEmail = user?.Email;

                    var projection = await movieClient.GetProjectionById(req.ProjectionId);
                    var movie = await movieClient.GetMovieById(projection.MovieId);
                    var hall = await cinemaClient.GetHallByHallId(projection.HallId);

                    var seatInfos = new List<string>();
                    foreach (var seatId in req.SeatId)
                    {
                        var seat = await cinemaClient.GetSeatInfo(seatId);
                        seatInfos.Add(seat);
                    }

                    string message = $"Rezervace pro uzivatele {userEmail}, na {movie.Title} v Sale {hall} se zacatkem {projection.StartTime},  {string.Join(", ", seatInfos)}";

                    var messageModel = new RabbitMQMessageModel
                    {
                        ReservationId = result.Value,
                        Message = message,
                        ConfirmationLink = confirmationLink,
                    };

                    await rabbitMQManager.PublishMessageAsync(messageModel);
                }
            }
            catch (DBConcurrencyException)
            {
                ThrowError("Sedadla byla zarezervovana jiným uživatelem");
            }
        }
    }
}

