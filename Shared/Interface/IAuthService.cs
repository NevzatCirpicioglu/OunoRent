using Shared.DTO.Login.Request;
using Shared.DTO.Register.Request;
using Shared.DTO.User.Response;

namespace Shared.Interface;

public interface IAuthService
{
    Task<UserResponse> RegisterAsync(RegisterRequest registerRequest);
    Task<string> LoginAsync(LoginRequest loginRequest);
}
