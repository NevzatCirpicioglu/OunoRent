using Shared.DTO.Login.Request;
using Shared.DTO.User.Request;

namespace Shared.Interface;

public interface IAuthService
{
    Task<string> Register(CreateUserRequest createUserRequest);
    Task<string> Login(LoginRequest loginRequest);
}
