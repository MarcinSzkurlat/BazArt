using Application.Dtos.Event;
using Application.Dtos.Product;
using Application.Dtos.User;
using Application.Features.Event.Queries;
using Application.Features.Product.Queries;
using Application.Features.User.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BazArtAPI.Controllers
{
    public class UserController : BaseApiController
    {
        private readonly IMediator _mediator;

        public UserController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<UserDetailDto>> GetUserByIdAsync([FromRoute] Guid id)
        {
            var user = await _mediator.Send(new GetUserByIdAsync.Query { Id = id });

            return Ok(user);
        }

        [HttpGet("{id:guid}/products")]
        public async Task<ActionResult<List<ProductDto>>> GetUserProductsAsync([FromRoute] Guid id)
        {
            var products = await _mediator.Send(new GetProductsByUserIdAsync.Query{Id = id});

            return Ok(products);
        }

        [HttpGet("{id:guid}/events")]
        public async Task<ActionResult<List<EventDto>>> GetUserEventsAsync([FromRoute] Guid id)
        {
            var events = await _mediator.Send(new GetEventsByUserIdAsync.Query{Id = id});

            return Ok(events);
        }
    }
}
