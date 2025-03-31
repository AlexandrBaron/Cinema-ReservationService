using RegistrationService.Infrastructure.Repositories;

namespace RegistrationService.UseCases.Users.Create;
public class CreateUserHandler(UserRepository _repository)
    : ICommandHandler<CreateUserCommand, Result<Guid>>
{

    public async Task<Result<Guid>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var newUser = new User(request.Email, request.PhoneNumber)
        {
            Email = request.Email,
            PhoneNumber = request.PhoneNumber,
        };


        var exist = _repository.ExistsWithEmail(newUser.Email);

        if (exist)
        {
            return Result.Error("User with this email already exists");
        }
        else
        {
            var createUser = await _repository.AddAsync(newUser, cancellationToken);
            await _repository.SaveChangesAsync();

            return Result.Success(createUser.Id);
        }
    }
}
