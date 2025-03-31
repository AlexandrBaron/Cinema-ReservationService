namespace RegistrationService.API.Users.Delete
{
    public class DeleteUserRequest
    {
        public const string Route = "/Users/Delete{Id:Guid}";
        public static string BuildRoute(Guid Id) => Route.Replace("{Id:Guid}", Id.ToString());
        public Guid Id { get; set; }
    }
}