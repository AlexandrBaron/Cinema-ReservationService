namespace RegistrationService.CLientLibrary
{
    public class WebHttClient
    {
        private static readonly HttpClient _httpClient = new HttpClient();

        string baseUrl = "https://localhost:7275/";

        string seatsById = $"Seats/GetById/";
        string getOccupiedSeats = "Reservation/GetOccupiedSeatsByProjectionId/";
        string getUserIdByEmail = "Users/GetByEmail";

        public async Task<Guid> GetUserIdByEmail(string email)
        {
            string path = baseUrl + getUserIdByEmail + email;

            var httpResponse = await _httpClient.GetAsync(path);

            var response = await httpResponse.Content.ReadAsStringAsync();

            string cleanedInput = response.Trim('"');

            return Guid.Parse(cleanedInput);
        }

    }
}
