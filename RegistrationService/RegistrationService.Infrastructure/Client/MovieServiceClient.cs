using System.Text.Json;

namespace RegistrationService.Infrastructure.Client
{
    public class MovieServiceClient
    {
        public HttpClient _httpClient;
        public MovieServiceClient(HttpClient httpClient)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }

        string baseUrl = "https://localhost:7043/";
        string getAllMovies = "movies/Detail";
        string getMovieById = $"movies/";
        string getAllProjectionsOfMovie = "movies/";
        string getHallId = "projections/";
        string getAllSeats = "projections/seats/available-seats/";
        string getProjectionById = "projections/";


        public async Task<List<DetailMovieRecord>> GetAllMovies()
        {
            string path = baseUrl + getAllMovies;

            var response = await _httpClient.GetAsync(path);
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadAsStringAsync();

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var wrapper = JsonSerializer.Deserialize<MovieResponse>(result, options);
            return wrapper?.Movies ?? new List<DetailMovieRecord>();
        }
        public async Task<MovieRecord> GetMovieById(Guid movieId)
        {
            string path = baseUrl + getMovieById + movieId.ToString();

            var response = await _httpClient.GetAsync(path);

            var result = await response.Content.ReadAsStringAsync();

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var movie = JsonSerializer.Deserialize<MovieRecord>(result, options);

            return movie;
        }
        public async Task<List<ProjectionRecord>> GetAllProjectionsOfMovie(Guid movieId)
        {
            var path = baseUrl + getAllProjectionsOfMovie + movieId + "/projections";

            var response = await _httpClient.GetAsync(path);

            var content = await response.Content.ReadAsStringAsync();

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var result = JsonSerializer.Deserialize<ProjectionApiResponse>(content, options);

            return result.Value.Projections;
        }

        public async Task<List<SeatAvailabilityResponse>> GetSeatsProjectionId(Guid projectionId)
        {
            string path = baseUrl + getAllSeats + projectionId.ToString();

            var response = await _httpClient.GetAsync(path);

            var content = await response.Content.ReadAsStringAsync();

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var result = JsonSerializer.Deserialize<List<SeatAvailabilityResponse>>(content, options);

            return result;
        }
        public async Task<ProjectionRecord> GetProjectionById(Guid projectionId)
        {
            string path = baseUrl + getProjectionById + projectionId.ToString();

            var response = await _httpClient.GetAsync(path);

            var result = await response.Content.ReadAsStringAsync();

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            return JsonSerializer.Deserialize<ProjectionRecord>(result, options);
        }

    }
    public class MovieResponse
    {
        public List<DetailMovieRecord> Movies { get; set; }
    }
    public record MovieRecord(string Title, string Description, string Director, string Cast, long DurationInMinutes);
    public record DetailMovieRecord(Guid Id, string Title, string Description, string Director, string Cast, long DurationInMinutes);
    public record ProjectionApiResponse(bool IsSuccess, string ErrorMessage, ProjectionValue Value);
    public record ProjectionValue(List<ProjectionRecord> Projections);
    public record ProjectionRecord(Guid Id, DateTime StartTime, Guid MovieId, Guid HallId);

    public class SeatAvailabilityResponse
    {
        public Guid SeatId { get; set; }
        public int SeatNumber { get; set; }

        public int RowNumber { get; set; }

        public bool IsOccupied { get; set; }
    }

}

