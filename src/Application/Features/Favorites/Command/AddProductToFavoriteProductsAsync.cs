using Application.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using Application.Exceptions;

namespace Application.Features.Favorites.Command
{
    public class AddProductToFavoriteProductsAsync
    {
        public class Command : IRequest
        {
            public Guid ProductId { get; set; }
        }

        public class Handler : IRequestHandler<Command>
        {
            private readonly IFavoriteProductsRepository _favoriteProductsRepository;
            private readonly IHttpContextAccessor _contextAccessor;
            private readonly IProductRepository _productRepository;

            public Handler(IFavoriteProductsRepository favoriteProductsRepository, IHttpContextAccessor contextAccessor, IProductRepository productRepository)
            {
                _favoriteProductsRepository = favoriteProductsRepository;
                _contextAccessor = contextAccessor;
                _productRepository = productRepository;
            }

            public async Task Handle(Command request, CancellationToken cancellationToken)
            {
                var product = await _productRepository.GetProductByIdAsync(request.ProductId);

                if (product == null) throw new NotFoundException("Product with this ID not exist");

                var userId = Guid.Parse(_contextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));

                if (await _favoriteProductsRepository.UserHasFavoriteProductAsync(userId, request.ProductId))
                    throw new BadRequestException("You already have this product in favorites");

                await _favoriteProductsRepository.AddFavoriteProductAsync(userId, request.ProductId);

                await _favoriteProductsRepository.SaveChangesAsync();
            }
        }
    }
}
