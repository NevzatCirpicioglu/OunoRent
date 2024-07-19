using Microsoft.AspNetCore.Mvc;
using OunoRentAdminPanel.Models;
using OunoRentAdminPanel.Utilities.Attributes;

namespace OunoRentAdminPanel.Controllers;

[ServiceFilter(typeof(AuthAttribute))]
[Route("[controller]")]
public class CategoryController : Controller
{
    private readonly IHttpClientFactory _httpClientFactory;

    public CategoryController(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    [HttpGet("Index")]
    public async Task<IActionResult> Index()
    {
        var client = _httpClientFactory.CreateClient("ounoRentApi");
        var response = await client.GetFromJsonAsync<List<CategoryViewModel>>("category");

        var token = HttpContext.Session.GetString("token");
        Console.WriteLine(token);
        return View(response);
    }
}
