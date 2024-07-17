using Shared.DTO.Login.Response;
using Shared.DTO.User.Response;

namespace Shared.Interface;

public interface ITokenService
{
    Task<string> GenerateTokenAsync(UserDetailsResponse userDetailsResponse);
}
