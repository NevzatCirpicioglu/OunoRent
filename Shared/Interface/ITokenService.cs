namespace Shared.Interface;

public interface ITokenService
{
    Task<string> GenerateToken(UserResponse userResponse);
}
