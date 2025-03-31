using FluentValidation;
using RegistrationService.API.Users.Create;

namespace RegistrationService.UseCases.Users.Create;
public class CreateUserValidator : Validator<CreateUserRequest>
{
    public CreateUserValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage("Email is required!")
            .MinimumLength(5)
            .MaximumLength(100);

        RuleFor(x => x.PhoneNumber)
            .NotEmpty()
            .WithMessage("Phone number is required!");
    }
}
