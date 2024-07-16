using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusinessLayer.Category.Command;
using BusinessLayer.Category.Query;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shared.DTO.Category.Request;

namespace OunoRentApi.Controllers.CategoryController
{
    [ApiController]
    [Route("api/[controller]")]
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

        [HttpGet("{categoryId}")]
        public async Task<IActionResult> GetCategory(int categoryId)
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
    }
}