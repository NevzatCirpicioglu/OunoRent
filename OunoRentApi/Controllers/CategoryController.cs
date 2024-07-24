using BusinessLayer.CQRS.Category.Command;
using BusinessLayer.CQRS.Category.Query;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.DTO.Category.Request;

namespace OunoRentApi.Controllers.CategoryController;

[ApiController]
[Route("api/category")]
public class CategoryController : ControllerBase
{
    private readonly IMediator _mediator;

    public CategoryController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetCategories()
    {
        var categories = await _mediator.Send(new GetCategoriesQuery());
        return Ok(categories);
    }

    [HttpGet("{categoryId:guid}")]
    public async Task<IActionResult> GetCategory(Guid categoryId)
    {
        var category = await _mediator.Send(new GetCategoryQuery(categoryId));
        return Ok(category);
    }

    [HttpPost]
    public async Task<IActionResult> CreateCategory([FromBody] CreateCategoryRequest request)
    {
        var category = await _mediator.Send(new CreateCategoryCommand(request));
        return Ok(category);
    }

    [HttpPut("{categoryId:guid}")]
    public async Task<IActionResult> UpdateCategory([FromBody] UpdateCategoryRequest request)
    {
        var category = await _mediator.Send(new UpdateCategoryCommand(request));
        return Ok(category);
    }

    [HttpDelete("{categoryId:guid}")]
    public async Task<IActionResult> DeleteCategory(Guid categoryId)
    {
        var category = await _mediator.Send(new DeleteCategoryCommand(categoryId));
        return Ok(category);
    }
}