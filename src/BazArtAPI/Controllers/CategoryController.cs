using Application.Dtos.Category;
using Application.Features.Category.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BazArtAPI.Controllers
{
    public class CategoryController : BaseApiController
    {
        private readonly IMediator _mediator;

        public CategoryController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoryDto>>> GetCategoriesAsync()
        {
            var categories = await _mediator.Send(new GetCategoriesAsync.Query());

            return Ok(categories);
        }

        [AllowAnonymous]
        [HttpGet("{name}")]
        public async Task<ActionResult<CategoryDto>> GetCategoryByNameAsync([FromRoute] string name)
        {
            var category = await _mediator.Send(new GetCategoryByNameAsync.Query { Name = name });

            return Ok(category);
        }
    }
}
