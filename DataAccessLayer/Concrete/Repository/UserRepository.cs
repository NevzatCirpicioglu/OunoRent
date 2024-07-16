using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
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

    public async Task<List<GetUsersResponse>> GetUsers()
    {
        var users = await _applicatiıonDbContext.Users
        .Select(x=> new GetUsersResponse
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
}
