namespace RegistrationService.Core.Entities
{
    public class User : BaseEntity
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public int PhoneNumber { get; set; }
        public User()
        {

        }
        public User(string email, int phoneNumber)
        {
            Email = email;
            PhoneNumber = phoneNumber;
        }
        public void UpdateUser(string email, int phoneNumber)
        {
            Email = Guard.Against.Null(email, nameof(email));
        }
    }
}
