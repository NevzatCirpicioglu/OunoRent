using Shared.DTO.Login.Request;
using Shared.DTO.User.Request;
using Shared.Interface;

namespace BusinessLayer.Services.Authhentication;

public class AuthService : IAuthService
{
    private readonly ITokenService _tokenManager;

    public AuthService(ITokenService tokenManager)
    {
        _tokenManager = tokenManager;
    }

    public Task<string> Login(LoginRequest loginUserRequest)
    {
        //TODO LOGİN İŞLEMLERİ GERÇEKLEŞECEK
        return Task.FromResult("Login Sağlandı");
    }

    public Task<string> Register(CreateUserRequest createUserRequest)
    {
        throw new NotImplementedException();
    }
}
