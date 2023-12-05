using System.Security.Claims;
using Application.Exceptions;
using Application.Interfaces;
using Application.Interfaces.Services;
using MediatR;
using Microsoft.AspNetCore.Http;
using Serilog;

namespace Application.Features.Event.Commands
{
    public class AddEventPhotoAsync
    {
        public class Command : IRequest
        {
            public Guid EventId { get; set; }
            public IFormFile File { get; set; }
            public int PhotoHeight { get; set; }
            public int PhotoWidth { get; set; }
        }

        public class Handler : IRequestHandler<Command>
        {
            private readonly IEventRepository _eventRepository;
            private readonly IHttpContextAccessor _contextAccessor;
            private readonly IPhotoService _photoService;

            public Handler(IEventRepository eventRepository, IHttpContextAccessor contextAccessor, IPhotoService photoService)
            {
                _eventRepository = eventRepository;
                _contextAccessor = contextAccessor;
                _photoService = photoService;
            }

            public async Task Handle(Command request, CancellationToken cancellationToken)
            {
                request.PhotoHeight = request.PhotoHeight == 0 ? 480 : request.PhotoHeight;
                request.PhotoWidth = request.PhotoWidth == 0 ? 640 : request.PhotoWidth;

                var eventFromDb = await _eventRepository.GetEventByIdAsync(request.EventId);

                if (eventFromDb == null) throw new NotFoundException("Event with this ID not exist");

                if (eventFromDb.CreatedById !=
                    Guid.Parse(_contextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)!)
                    && !_contextAccessor.HttpContext.User.IsInRole("Admin"))
                    throw new ForbiddenAccessException("You cannot add photo to this event");

                await _photoService.DeletePhotoAsync(request.EventId.ToString());

                eventFromDb.ImageUrl = await _photoService.AddPhotoAsync(request.File, request.EventId.ToString(), request.PhotoHeight, request.PhotoWidth, "Images");

                await _eventRepository.SaveChangesAsync();

                Log.Information($"Photo added to event ({request.EventId})");
            }
        }
    }
}
