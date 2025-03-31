using System.Text.Json;
using Cinema.Common.Entities.MovieService;
using Cinema.Common.RequestModels.MovieService;
namespace Cinema.WEB.Api
{
    public class MovieServiceHttpClient
    {
        private readonly HttpClient _httpClient;
        private readonly JsonSerializerOptions options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        public MovieServiceHttpClient()
        {
            _httpClient = new HttpClient();
        }
        public MovieServiceHttpClient(HttpClient httpClient)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }
        string BaseUrl = "https://localhost:7043/";

        public async Task<CreateMovieRequestBase> GetMovieById(Guid movieId)
        {
            string path = $"{BaseUrl}{Movies.GetByIdPath}{movieId.ToString()}";
            var response = await _httpClient.GetAsync(path);
            var result = await response.Content.ReadAsStringAsync();
            var movie = JsonSerializer.Deserialize<CreateMovieRequestBase>(result, options);
            return movie;
        }
        public async Task<List<MovieEntity>> GetAllMovies()
        {
            string path = $"{BaseUrl}{Movies.GetAllPath}";
            var response = await _httpClient.GetAsync(path);
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();
            var wrapper = JsonSerializer.Deserialize<MovieResponse>(result, options);
            return wrapper?.Movies ?? new List<MovieEntity>();
        }
        public async Task CreateMovie(CreateMovieRequestBase movie)
        {
            string path = $"{BaseUrl}{Movies.CreatePath}";
            var result = await _httpClient.PostAsJsonAsync(path, movie);
        }
        public async Task UpdateMovie(Guid Id, MovieEntity movie)
        {
            string path = $"{BaseUrl}{Movies.UpdatePath}{Id.ToString()}";
            await _httpClient.PutAsJsonAsync(path, movie);
        }
        public async Task DeleteMovie(Guid Id)
        {
            string path = $"{BaseUrl}{Movies.DeletePath}{Id.ToString()}";
            var response = await _httpClient.DeleteAsync(path);
            response.EnsureSuccessStatusCode();
        }
        public async Task CreateProjeciton(CreateProjectionRequestBase projection)
        {
            string path = $"{BaseUrl}{Projections.CreatePath}";
            var result = await _httpClient.PostAsJsonAsync(path, projection);
        }
        public async Task<ProjectionEntity> GetProjectionById(Guid projectionId)
        {
            string path = $"{BaseUrl}{Projections.GetByIdPath}{projectionId.ToString()}";
            var response = await _httpClient.GetAsync(path);
            var content = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<ProjectionEntity>(content, options);
            return result;
        }
        public async Task<List<SeatAvailabilityResponse>> GetSeatsProjectionId(Guid projectionId)
        {
            string path = $"{BaseUrl}{Projections.GetAvailableSeatsPath}{projectionId.ToString()}";
            var response = await _httpClient.GetAsync(path);
            var content = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<List<SeatAvailabilityResponse>>(content, options);
            return result;
        }
        public async Task<List<ProjectionEntity>> GetAllProjectionsOfMovie(Guid movieId)
        {
            var path = $"{BaseUrl}{Movies.GetProjectionsPath}{movieId.ToString()}/projections";
            var response = await _httpClient.GetAsync(path);
            var content = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<ProjectionApiResponse>(content, options);
            return result.Value.Projections;
        }
    }
    public class MovieResponse
    {
        public List<MovieEntity> Movies { get; set; }
    }
    public record ProjectionApiResponse(bool IsSuccess, string ErrorMessage, ProjectionValue Value);
    public record ProjectionValue(List<ProjectionEntity> Projections);
    public class SeatAvailabilityResponse
    {
        public Guid SeatId { get; set; }
        public int SeatNumber { get; set; }

        public int RowNumber { get; set; }

        public bool IsOccupied { get; set; }
    }

}
