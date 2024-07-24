using BusinessLayer.CQRS.Blog.Command;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shared.DTO.Blog.Request;

namespace OunoRentApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BlogController : ControllerBase
{
    private readonly IMediator _mediator;

    public BlogController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await _mediator.Send(new DeleteBlogCommand(id));

        return Ok(result);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(UpdateBlogRequest updateBlogRequest)
    {
        var result = await _mediator.Send(new UpdateBlogCommand(updateBlogRequest));

        return Ok(result);
    }
}
