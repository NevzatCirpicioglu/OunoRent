using Microsoft.AspNetCore.Mvc;
using OunoRentAdminPanel.Models.EntityModels;
using OunoRentAdminPanel.Models.EntityModels.CategoryModel;
using OunoRentAdminPanel.Utilities.Attributes;

namespace OunoRentAdminPanel.Controllers;

[ServiceFilter(typeof(AuthAttribute))]
[Route("category")]
public class CategoryController : Controller
{

    private readonly IHttpClientFactory _httpClientFactory;

    public CategoryController(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    [HttpGet()]
    public async Task<IActionResult> Index()
    {
        var client = _httpClientFactory.CreateClient("ounoRentApi");
        
        var response = await client.GetAsync("category");

        if (response.IsSuccessStatusCode)
        {
            var categories = await response.Content.ReadFromJsonAsync<List<CategoryViewModel>>();
            return View(categories);
        }

        return View();
    }

    [HttpGet("addCategory")]
    public IActionResult AddCategory()
    {
        return View();
    }

    [HttpPost("addCategory")]
    public async Task<IActionResult> AddCategory(CreateCategoryModel model)
    {
        if (ModelState.IsValid)
        {
            var client = _httpClientFactory.CreateClient("ounoRentApi");

            var response = await client.PostAsJsonAsync("category", model);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }

            return View(model);
        }
        else
        {
            ModelState.AddModelError(string.Empty, "Kategori eklenirken bir hata oluştu.");

            return View(model);
        }
    }
}
