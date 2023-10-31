using System.Security.Claims;
using Application.Exceptions;
using Application.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Http;
using Serilog;

namespace Application.Features.Event.Commands
{
    public class DeleteEventAsync
    {
        public class Command : IRequest
        {
            public Guid Id { get; set; }
        }

        public class Handler : IRequestHandler<Command>
        {
            private readonly IEventRepository _eventRepository;
            private readonly IHttpContextAccessor _contextAccessor;

            public Handler(IEventRepository eventRepository, IHttpContextAccessor contextAccessor)
            {
                _eventRepository = eventRepository;
                _contextAccessor = contextAccessor;
            }

            public async Task Handle(Command request, CancellationToken cancellationToken)
            {
                var eventToDelete = await _eventRepository.GetEventByIdAsync(request.Id);

                if (eventToDelete == null) throw new NotFoundException("Event with this ID not exist");

                if (eventToDelete.CreatedById !=
                    Guid.Parse(_contextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)!)
                    && !_contextAccessor.HttpContext.User.IsInRole("Admin"))
                    throw new ForbiddenAccessException("You cannot edit this product");

                _eventRepository.DeleteEvent(eventToDelete);

                var result = await _eventRepository.SaveChangesAsync();

                if (result == 0) throw new Exception();

                Log.Information($"Event ({request.Id}) deleted");
            }
        }
    }
}
