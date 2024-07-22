using Microsoft.AspNetCore.Mvc;

namespace OunoRentAdminPanel.Controllers;

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
