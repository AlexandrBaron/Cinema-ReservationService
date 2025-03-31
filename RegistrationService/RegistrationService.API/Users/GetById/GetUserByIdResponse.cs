namespace RegistrationService.API.Users.GetById
{
    public class GetUserByIdResponse(Guid id, string email, int phoneNumber)
    {
        public Guid Id { get; set; } = id;
        public string Email { get; set; } = email;
        public int PhoneNumber { get; set; } = phoneNumber;
    }
}