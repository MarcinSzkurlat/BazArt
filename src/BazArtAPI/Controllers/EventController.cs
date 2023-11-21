using Application.Dtos;
using Application.Dtos.Event;
using Application.Features.Event.Commands;
using Application.Features.Event.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BazArtAPI.Controllers
{
    public class EventController : BaseApiController
    {
        private readonly IMediator _mediator;
        private readonly IConfiguration _config;

        public EventController(IMediator mediator, IConfiguration config)
        {
            _mediator = mediator;
            _config = config;
        }

        [AllowAnonymous]
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<EventDetailsDto>> GetEventByIdAsync([FromRoute]Guid id)
        {
            var eventDto = await _mediator.Send(new GetEventByIdAsync.Query { Id = id });

            return Ok(eventDto);
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<PaginatedItems<IEnumerable<EventDto>>>> GetEventsByCategoryAsync([FromQuery]string categoryName, [FromQuery]int pageNumber = 1)
        {
            var results = await _mediator.Send(new GetEventsByCategoryAsync.Query
            {
                CategoryName = categoryName, 
                PageNumber = pageNumber, 
                PageSize = int.Parse(_config["PageSize"]!)
            });

            return Ok(results);
        }

        [HttpPost]
        public async Task<ActionResult<EventDetailsDto>> CreateEventAsync([FromBody]CreateEventDto eventToCreate)
        {
            var eventDto = await _mediator.Send(new CreateEventAsync.Command { CreateEventDto = eventToCreate });

            return Created($"api/event/{eventToCreate.Id}", eventDto);
        }

        [HttpPut("{id:guid}")]
        public async Task<ActionResult<EventDetailsDto>> EditEventAsync([FromRoute]Guid id, [FromBody]EditEventDto eventToEdit)
        {
            var eventDto = await _mediator.Send(new EditEventAsync.Command {Id = id, EditEventDto = eventToEdit });

            return Ok(eventDto);
        }

        [HttpDelete("{id:guid}")]
        public async Task<ActionResult> DeleteEventAsync([FromRoute]Guid id)
        {
            await _mediator.Send(new DeleteEventAsync.Command { Id = id });

            return Ok();
        }

        [AllowAnonymous]
        [HttpGet("latest")]
        public async Task<ActionResult<IEnumerable<EventDto>>> GetEventsByCreatedDate()
        {
            var events = await _mediator.Send(new GetEventsByCreateDate.Query());
            
            return Ok(events);
        }
    }
}
