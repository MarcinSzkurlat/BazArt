using Application.Dtos;
using Application.Dtos.Event;
using Application.Dtos.Product;
using Application.Dtos.User;
using Application.Features.Event.Queries;
using Application.Features.Product.Queries;
using Application.Features.User.Command;
using Application.Features.User.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BazArtAPI.Controllers
{
    public class UserController : BaseApiController
    {
        private readonly IMediator _mediator;
        private readonly IConfiguration _config;

        public UserController(IMediator mediator, IConfiguration config)
        {
            _mediator = mediator;
            _config = config;
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<UserDetailDto>> GetUserByIdAsync([FromRoute] Guid id)
        {
            var user = await _mediator.Send(new GetUserByIdAsync.Query { Id = id });

            return Ok(user);
        }

        [HttpGet("{id:guid}/products")]
        public async Task<ActionResult<PaginatedItems<IEnumerable<ProductDto>>>> GetUserProductsAsync([FromRoute] Guid id, [FromQuery]int pageNumber)
        {
            var results = await _mediator.Send(new GetProductsByUserIdAsync.Query
            {
                Id = id, 
                PageNumber = pageNumber,
                PageSize = int.Parse(_config["PageSize"]!)
            });

            return Ok(results);
        }

        [HttpGet("{id:guid}/events")]
        public async Task<ActionResult<PaginatedItems<IEnumerable<EventDto>>>> GetUserEventsAsync([FromRoute] Guid id, [FromQuery]int pageNumber)
        {
            var results = await _mediator.Send(new GetEventsByUserIdAsync.Query
            {
                Id = id, 
                PageNumber = pageNumber,
                PageSize = int.Parse(_config["PageSize"]!)
            });

            return Ok(results);
        }

        [HttpPut]
        public async Task<ActionResult<UserDetailDto>> EditCurrentUserDetailsAsync(
            [FromBody] EditUserDetailsDto editUserDetailsDto)
        {
            var userDto = await _mediator.Send(new EditUserDetailsAsync.Command{EditUserDetailsDto = editUserDetailsDto});

            return Ok(userDto);
        }

        [HttpDelete("{id:guid}")]
        public async Task<ActionResult> DeleteUserByIdAsync([FromRoute]Guid id)
        {
            await _mediator.Send(new DeleteUserByIdAsync.Command
                { Id = id });

            return Ok();
        }
    }
}
