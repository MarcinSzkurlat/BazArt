using Application.Dtos.Search;
using Application.Features.Search.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BazArtAPI.Controllers
{
    public class SearchController : BaseApiController
    {
        private readonly IMediator _mediator;

        public SearchController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<SearchResultDto>> SearchItemsAsync([FromQuery] string searchQuery)
        {
            var result = await _mediator.Send(new GetSearchingItemsAsync.Query { SearchQuery = searchQuery });

            return Ok(result);
        }
    }
}
