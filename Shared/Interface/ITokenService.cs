using System.Security.Claims;
using Shared.DTO.Login.Response;

namespace Shared.Interface;

public interface ITokenService
{
    Task<string> GenerateTokenAsync(UserDetailsResponse userDetailsResponse);
    ClaimsPrincipal? GetPrincipal(string token);
    Task<string?> RefreshTokenAsync(string token);
}
