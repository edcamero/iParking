using System.Security.Claims;

namespace iParking.Infrastructure.Security
{
    public interface ITokenService
    {
        string GenerateToken(string userId);

        ClaimsPrincipal? ValidateToken(string token);

        int GetUserIdFromToken(string token);
    }
}