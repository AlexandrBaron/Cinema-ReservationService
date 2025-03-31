namespace RegistrationService.API.Users.GetByEmail
{
    public class GetUsersByEmailRequest
    {
        public const string Route = "/Users/GetByEmail{Email}";
        public static string BuildRoute(string Email) => Route.Replace("{Email}", Email);
        public required string Email { get; set; }
    }
}