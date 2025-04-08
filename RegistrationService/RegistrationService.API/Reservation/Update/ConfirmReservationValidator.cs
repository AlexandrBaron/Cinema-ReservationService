using FluentValidation;

namespace RegistrationService.UseCases.Registrations.Update;
public class ConfirmReservationValidator : Validator<ConfirmReservationRequest>
{
    public ConfirmReservationValidator()
    {
        RuleFor(x => x.Rs)
            .IsInEnum()
            .WithMessage("Invalid reservation status.");

    }
}
