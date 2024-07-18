using Shared.DTO.Authentication.Request;
using Shared.DTO.User.Response;

namespace Shared.Interface;

public interface IAuthService
{
    /// <summary>
    /// Asenkron olarak yeni kullanıcı kayıt işlemi yapar.
    /// </summary>
    /// <param name="registerRequest">Kullanıcı kayıt bilgilerini içeren istek nesnesi.</param>
    /// <returns>Başarılı kayıt sonrası kullanıcı bilgilerini döner.</returns>
    /// <exception cref="NullReferenceException">Email başka bir kullanıcıya kayıtlı olduğunda fırlatılır.</exception>
    Task<UserResponse> RegisterAsync(RegisterRequest registerRequest);

    /// <summary>
    /// Asenkron olarak kullanıcı giriş işlemi yapar.
    /// </summary>
    /// <param name="loginRequest">Kullanıcı giriş bilgilerini içeren istek nesnesi.</param>
    /// <returns>Başarılı giriş sonrası JWT token döner.</returns>
    /// <exception cref="NullReferenceException">Kullanıcı bulunamadığında fırlatılır.</exception>
    /// <exception cref="Exception">Şifre yanlış olduğunda fırlatılır.</exception>
    Task<string> LoginAsync(LoginRequest loginRequest);

    /// <summary>
    /// Validates the given token and checks if it is valid.
    /// </summary>
    /// <param name="token">The token to be validated.</param>
    /// <returns>
    /// <c>true</c> if the token is valid; otherwise, <c>false</c>.
    /// </returns>
    /// <remarks>
    /// This method uses the _tokenService to retrieve the principal associated with the token.
    /// If a principal is found, the token is considered valid.
    /// </remarks>
    public bool ValidateToken(string token);
}
