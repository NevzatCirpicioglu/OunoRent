using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shared.DTO.User.Request;

public sealed record UpdateUserRequest(
    Guid Id,
    string Name,
    string Surname,
    string Email,
    string PhoneNumber,
    string Address
);

