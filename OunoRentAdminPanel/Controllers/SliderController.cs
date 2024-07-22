using Microsoft.AspNetCore.Mvc;
using OunoRentAdminPanel.Utilities.Attributes;

namespace OunoRentAdminPanel.Controllers;

[ServiceFilter(typeof(AuthAttribute))]
[Route("slider")]
public class SliderController : Controller
{
    [HttpGet()] //Tüm Slider'lar - Edit Slider Modal olarak bu sayfada açılacak
    public IActionResult Sliders()
    {
        return View();
    }

    [HttpGet("add-slider")]
    public IActionResult AddSlider()
    {
        return View();
    }
}
