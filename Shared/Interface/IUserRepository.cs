using Shared.DTO.User.Response;

namespace Shared.Interface;

public interface IUserRepository
{
    Task<List<GetUsersResponse>> GetUsers();
}
