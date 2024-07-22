using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using OunoRentAdminPanel.Models;
using OunoRentAdminPanel.Models.ApiModels;
using OunoRentAdminPanel.Services.Interface;

namespace OunoRentAdminPanel.Services.Concrete;

public class AuthService : IAuthService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IHttpClientFactory _httpClientFactory;

    public AuthService(IHttpContextAccessor httpContextAccessor, IHttpClientFactory httpClientFactory)
    {
        _httpContextAccessor = httpContextAccessor;
        _httpClientFactory = httpClientFactory;
    }

    public async Task<IActionResult> LoginAsync(LoginViewModel loginViewModel)
    {
        var client = _httpClientFactory.CreateClient("ounoRentApi");
        var response = await client.PostAsJsonAsync("auth/login", loginViewModel);

        if (!response.IsSuccessStatusCode)
        {
            return new UnauthorizedObjectResult("Login failed.");
        }

        var content = await response.Content.ReadAsStringAsync();
        var apiConfiguration = JsonConvert.DeserializeObject<ApiConfiguration>(content);

        if (apiConfiguration == null)
        {
            return new UnauthorizedObjectResult("Login failed.");
        }

        if (loginViewModel.RememberMe)
        {
            var cookieOptions = new CookieOptions
            {
                Expires = DateTime.Now.AddDays(30),
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict
            };
            _httpContextAccessor.HttpContext.Response.Cookies.Append("token", apiConfiguration.Token, cookieOptions);
        }

        _httpContextAccessor.HttpContext.Session.SetString("token", apiConfiguration.Token);

        return new OkObjectResult(apiConfiguration);
    }

    public void LogoutAsync()
    {
        _httpContextAccessor.HttpContext.Session.Remove("token");
        _httpContextAccessor.HttpContext.Session.Remove("expireTime");

        if (_httpContextAccessor.HttpContext.Request.Cookies.ContainsKey("token"))
            _httpContextAccessor.HttpContext.Response.Cookies.Delete("token");

    }
}
