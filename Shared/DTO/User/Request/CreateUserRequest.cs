namespace Shared.DTO.User.Request;

public sealed record CreateUserRequest(
    string Name, string Surname, string Email,
    string PhoneNumber, string TC, DateTime BirthDate, string Gender,
    string Address, string AccountStatus, string PasswordHash);

