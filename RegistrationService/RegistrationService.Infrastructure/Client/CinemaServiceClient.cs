namespace RegistrationService.Infrastructure.Client
{
    public class CinemaServiceClient
    {
        private readonly HttpClient _httpClient;
        private readonly JsonSerializerOptions options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };
        public CinemaServiceClient(HttpClient httpClient)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }

        private const string BaseUrl = "https://localhost:7254/";
        private const string GetHallByIdPath = "Halls/GetById/";
        private const string GetSeatById = "Seats/GetById/";
        private const string GetRowById = "Rows/GetById";

        public async Task<string> GetHallByHallId(Guid hallId)
        {
            string path = BaseUrl + GetHallByIdPath + hallId.ToString();

            var response = await _httpClient.GetAsync(path);

            var content = await response.Content.ReadAsStringAsync();

            var result = JsonSerializer.Deserialize<Hall>(content, options);

            return result.Name;
        }

        public async Task<string> GetSeatInfo(Guid seatId)
        {
            var seatPath = $"{BaseUrl}{GetSeatById}{seatId}";
            var response = await _httpClient.GetAsync(seatPath);

            if (!response.IsSuccessStatusCode)
                throw new Exception($"Chyba při načítání seat: {response.StatusCode}");

            var result = await response.Content.ReadAsStringAsync();

            if (string.IsNullOrWhiteSpace(result))
                throw new Exception("Seat response je prázdný");

            var seat = JsonSerializer.Deserialize<Seat>(result, options)
                ?? throw new Exception("Deserializace seat selhala");

            var rowPath = $"{BaseUrl}{GetRowById}?id={seat.RowId}";

            var rowResponse = await _httpClient.GetAsync(rowPath);

            if (!rowResponse.IsSuccessStatusCode)
                throw new Exception($"Chyba při načítání seat: {rowResponse.StatusCode}");

            var rowResult = await rowResponse.Content.ReadAsStringAsync();

            if (string.IsNullOrWhiteSpace(rowResult))
                throw new Exception("Seat response je prázdný");

            var row = JsonSerializer.Deserialize<Seat>(rowResult, options)
                ?? throw new Exception("Deserializace seat selhala");

            return $"sedadlo {seat.Number}, rada {row.Number}";
        }


    }
}
public class Seat
{
    public Guid Id { get; set; }
    public int Number { get; set; }
    public Guid RowId { get; set; }

}
public class Row : BaseEntity
{
    public Guid Id { get; set; }
    public int Number { get; set; }
    public Guid HallId { get; set; }
}

public class Hall : BaseEntity
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public HallStatus Status { get; set; }
    public Hall()
    {

    }
    public Hall(string name, HallStatus status)
    {
        Name = name;
        Status = status;
    }

    public enum HallStatus
    {
        InOperation,
        OutOfOperation
    }
}
