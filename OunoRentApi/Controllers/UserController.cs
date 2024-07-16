using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusinessLayer.User.Query;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shared.DTO.User.Response;

namespace OunoRentApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly IMediator _mediator;

    public UserController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetUsers()
    {
        var result = await _mediator.Send(new GetUsersQuery());
        return Ok(result);
    }
}
