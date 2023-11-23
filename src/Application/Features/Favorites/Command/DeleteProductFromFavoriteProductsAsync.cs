using Application.Exceptions;
using Application.Interfaces;
using MediatR;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace Application.Features.Favorites.Command
{
    public class DeleteProductFromFavoriteProductsAsync
    {
        public class Command : IRequest
        {
            public Guid ProductId { get; set; }
        }

        public class Handler : IRequestHandler<Command>
        {
            private readonly IFavoriteProductsRepository _favoriteProductsRepository;
            private readonly IProductRepository _productRepository;
            private readonly IHttpContextAccessor _contextAccessor;

            public Handler(IFavoriteProductsRepository favoriteProductsRepository, IProductRepository productRepository, IHttpContextAccessor contextAccessor)
            {
                _favoriteProductsRepository = favoriteProductsRepository;
                _productRepository = productRepository;
                _contextAccessor = contextAccessor;
            }

            public async Task Handle(Command request, CancellationToken cancellationToken)
            {
                var product = await _productRepository.GetProductByIdAsync(request.ProductId);

                if (product == null) throw new NotFoundException("Product with this ID not exist");

                var userId = Guid.Parse(_contextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));

                if (!await _favoriteProductsRepository.UserHasFavoriteProductAsync(userId, request.ProductId))
                    throw new BadRequestException("You don't have this product in favorites");

                await _favoriteProductsRepository.DeleteFavoriteProductAsync(userId, request.ProductId);

                await _favoriteProductsRepository.SaveChangesAsync();
            }
        }
    }
}
