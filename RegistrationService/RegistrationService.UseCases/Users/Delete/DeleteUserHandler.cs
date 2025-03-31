
namespace RegistrationService.UseCases.Users.Delete;
public class DeleteUserHandler(IRepository<User> _repository)
    : ICommandHandler<DeleteUserCommand, Result<string>>
{
    public async Task<Result<string>> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        var userToDelete = await _repository.GetByIdAsync(request.Id);
        if (userToDelete is null)
        {
            throw new ArgumentException("There is no user with this Id");
        }
        await _repository.DeleteAsync(userToDelete, cancellationToken);
        await _repository.SaveChangesAsync();

        return Result.Success("User Deleteed");
    }
}
