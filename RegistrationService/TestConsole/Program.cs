using UserClient = Cinema.WEB.Api.UsersClient;

namespace TestConsole
{
    internal class Program
    {
        public static System.Net.Http.HttpClient _httpClient = new HttpClient();
        static async Task Main(string[] args)
        {

            var url = "https:/localhost:7275/http:/users/GetByEmail/user1@email.com";

            UserClient userClient = new UserClient(_httpClient);

            var result = await userClient.RegistrationServiceAPIUsersGetByEmailGetByEmailAsync(url);

            Console.WriteLine(result);

            HttpClient client = new HttpClient();
            client.GetAsync();
        }
    }
}
