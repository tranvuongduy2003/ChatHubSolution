using ChatHubSolution.Data.Entities;
using System.Security.Claims;

namespace ChatHubSolution.Services
{
    public interface ITokenService
    {
        Task<string> GenerateAccessTokenAsync(User user);
        string GenerateRefreshToken();
        ClaimsPrincipal? GetPrincipalFromToken(string token);
        bool ValidateTokenExpire(string token);
    }
}
