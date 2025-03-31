namespace RegistrationService.API.Users.Create
{
    public class CreateUserResponse(string email, int phoneNumber)
    {
        public string Email { get; set; } = email;
        public int PhoneNumber { get; set; } = phoneNumber;
    }
}