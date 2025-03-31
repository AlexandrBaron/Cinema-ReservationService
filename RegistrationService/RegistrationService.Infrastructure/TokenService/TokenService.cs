using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace RegistrationService.Infrastructure.TokenService
{
    public class TokenService
    {
        private readonly string _secretKey;
        private readonly int _tokenExpirationMinutes;

        public TokenService(IConfiguration configuration)
        {
            _secretKey = configuration["JwtSettings:SecretKey"];
            _tokenExpirationMinutes = int.Parse(configuration["JwtSettings:TokenExpirationMinutes"]);
        }

        public string GenerateToken(Guid reservationId)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: "ReservationService",
                audience: "ReservationService",
                claims: new[] { new Claim("reservationId", reservationId.ToString()) },
                expires: DateTime.Now.AddMinutes(_tokenExpirationMinutes),
                signingCredentials: credentials
                );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public Guid? ValidateToken(string token)
        {
            try
            {
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey));
                var handler = new JwtSecurityTokenHandler();

                var validationParametres = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = "ReservationService",
                    ValidAudience = "ReservationService",
                    IssuerSigningKey = key,
                    ClockSkew = TimeSpan.Zero
                };

                var principal = handler.ValidateToken(token, validationParametres, out _);
                var reservationId = principal.FindFirst("reservationId")?.Value;

                return reservationId != null ? Guid.Parse(reservationId) : null;
            }
            catch
            {
                return null;
            }
        }
    }
}
