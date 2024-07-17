using Shared.DTO.Login.Request;
using Shared.DTO.Register.Request;
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
}
