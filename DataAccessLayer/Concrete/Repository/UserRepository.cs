using BusinessLayer.Middlewares;
using EntityLayer.Entities;
using Microsoft.EntityFrameworkCore;
using Shared.DTO.Authentication.Response;
using Shared.DTO.User.Request;
using Shared.DTO.User.Response;
using Shared.Interface;

namespace DataAccessLayer.Concrete.Repository;

public class UserRepository : IUserRepository
{

    private readonly ApplicationDbContext _applicationDbContext;

    public UserRepository(ApplicationDbContext applicationDbContext)
    {
        _applicationDbContext = applicationDbContext;
    }

    #region CreateUser
    public async Task<UserResponse> CreateUser(CreateUserRequest request)
    {
        var user = new User
        {
            Email = request.Email,
            AccountStatus = true,
            PasswordHash = request.PasswordHash,
            //ToDo : Account Status bool olucak
        };

        _applicationDbContext.Users.Add(user);

        await _applicationDbContext.SaveChangesAsync();

        return new UserResponse
        {
            Id = user.Id,
            CreatedDateTime = DateTime.UtcNow
        };
    }
    #endregion

    #region DeleteUser
    public async Task<UserResponse> DeleteUser(Guid userId)
    {
        var deletedUser = _applicationDbContext.Users
        .Where(x => x.Id == userId)
        .FirstOrDefault()
        ?? throw new NotFoundException("User not found");

        _applicationDbContext.Users.Remove(deletedUser);

        await _applicationDbContext.SaveChangesAsync();

        return new UserResponse
        {
            Id = deletedUser.Id,
        };
    }
    #endregion

    #region GetUserById
    public async Task<GetUserResponse> GetUserById(Guid userId)
    {
        var user = await _applicationDbContext.Users
        .AsNoTracking()
        .Where(x => x.Id == userId)
        .Select(x => new GetUserResponse
        {
            Id = x.Id,
            Name = x.Name,
            Surname = x.Surname,
            Email = x.Email,
            PhoneNumber = x.PhoneNumber,
            TC = x.TC,
            BirthDate = x.BirthDate,
            Gender = x.Gender,
            Address = x.Address,
            CreatedDateTime = x.CreatedDateTime,
            ModifiedDateTime = x.ModifiedDateTime
        }).FirstOrDefaultAsync()
        ?? throw new NotFoundException("User not found");

        return user;
    }
    #endregion

    #region IsExistAsync
    public async Task<bool> IsExistAsync(string email)
    {
        var isExist = await _applicationDbContext.Users
        .AsNoTracking()
        .AnyAsync(u => u.Email == email);
        return isExist;
    }
    #endregion

    #region GetUserByEmail
    public async Task<UserDetailsResponse> GetUserByEmail(string email)
    {
        var user = await _applicationDbContext.Users
            .AsNoTracking()
            .Where(x => x.Email == email)
            .Select(x => new UserDetailsResponse
            {
                Id = x.Id,
                Name = x.Name,
                Surname = x.Surname,
                Email = x.Email,
                PhoneNumber = x.PhoneNumber,
                TC = x.TC,
                BirthDate = x.BirthDate,
                Gender = x.Gender,
                Address = x.Address,
                AccountStatus = x.AccountStatus,
                PasswordHash = x.PasswordHash
            }).FirstOrDefaultAsync()
            ?? throw new NotFoundException("User not found");

        return user;
    }
    #endregion

    #region GetUsers
    public async Task<List<GetUsersResponse>> GetUsers()
    {
        var users = await _applicationDbContext.Users
        .AsNoTracking()
        .Select(x => new GetUsersResponse
        {
            Id = x.Id,
            Name = x.Name,
            Surname = x.Surname,
            Email = x.Email,
            PhoneNumber = x.PhoneNumber,
            TC = x.TC,
            BirthDate = x.BirthDate,
            Gender = x.Gender,
            Address = x.Address,
            CreatedDateTime = x.CreatedDateTime,
            ModifiedDateTime = x.ModifiedDateTime
        }).ToListAsync();

        return users;
    }
    #endregion

    #region UpdateUser

    public async Task<UserResponse> UpdateUser(UpdateUserRequest request)
    {
        var userEntity = await _applicationDbContext.Users
        .FirstOrDefaultAsync(x => x.Id == request.Id)
        ?? throw new NotFoundException("User not found");

        userEntity.Name = request.Name;
        userEntity.Surname = request.Surname;
        userEntity.Email = request.Email;
        userEntity.PhoneNumber = request.PhoneNumber;
        userEntity.Address = request.Address;

        _applicationDbContext.Update(userEntity);

        await _applicationDbContext.SaveChangesAsync();

        return new UserResponse
        {
            Id = userEntity.Id,
            CreatedDateTime = userEntity.CreatedDateTime,
            ModifiedDateTime = DateTime.UtcNow
        };
    }
    #endregion
}
