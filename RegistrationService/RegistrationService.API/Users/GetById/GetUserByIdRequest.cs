namespace RegistrationService.API.Users.GetById

{
    public class GetUserByIdRequest
    {
        public const string Route = "/Users/GetById/{Id:Guid}";
        public static string BuildRoute(Guid Id) => Route.Replace("{Id}", Id.ToString());
        public required Guid Id { get; set; }
    }
}