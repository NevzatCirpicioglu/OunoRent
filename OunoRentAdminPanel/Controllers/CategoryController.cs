using Microsoft.AspNetCore.Mvc;

namespace OunoRentAdminPanel.Controllers;

[Route("category")]
public class CategoryController : Controller
{
    [HttpGet()]
    public IActionResult Index()
    {
        return View();
    }

    [HttpGet("add-category")]
    public IActionResult AddCategory()
    {
        return View();
    }
}
