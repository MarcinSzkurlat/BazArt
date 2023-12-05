using Application.Exceptions;
using Application.Interfaces.Services;
using Application.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Http;
using Serilog;
using System.Security.Claims;

namespace Application.Features.User.Command
{
    public class AddBackgroundImageAsync
    {
        public class Command : IRequest
        {
            public Guid UserId { get; set; }
            public IFormFile File { get; set; }
            public int PhotoHeight { get; set; }
            public int PhotoWidth { get; set; }
        }

        public class Handler : IRequestHandler<Command>
        {
            private readonly IUserRepository _userRepository;
            private readonly IHttpContextAccessor _contextAccessor;
            private readonly IPhotoService _photoService;

            public Handler(IUserRepository userRepository, IHttpContextAccessor contextAccessor, IPhotoService photoService)
            {
                _userRepository = userRepository;
                _contextAccessor = contextAccessor;
                _photoService = photoService;
            }

            public async Task Handle(Command request, CancellationToken cancellationToken)
            {
                request.PhotoHeight = request.PhotoHeight == 0 ? 320 : request.PhotoHeight;
                request.PhotoWidth = request.PhotoWidth == 0 ? 640 : request.PhotoWidth;

                var user = await _userRepository.GetUserByIdAsync(request.UserId);

                if (user == null) throw new NotFoundException("User with this ID not exist");

                if (user.Id !=
                    Guid.Parse(_contextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)!)
                    && !_contextAccessor.HttpContext.User.IsInRole("Admin"))
                    throw new ForbiddenAccessException("You cannot add this photo as user background image");

                var publicPhotoId = $"{request.UserId}_BackgroundImage";

                await _photoService.DeletePhotoAsync(publicPhotoId);

                user.BackgroundImage = await _photoService.AddPhotoAsync(request.File, publicPhotoId, request.PhotoHeight, request.PhotoWidth, "Users");

                await _userRepository.SaveChangesAsync();

                Log.Information($"Photo added as user background image ({request.UserId})");
            }
        }
    }
}
