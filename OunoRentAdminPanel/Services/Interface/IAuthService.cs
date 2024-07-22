using Microsoft.AspNetCore.Mvc;
using OunoRentAdminPanel.Models;

namespace OunoRentAdminPanel.Services.Interface;

public interface IAuthService
{
    Task<IActionResult> LoginAsync(LoginViewModel loginViewModel);
    void LogoutAsync();
}
