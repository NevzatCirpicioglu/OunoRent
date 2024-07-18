using BusinessLayer.Utilities;
using Shared.DTO.Authentication.Request;
using Shared.DTO.User.Request;
using Shared.DTO.User.Response;
using Shared.Interface;

namespace BusinessLayer.Services;

public class AuthService : IAuthService
{
    private readonly ITokenService _tokenService;
    private readonly IUserRepository _userRepository;

    public AuthService(ITokenService tokenService, IUserRepository userRepository)
    {
        _tokenService = tokenService;
        _userRepository = userRepository;
    }

    public async Task<string> LoginAsync(LoginRequest loginRequest)
    {
        var isUserExist = await _userRepository.IsExistAsync(loginRequest.Email);

        if (!isUserExist)
            throw new NullReferenceException("Böyle bir kullanıcı bulunamadı");

        var user = await _userRepository.GetUserByEmail(loginRequest.Email);
        var isVerify = PasswordHasher.VerifyPassword(loginRequest.Password, user.PasswordHash);

        if (!isVerify)
            throw new Exception("Şifre yanlış.");

        var token = await _tokenService.GenerateTokenAsync(user);
        return token;
    }

    public async Task<UserResponse> RegisterAsync(RegisterRequest registerRequest)
    {
        var isUserExist = await _userRepository.IsExistAsync(registerRequest.Email);

        if (isUserExist)
            throw new NullReferenceException("Bu email başka bir kullanıcıya kayıtlı.");

        var hashPassword = PasswordHasher.HashPassword(registerRequest.Password);
        var createUserRequest = new CreateUserRequest(registerRequest.Email, hashPassword);

        var result = await _userRepository.CreateUser(createUserRequest);
        return result;
    }

    public bool ValidateToken(string token)
    {
        var result = _tokenService.GetPrincipal(token);
        return result != null;
    }
}