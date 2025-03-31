namespace RegistrationService.UseCases.Users.GetById
{
    public class GetByIdHandler(IRepository<User> _repository)
        : ICommandHandler<GetByIdCommand, Result<User>>
    {
        public async Task<Result<User>> Handle(GetByIdCommand request, CancellationToken cancellationToken)
        {
            var user = await _repository.GetByIdAsync(request.Id);

            if (user == null)
            {
                //throw new ArgumentNullException("There is not user with this email address");
                return Result.Error("There is not user with this Id");
            }
            return Result.Success(user);
        }
    }
}
