using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusinessLayer.CQRS.Slider.Command;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.DTO.Slider.Request;

namespace OunoRentApi.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class SliderController : ControllerBase
{
    private readonly IMediator _mediator;

    public SliderController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> CreateSlider([FromBody] CreateSliderRequest request)
    {
        var slider = await _mediator.Send(new CreateSliderCommand(request));
        
        return Ok(slider);
    }
}
