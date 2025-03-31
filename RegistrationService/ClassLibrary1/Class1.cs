using Cinema.WEB.Api;
namespace ClassLibrary1
{
    public class Class1
    {
        public static async Task Main(string[] args)
        {
            HttpClient httpClient = new HttpClient
            {
                BaseAddress = new Uri("https://localhost:7275/") // Nahraď skutečnou adresou API
            };

            UsersClient client = new UsersClient(httpClient);

            var result = await client.RegistrationServiceAPIUsersGetByEmailGetByEmailAsync("user1@email.com");

            Console.WriteLine(result);
        }
    }
}
