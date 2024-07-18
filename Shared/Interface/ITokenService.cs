using System.Security.Claims;
using Shared.DTO.Login.Response;

namespace Shared.Interface;

public interface ITokenService
{
    /// <summary>
    /// Asenkron olarak kullanıcı detaylarına göre JWT token üretir.
    /// </summary>
    /// <param name="userDetailResponse">Kullanıcı detaylarını içeren cevap nesnesi.</param>
    /// <returns>Üretilen JWT token string olarak döner.</returns>
    Task<string> GenerateTokenAsync(UserDetailsResponse userDetailsResponse);

    /// <summary>
    /// Verilen JWT token'ı doğrular ve ClaimsPrincipal nesnesi döner.
    /// </summary>
    /// <param name="token">Doğrulanacak JWT token.</param>
    /// <returns>Doğrulanmış token'a ait ClaimsPrincipal nesnesi veya geçersiz token durumunda null döner.</returns>
    /// <exception cref="SecurityTokenException">Token geçersiz olduğunda fırlatılır.</exception>
    ClaimsPrincipal? GetPrincipal(string token);

    /// <summary>
    /// Asenkron olarak verilen JWT token'ı yeniler.
    /// </summary>
    /// <param name="token">Yenilenmesi istenen JWT token.</param>
    /// <returns>Yenilenmiş JWT token veya geçersiz token durumunda null döner.</returns>
    Task<string?> RefreshTokenAsync(string token);
}
