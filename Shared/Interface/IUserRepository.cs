using Shared.DTO.Login.Response;
using Shared.DTO.User.Request;
using Shared.DTO.User.Response;

namespace Shared.Interface;

public interface IUserRepository
{
    Task<List<GetUsersResponse>> GetUsers();

    Task<GetUserResponse> GetUserById(Guid userId);

    Task<UserResponse> CreateUser(CreateUserRequest request);

    Task<bool> IsExistAsync(string email);

    Task<UserDetailsResponse> GetUserByEmail(string email);
}
