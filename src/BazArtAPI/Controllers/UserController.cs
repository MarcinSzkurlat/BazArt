using Application.Dtos;
using Application.Dtos.Event;
using Application.Dtos.Product;
using Application.Dtos.User;
using Application.Features.Cart.Command;
using Application.Features.Cart.Queries;
using Application.Features.Event.Queries;
using Application.Features.Favorites.Command;
using Application.Features.Favorites.Queries;
using Application.Features.Product.Queries;
using Application.Features.User.Command;
using Application.Features.User.Queries;
using BazArtAPI.Dtos.User;
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
        public async Task<ActionResult<PaginatedItems<IEnumerable<ProductDto>>>> GetUserProductsAsync([FromRoute] Guid id, [FromQuery]int pageNumber = 1)
        {
            int.TryParse(_config["PageSize"], out int pageSize);

            var results = await _mediator.Send(new GetProductsByUserIdAsync.Query
            {
                Id = id, 
                PageNumber = pageNumber,
                PageSize = pageSize
            });

            return Ok(results);
        }

        [HttpGet("{id:guid}/events")]
        public async Task<ActionResult<PaginatedItems<IEnumerable<EventDto>>>> GetUserEventsAsync([FromRoute] Guid id, [FromQuery]int pageNumber = 1)
        {
            int.TryParse(_config["PageSize"], out int pageSize);

            var results = await _mediator.Send(new GetEventsByUserIdAsync.Query
            {
                Id = id, 
                PageNumber = pageNumber,
                PageSize = pageSize
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

        [HttpGet("favorites/product")]
        public async Task<ActionResult<PaginatedItems<IEnumerable<ProductDto>>>> GetUserFavoriteProductsAsync([FromQuery]int pageNumber = 1)
        {
            int.TryParse(_config["PageSize"], out int pageSize);

            var results = await _mediator.Send(new GetFavoritesProductsAsync.Query
            {
                PageNumber = pageNumber,
                PageSize = pageSize
            });

            return Ok(results);
        }

        [HttpPost("favorites/product/{id:guid}")]
        public async Task<ActionResult> AddProductToUserFavoriteProductsAsync([FromRoute] Guid id)
        {
            await _mediator.Send(new AddProductToFavoriteProductsAsync.Command { ProductId = id });

            return Ok();
        }

        [HttpDelete("favorites/product/{id:guid}")]
        public async Task<ActionResult> DeleteProductFromFavoriteProductsAsync([FromRoute] Guid id)
        {
            await _mediator.Send(new DeleteProductFromFavoriteProductsAsync.Command { ProductId = id });

            return Ok();
        }

        [HttpGet("favorites/user")]
        public async Task<ActionResult<PaginatedItems<IEnumerable<UserDto>>>> GetUserFavoriteUsersAsync(
            [FromQuery] int pageNumber = 1)
        {
            int.TryParse(_config["PageSize"], out int pageSize);

            var results = await _mediator.Send(new GetFavoritesUsersAsync.Query
            {
                PageNumber = pageNumber,
                PageSize = pageSize
            });

            return Ok(results);
        }

        [HttpPost("favorites/user/{id:guid}")]
        public async Task<ActionResult> AddUserToUserFavoriteUserAsync([FromRoute] Guid id)
        {
            await _mediator.Send(new AddUserToFavoriteUsersAsync.Command { FavoriteUserId = id });

            return Ok();
        }

        [HttpDelete("favorites/user/{id:guid}")]
        public async Task<ActionResult> DeleteUserFromFavoriteUsersAsync([FromRoute] Guid id)
        {
            await _mediator.Send(new DeleteUserFromFavoriteUsersAsync.Command { FavoriteUserId = id });

            return Ok();
        }

        [HttpGet("cart")]
        public async Task<ActionResult<PaginatedItems<IEnumerable<ProductDto>>>> GetUserCartAsync()
        {
            var results = await _mediator.Send(new GetUserCartAsync.Query());

            return Ok(results);
        }

        [HttpPost("cart/{id:guid}")]
        public async Task<ActionResult> AddProductToCartAsync([FromRoute] Guid id)
        {
            await _mediator.Send(new AddProductToCartAsync.Command { ProductId = id });

            return Ok();
        }

        [HttpDelete("cart/{id:guid}")]
        public async Task<ActionResult> DeleteProductFromCartAsync([FromRoute] Guid id)
        {
            await _mediator.Send(new DeleteProductFromCartAsync.Command { ProductId = id });

            return Ok();
        }

        [HttpPost("{id:guid}/photo/avatar")]
        public async Task<IActionResult> AddAvatarAsync([FromRoute] Guid id, [FromForm] IFormFile file)
        {
            int.TryParse(_config["Images:Settings:User:Avatar:Height"], out int photoHeight);
            int.TryParse(_config["Images:Settings:User:Avatar:Width"], out int photoWidth);

            await _mediator.Send(new AddAvatarAsync.Command
            {
                UserId = id,
                File = file,
                PhotoHeight = photoHeight,
                PhotoWidth = photoWidth
            });

            return Ok();
        }

        [HttpDelete("{id:guid}/photo/avatar")]
        public async Task<IActionResult> DeleteAvatarAsync([FromRoute] Guid id)
        {
            await _mediator.Send(new DeleteAvatarAsync.Command{Id = id});

            return Ok();
        }

        [HttpPost("{id:guid}/photo/background")]
        public async Task<IActionResult> AddBackgroundImageAsync([FromRoute] Guid id, [FromForm] IFormFile file)
        {
            int.TryParse(_config["Images:Settings:User:BackgroundImage:Height"], out int photoHeight);
            int.TryParse(_config["Images:Settings:User:BackgroundImage:Width"], out int photoWidth);

            await _mediator.Send(new AddBackgroundImageAsync.Command
            {
                UserId = id,
                File = file,
                PhotoHeight = photoHeight,
                PhotoWidth = photoWidth
            });

            return Ok();
        }

        [HttpDelete("{id:guid}/photo/background")]
        public async Task<IActionResult> DeleteBackgroundImageAsync([FromRoute] Guid id)
        {
            await _mediator.Send(new DeleteBackgroundImageAsync.Command { Id = id });

            return Ok();
        }
    }
}
