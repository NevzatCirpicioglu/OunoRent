using BusinessLayer.CQRS.Authentication.Command;
using BusinessLayer.CQRS.Authentication.Query;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shared.DTO.Authentication.Request;

namespace OunoRentApi.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;

    public AuthController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequest loginRequest)
    {
        try
        {
            var token = await _mediator.Send(new LoginCommand(loginRequest));
            return Ok(token);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterRequest registerRequest)
    {
        try
        {
            var result = await _mediator.Send(new RegisterCommand(registerRequest));
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("validate-token")]
    public async Task<IActionResult> ValidateToken(ValidateTokenRequest validateTokenRequest)
    {
        try
        {
            var result = await _mediator.Send(new ValidateTokenQuery(validateTokenRequest));

            if (!result.IsValid)
                return Unauthorized();

            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
