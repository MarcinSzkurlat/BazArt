using System.Security.Claims;
using Application.Exceptions;
using Application.Interfaces;
using Application.Interfaces.Services;
using MediatR;
using Microsoft.AspNetCore.Http;
using Serilog;

namespace Application.Features.User.Command
{
    public class DeleteUserByIdAsync
    {
        public class Command : IRequest
        {
            public Guid Id { get; set; }
        }

        public class Handler : IRequestHandler<Command>
        {
            private readonly IUserRepository _userRepository;
            private readonly IHttpContextAccessor _contextAccessor;
            private readonly IFavoriteProductsRepository _favoriteProductsRepository;
            private readonly IFavoriteUsersRepository _favoriteUsersRepository;
            private readonly IPhotoService _photoService;

            public Handler(IUserRepository userRepository, IHttpContextAccessor contextAccessor, IFavoriteProductsRepository favoriteProductsRepository, IFavoriteUsersRepository favoriteUsersRepository, IPhotoService photoService)
            {
                _userRepository = userRepository;
                _contextAccessor = contextAccessor;
                _favoriteProductsRepository = favoriteProductsRepository;
                _favoriteUsersRepository = favoriteUsersRepository;
                _photoService = photoService;
            }

            public async Task Handle(Command request, CancellationToken cancellationToken)
            {
                var userToDelete = await _userRepository.GetUserByIdAsync(request.Id);

                if (userToDelete == null) throw new NotFoundException("User with this ID not exist");

                if (request.Id !=
                    Guid.Parse(_contextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)) &&
                    !_contextAccessor.HttpContext.User.IsInRole("Admin"))
                    throw new ForbiddenAccessException("You cannot delete user account");

                await _favoriteProductsRepository.DeleteAllUserFavoritesProductsAsync(userToDelete.Id);
                await _favoriteUsersRepository.DeleteAllUserFavoritesUsersAsync(userToDelete.Id);

                await _photoService.DeletePhotoAsync($"{userToDelete.Id}_Avatar");
                await _photoService.DeletePhotoAsync($"{userToDelete.Id}_BackgroundImage");

                _userRepository.DeleteUserById(userToDelete);

                var result = await _userRepository.SaveChangesAsync();

                if (result == 0) throw new Exception();

                Log.Information($"User ({request.Id}) deleted");
            }
        }
    }
}
