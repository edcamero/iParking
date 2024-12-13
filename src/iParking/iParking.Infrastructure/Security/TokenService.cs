using iParking.Domain.Entities.Auth;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace iParking.Infrastructure.Security
{
    public class TokenService : ITokenService
    {
        private readonly JwtOptions _jwtOptions;

        public TokenService(IOptions<JwtOptions> jwtOptions)
        {
            _jwtOptions = jwtOptions.Value;
        }

        public string GenerateToken(string userId)
        {
            var claims = new[]
            {
            new Claim(JwtRegisteredClaimNames.Sub, userId),
            new Claim("userId", userId),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.Key));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _jwtOptions.Issuer,
                audience: _jwtOptions.Issuer,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_jwtOptions.ExpirationMinutes),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public ClaimsPrincipal? ValidateToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_jwtOptions.Key);

            try
            {
                var validationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = _jwtOptions.ValidateIssuer,
                    ValidateAudience = _jwtOptions.ValidateAudience,
                    ValidateLifetime = _jwtOptions.ValidateLifetime,
                    ValidateIssuerSigningKey = _jwtOptions.ValidateIssuerSigningKey,
                    ValidIssuer = _jwtOptions.Issuer,
                    ValidAudience = _jwtOptions.Issuer,
                    IssuerSigningKey = new SymmetricSecurityKey(key)
                };

                var principal = tokenHandler.ValidateToken(token, validationParameters, out _);
                return principal;
            }
            catch (Exception ex)
            {
                // Maneja errores de validación
                Console.WriteLine($"Error validando el token: {ex.Message}");
                return null;
            }
        }

        public  int GetUserIdFromToken(string token)
        {
            // Validar el token y obtener los claims
            var claims = ValidateToken(token);

            if (claims is null)
            {
                throw new ArgumentException("El token no contiene claims válidos.");
            }

            // Buscar el claim 'sub'
            var claim = claims.FindFirst("userId");

            if (claim == null)
            {
                throw new InvalidOperationException("El claim 'sub' no está presente en el token.");
            }

            // Convertir el valor del claim a int
            if (!int.TryParse(claim.Value, out int userId))
            {
                throw new FormatException("El valor del claim 'sub' no es un número válido.");
            }

            return userId;
        }

    }
}
