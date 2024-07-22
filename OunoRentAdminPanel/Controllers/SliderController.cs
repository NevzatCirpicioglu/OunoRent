using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace OunoRentAdminPanel.Controllers;

[Route("[controller]")]
public class SliderController : Controller
{
    private readonly ILogger<SliderController> _logger;

    public SliderController(ILogger<SliderController> logger)
    {
        _logger = logger;
    }
    
    [HttpGet("AddSlider")] 
    public IActionResult AddSlider()
    {
        return View();
    }

    [HttpGet("Sliders")] //Tüm Slider'lar - Edit Slider Modal olarak bu sayfada açılacak
    public IActionResult Sliders()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View("Error!");
    }
}
