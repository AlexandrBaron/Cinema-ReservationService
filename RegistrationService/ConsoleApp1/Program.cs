using Cinema.WEB.Api;

namespace ConsoleApp1
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            await Task.Delay(4);
            MovieServiceHttpClient client = new MovieServiceHttpClient();

            CreateProjectionRequest request = new CreateProjectionRequest();
            request.StartTime = new DateTime(2025, 03, 27, 11, 00, 00);
            request.MovieId = Guid.Parse("0195d1e0-d6ba-4562-8889-c83f6438942f");
            request.HallId = Guid.Parse("B0116D00-E611-43F7-EB3B-08DD69DD2B2A");

            await client.CreateProjeciton(request);
        }

    }
}
