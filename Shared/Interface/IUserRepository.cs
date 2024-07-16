using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Shared.DTO.User.Request;
using Shared.DTO.User.Response;

namespace Shared.Interface;

public interface IUserRepository
{
    Task<List<GetUsersResponse>> GetUsers();

    Task<GetUserResponse> GetUserById(Guid userId);

    Task<UserResponse> CreateUser(CreateUserRequest request);
}
