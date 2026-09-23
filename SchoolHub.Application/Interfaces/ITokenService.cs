using System.Security.Claims;

namespace SchoolHub.Application.Interfaces
{
    public interface ITokenService
    {
        string GenerateToken(string userId, string userName, string role,out string jwtId);
        string GenerateRefreshToken();
        ClaimsPrincipal? GetClaimsPrincipalFromExpiredToken(string token);
    }
}