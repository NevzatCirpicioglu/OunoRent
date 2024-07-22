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
    [HttpGet("editCategory/{id}")]
    public async Task<IActionResult> EditCategory(Guid id)
    {
        var client = _httpClientFactory.CreateClient("ounoRentApi");
        var response = await client.GetAsync($"category/{id}");

        if (response.IsSuccessStatusCode)
        {
            var category = await response.Content.ReadFromJsonAsync<CategoryViewModel>();
            return View(category);
        }

        return RedirectToAction("Index");
    }

    // [HttpPost("editCategory")]
    // public async Task<IActionResult> EditCategory(CategoryViewModel model)
    // {
    //     if (!ModelState.IsValid)
    //     {
    //         return View(model);
    //     }

    //     var client = _httpClientFactory.CreateClient("ounoRentApi");
    //     var response = await client.PutAsJsonAsync($"category/{model.categoryId}", model);

    //     if (response.IsSuccessStatusCode)
    //     {
    //         return RedirectToAction("Index");
    //     }

    //     return View(model);
    // }

}
