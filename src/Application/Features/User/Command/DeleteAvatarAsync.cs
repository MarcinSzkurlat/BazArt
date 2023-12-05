using System.Security.Claims;
using Application.Exceptions;
using Application.Interfaces;
using Application.Interfaces.Services;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Serilog;

namespace Application.Features.User.Command
{
    public class DeleteAvatarAsync
    {
        public class Command : IRequest
        {
            public Guid Id { get; set; }
        }

        public class Handler : IRequestHandler<Command>
        {
            private readonly IUserRepository _userRepository;
            private readonly IHttpContextAccessor _contextAccessor;
            private readonly IPhotoService _photoService;
            private readonly IConfiguration _config;

            public Handler(IUserRepository userRepository, IHttpContextAccessor contextAccessor, IPhotoService photoService, IConfiguration config)
            {
                _userRepository = userRepository;
                _contextAccessor = contextAccessor;
                _photoService = photoService;
                _config = config;
            }

            public async Task Handle(Command request, CancellationToken cancellationToken)
            {
                var user = await _userRepository.GetUserByIdAsync(request.Id);

                if (user == null) throw new NotFoundException("User with this ID not exist");

                if (user.Id!=
                    Guid.Parse(_contextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)!)
                    && !_contextAccessor.HttpContext.User.IsInRole("Admin"))
                    throw new ForbiddenAccessException("You cannot delete photo from user avatar");

                var publicPhotoId = $"{user.Id}_Avatar";

                await _photoService.DeletePhotoAsync(publicPhotoId);

                user.Avatar = _config["Images:PlaceHolders:User:Avatar"];

                await _userRepository.SaveChangesAsync();

                Log.Information($"Photo deleted from user avatar (${request.Id})");
            }
        }
    }
}
