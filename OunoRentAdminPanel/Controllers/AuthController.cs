using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace OunoRentAdminPanel.Controllers;

[Route("auth")]
public class AuthController : Controller
{

    [HttpGet("login")]
    public IActionResult Login()
    {
        return View();
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
