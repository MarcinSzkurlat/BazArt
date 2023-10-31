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

        public EventController(IMediator mediator)
        {
            _mediator = mediator;
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
        public async Task<ActionResult<List<EventDto>>> GetEventsByCategoryAsync([FromQuery]string categoryName)
        {
            var events = await _mediator.Send(new GetEventsByCategoryAsync.Query { CategoryName = categoryName });

            return Ok(events);
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
