using Application.Exceptions;
using Application.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Application.Features.Favorites.Command
{
    public class DeleteUserFromFavoriteUsersAsync
    {
        public class Command : IRequest
        {
            public Guid FavoriteUserId { get; set; }
        }

        public class Handler : IRequestHandler<Command>
        {
            private readonly IFavoriteUsersRepository _favoriteUsersRepository;
            private readonly IUserRepository _userRepository;
            private readonly IHttpContextAccessor _contextAccessor;

            public Handler(IFavoriteUsersRepository favoriteUsersRepository, IUserRepository userRepository, IHttpContextAccessor contextAccessor)
            {
                _favoriteUsersRepository = favoriteUsersRepository;
                _userRepository = userRepository;
                _contextAccessor = contextAccessor;
            }

            public async Task Handle(Command request, CancellationToken cancellationToken)
            {
                var user = await _userRepository.GetUserByIdAsync(request.FavoriteUserId);

                if (user == null) throw new NotFoundException("User with this ID not exist");

                var userId = Guid.Parse(_contextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));

                if (!await _favoriteUsersRepository.UserHasFavoriteUserAsync(userId, request.FavoriteUserId))
                    throw new BadRequestException("You don't have this user in favorites");

                await _favoriteUsersRepository.DeleteFavoriteUserAsync(userId, request.FavoriteUserId);

                await _favoriteUsersRepository.SaveChangesAsync();
            }
        }
    }
}
