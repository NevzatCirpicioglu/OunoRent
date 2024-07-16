using EntityLayer.Entities;
using Microsoft.EntityFrameworkCore;
using Shared.DTO.Login.Response;
using Shared.DTO.User.Request;
using Shared.DTO.User.Response;
using Shared.Interface;

namespace DataAccessLayer.Concrete.Repository;

public class UserRepository : IUserRepository
{

    private readonly ApplicationDbContext _applicatiıonDbContext;

    public UserRepository(ApplicationDbContext applicatiıonDbContext)
    {
        _applicatiıonDbContext = applicatiıonDbContext;
    }

    public async Task<UserResponse> CreateUser(CreateUserRequest request)
    {
        var user = new User
        {
            Email = request.Email,
            PasswordHash = request.PasswordHash,
            CreatedDateTime = DateTime.UtcNow,
            ModifiedDateTime = DateTime.UtcNow,
            AccountStatus = "Active",
            CreatedBy = "System",
            ModifiedBy = "System"
        };

        _applicatiıonDbContext.Users.Add(user);

        await _applicatiıonDbContext.SaveChangesAsync();

        return new UserResponse
        {
            Id = user.Id,
            CreatedDateTime = user.CreatedDateTime
        };
    }

    public async Task<GetUserResponse> GetUserById(Guid userId)
    {
        var user = await _applicatiıonDbContext.Users
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
            Address = x.Address
        }).FirstOrDefaultAsync()
        ?? throw new Exception("User not found");

        return user;
    }

    public async Task<List<GetUsersResponse>> GetUsers()
    {
        var users = await _applicatiıonDbContext.Users
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
            Address = x.Address
        }).ToListAsync();

        return users;
    }

    public async Task<bool> IsExistAsync(string email)
    {
        var isExist = await _applicatiıonDbContext.Users.AsNoTracking().AnyAsync(u => u.Email == email);
        return isExist;
    }

    public async Task<UserDetailsResponse> GetUserByEmail(string email)
    {
        var user = await _applicatiıonDbContext.Users
            .AsNoTracking()
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
            ?? throw new Exception("User not found");

        return user;
    }
}
