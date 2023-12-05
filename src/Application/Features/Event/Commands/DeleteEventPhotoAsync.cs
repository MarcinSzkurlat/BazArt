using System.Security.Claims;
using Application.Exceptions;
using Application.Interfaces;
using Application.Interfaces.Services;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Serilog;

namespace Application.Features.Event.Commands
{
    public class DeleteEventPhotoAsync
    {
        public class Command : IRequest
        {
            public Guid EventId { get; set; }
        }

        public class Handler : IRequestHandler<Command>
        {
            private readonly IEventRepository _eventRepository;
            private readonly IHttpContextAccessor _contextAccessor;
            private readonly IPhotoService _photoService;
            private readonly IConfiguration _config;

            public Handler(IEventRepository eventRepository, IHttpContextAccessor contextAccessor, IPhotoService photoService, IConfiguration config)
            {
                _eventRepository = eventRepository;
                _contextAccessor = contextAccessor;
                _photoService = photoService;
                _config = config;
            }

            public async Task Handle(Command request, CancellationToken cancellationToken)
            {
                var eventFromDb = await _eventRepository.GetEventByIdAsync(request.EventId);

                if (eventFromDb == null) throw new NotFoundException("Event with this ID not exist");

                if (eventFromDb.CreatedById !=
                    Guid.Parse(_contextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)!)
                    && !_contextAccessor.HttpContext.User.IsInRole("Admin"))
                    throw new ForbiddenAccessException("You cannot delete photo from this event");

                await _photoService.DeletePhotoAsync(request.EventId.ToString());

                eventFromDb.ImageUrl = _config["Images:PlaceHolders:Event"];

                await _eventRepository.SaveChangesAsync();

                Log.Information($"Photo deleted from event (${request.EventId})");
            }
        }
    }
}
