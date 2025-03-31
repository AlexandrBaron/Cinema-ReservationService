using RegistrationService.Infrastructure.Repositories;

namespace RegistrationService.UseCases.Users.GetByEmail
{
    public class GetByEmailHandler(UserRepository _repository)
        : ICommandHandler<GetByEmailCommand, Result<Guid>>
    {
        public async Task<Result<Guid>> Handle(GetByEmailCommand request, CancellationToken cancellationToken)
        {
            var user = await _repository.GetUserByEmail(request.email);

            if (user == null)
            {
                //throw new ArgumentNullException("There is not user with this email address");
                return Result.Error("There is not user with this email address");
            }
            return Result.Success(user.Id);
        }
    }
}
