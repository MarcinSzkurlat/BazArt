using Application.Dtos.Event;
using Application.Event.Commands;
using Application.Event.Queries;
using Domain.Models;
using MediatR;
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

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<EventDetailsDto>> GetEventByIdAsync([FromRoute]Guid id)
        {
            var eventDto = await _mediator.Send(new GetEventByIdAsync.Query { Id = id });

            return Ok(eventDto);
        }

        [HttpGet]
        public async Task<ActionResult<List<EventDto>>> GetEventsByCategoryAsync([FromQuery]Categories categoryName)
        {
            var events = await _mediator.Send(new GetEventsByCategoryAsync.Query { CategoryName = categoryName });

            return Ok(events);
        }

        [HttpPost]
        public async Task<ActionResult> CreateEventAsync([FromBody]CreateEventDto eventToCreate)
        {
            await _mediator.Send(new CreateEventAsync.Command { CreateEventDto = eventToCreate });

            return Created($"api/event/{eventToCreate.Id}", null);
        }

        [HttpPut("{id:guid}")]
        public async Task<ActionResult> EditEventAsync([FromRoute]Guid id, [FromBody]EditEventDto eventToEdit)
        {
            await _mediator.Send(new EditEventAsync.Command {Id = id, EditEventDto = eventToEdit });

            return Ok();
        }

        [HttpDelete("{id:guid}")]
        public async Task<ActionResult> DeleteEventAsync([FromRoute]Guid id)
        {
            await _mediator.Send(new DeleteEventAsync.Command { Id = id });

            return Ok();
        }
    }
}
