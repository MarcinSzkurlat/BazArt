using MediatR;
using System.Security.Claims;
using Application.Exceptions;
using Application.Interfaces;
using Microsoft.AspNetCore.Http;

namespace Application.Features.Favorites.Command
{
    public class AddUserToFavoriteUsersAsync
    {
        public class Command : IRequest
        {
            public Guid FavoriteUserId { get; set; }
        }

        public class Handler : IRequestHandler<Command>
        {
            private readonly IFavoriteUsersRepository _favoriteUsersRepository;
            private readonly IHttpContextAccessor _contextAccessor;
            private readonly IUserRepository _userRepository;

            public Handler(IFavoriteUsersRepository favoriteUsersRepository, IHttpContextAccessor contextAccessor, IUserRepository userRepository)
            {
                _favoriteUsersRepository = favoriteUsersRepository;
                _contextAccessor = contextAccessor;
                _userRepository = userRepository;
            }

            public async Task Handle(Command request, CancellationToken cancellationToken)
            {
                var user = await _userRepository.GetUserByIdAsync(request.FavoriteUserId);

                if (user == null) throw new NotFoundException("User with this ID not exist");

                var userId = Guid.Parse(_contextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));

                if (await _favoriteUsersRepository.UserHasFavoriteUserAsync(userId, request.FavoriteUserId))
                    throw new BadRequestException("You already have this user in favorites");

                await _favoriteUsersRepository.AddFavoriteUserAsync(userId, request.FavoriteUserId);

                await _favoriteUsersRepository.SaveChangesAsync();
            }
        }
    }
}
