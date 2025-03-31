using FluentValidation;
using RegistrationService.API.Reservation.Create;

namespace RegistrationService.UseCases.Registrations.Create;

public class CreateReservationValidator : Validator<CreateReservationRequest>
{
    public CreateReservationValidator()
    {
        RuleFor(x => x.ProjectionId)
            .NotEmpty()
            .WithMessage("Projection Id is required!");

        RuleFor(x => x.SeatId)
            .NotEmpty()
            .WithMessage("At leats one seat Id is required!");

        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithMessage("User Id is required!");
    }
}
