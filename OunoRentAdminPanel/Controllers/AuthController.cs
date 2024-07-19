using Microsoft.AspNetCore.Mvc;
using OunoRentAdminPanel.Models;
using OunoRentAdminPanel.Services.Interface;

namespace OunoRentAdminPanel.Controllers;

[Route("auth")]
public class AuthController : Controller
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpGet("login")]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginViewModel loginViewModel)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _authService.LoginAsync(loginViewModel);
        return result;
    }

    [HttpGet("logout")]
    public IActionResult LogoutAsync()
    {
        _authService.LogoutAsync();
        return RedirectToAction("login");
    }

    [HttpGet("forget-password")]
    public IActionResult ForgetPassword()
    {
        return View();
    }

    [HttpGet("reset-password")]
    public IActionResult ResetPassword()
    {
        return View();
    }
}
