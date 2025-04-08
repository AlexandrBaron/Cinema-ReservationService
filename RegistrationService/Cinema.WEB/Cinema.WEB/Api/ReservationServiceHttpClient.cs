using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using Cinema.Common.RequestModels.ReservationService;
using RegistrationService.API.Reservation.Update;
using RegistrationService.API.Users.GetById;
using RegistrationService.Core.Entities;
using JsonSerializer = System.Text.Json.JsonSerializer;
using ReservationStatus = Cinema.Common.Entities.ReservationService.ReservationStatus;

namespace Cinema.WEB.Api
{
    public class ReservationServiceHttpClient
    {
        private readonly HttpClient _httpClient;
        private readonly JsonSerializerOptions options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };
        private readonly JsonSerializerOptions optionsForEnums = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            Converters = { new JsonStringEnumConverter() }
        };
        public ReservationServiceHttpClient()
        {
            _httpClient = new HttpClient();
        }
        public ReservationServiceHttpClient(HttpClient httpClient)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }
        string BaseUrl = "https://localhost:7275/";
        public async Task CreateUserUser(int phoneNumber, string email)
        {
            string path = $"{BaseUrl}{Users.CreatePath}";

            CreateUserRequestBase userToCreate = new CreateUserRequestBase();
            userToCreate.PhoneNumber = phoneNumber;
            userToCreate.Email = email;

            var response = await _httpClient.PostAsJsonAsync(path, userToCreate);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception();
            }
        }
        public async Task<User> GetUserById(Guid Id)
        {
            string path = $"{BaseUrl}{Users.GetByIdPath}{Id.ToString()}";
            var response = await _httpClient.GetAsync(path);
            var content = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<User>(content, options);
            return result;
        }
        public async Task<Guid> GetUserIdByEmail(string email)
        {
            string path = $"{BaseUrl}{Users.GetByEmailPath}{email}";
            try
            {
                var httpResponse = await _httpClient.GetAsync(path);
                var response = await httpResponse.Content.ReadAsStringAsync();

                if (httpResponse.StatusCode == HttpStatusCode.OK)
                {
                    var cleanedInput = response.Trim('"', '\r', '\n', ' ');

                    if (Guid.TryParse(cleanedInput, out var userId))
                        return userId;
                }
                return Guid.Empty;
            }
            catch
            {
                return Guid.Empty;
            }
        }
        public async Task<Reservation> GetReservationById(Guid Id)
        {
            string path = $"{BaseUrl}{Reservations.GetByIdPath}{Id.ToString()}";
            var response = await _httpClient.GetAsync(path);
            var content = await response.Content.ReadAsStringAsync();
            var reservation = JsonSerializer.Deserialize<Reservation>(content, optionsForEnums);
            return reservation;
        }
        public async Task<Guid> CreateReservation(Guid projectionId, List<Guid> seatIds, Guid userId)
        {
            string path = $"{BaseUrl}{Reservations.CreatePath}";

            CreateReservationRequestBase create = new CreateReservationRequestBase();
            create.ProjectionId = projectionId;
            create.SeatId = seatIds;
            create.UserId = userId;

            var response = await _httpClient.PostAsJsonAsync(path, create);
            var content = await response.Content.ReadAsStringAsync();
            var reservation = JsonSerializer.Deserialize<Reservation>(content, optionsForEnums);

            var res = reservation.Id;
            return reservation.Id;
        }
        public async Task ConfirmReservation(string token)
        {
            string path = $"{BaseUrl}{Reservations.ConfirmPath}?token={token}";

            var request = new ConfirmReservationRequest()
            {
                Rs = ReservationStatus.Confirmed
            };

            var response = await _httpClient.PutAsJsonAsync(path, request);
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Nepodařilo se potvrdit rezervaci.");
            }
        }
    }
}

