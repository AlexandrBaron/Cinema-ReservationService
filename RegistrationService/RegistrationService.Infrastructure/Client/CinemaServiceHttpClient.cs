using System.Text.Json;
using Cinema.Common.Entities.CinemaService;
namespace RegistrationService.Infrastructure.Client
{
    public class CinemaServiceHttpClient
    {
        private readonly HttpClient _httpClient;
        public CinemaServiceHttpClient()
        {
            _httpClient = new HttpClient();
        }
        public CinemaServiceHttpClient(HttpClient httpClient)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }

        private const string BaseUrl = "https://localhost:7254/";
        JsonSerializerOptions options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        public async Task<List<Hall>> GetAllHalls()
        {
            string path = $"{BaseUrl}{Halls.GetAllHallsPath}";
            var response = await _httpClient.GetAsync(path);
            var content = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<List<Hall>>(content, options);
            return result;
        }
        public async Task<Hall> GetHallByHallId(Guid hallId)
        {
            string path = $"{BaseUrl}{Halls.GetHallByIdPath}{hallId.ToString()}";
            var response = await _httpClient.GetAsync(path);
            var content = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<Hall>(content, options);
            return result;
        }
        public async Task<string> GetSeatInfo(Guid seatId)
        {
            var seatPath = $"{BaseUrl}{Seats.GetById}{seatId}";
            var response = await _httpClient.GetAsync(seatPath);
            var result = await response.Content.ReadAsStringAsync();
            var seat = JsonSerializer.Deserialize<Seat>(result, options);

            var rowPath = $"{BaseUrl}{Rows.GetById}?id={seat.RowId}";
            var rowResponse = await _httpClient.GetAsync(rowPath);
            var rowResult = await rowResponse.Content.ReadAsStringAsync();
            var row = JsonSerializer.Deserialize<Seat>(rowResult, options);

            return $"sedadlo {seat.Number}, řada {row.Number}";
        }
    }
}