using Application.Exceptions;
using Application.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Application.Features.Cart.Command
{
    public class AddProductToCartAsync
    {
        public class Command : IRequest
        {
            public Guid ProductId { get; set; }
        }

        public class Handler : IRequestHandler<Command>
        {
            private readonly ICartRepository _cartRepository;
            private readonly IHttpContextAccessor _contextAccessor;
            private readonly IProductRepository _productRepository;

            public Handler(ICartRepository cartRepository, IHttpContextAccessor contextAccessor, IProductRepository productRepository)
            {
                _cartRepository = cartRepository;
                _contextAccessor = contextAccessor;
                _productRepository = productRepository;
            }

            public async Task Handle(Command request, CancellationToken cancellationToken)
            {
                var product = await _productRepository.GetProductByIdAsync(request.ProductId);

                if (product == null) throw new NotFoundException("Product with this ID not exist");

                var userId = Guid.Parse(_contextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));

                if (await _cartRepository.UserHasProductAsync(userId, request.ProductId))
                    throw new BadRequestException("You already have this product in cart");

                await _cartRepository.AddProductAsync(userId, request.ProductId);

                await _cartRepository.SaveChangesAsync();
            }
        }
    }
}
