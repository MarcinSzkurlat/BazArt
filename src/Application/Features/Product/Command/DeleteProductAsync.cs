using System.Security.Claims;
using Application.Exceptions;
using Application.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Http;
using Serilog;

namespace Application.Features.Product.Command
{
    public class DeleteProductAsync
    {
        public class Command : IRequest
        {
            public Guid Id { get; set; }
        }

        public class Handler : IRequestHandler<Command>
        {
            private readonly IProductRepository _productRepository;
            private readonly IHttpContextAccessor _contextAccessor;

            public Handler(IProductRepository productRepository, IHttpContextAccessor contextAccessor)
            {
                _productRepository = productRepository;
                _contextAccessor = contextAccessor;
            }

            public async Task Handle(Command request, CancellationToken cancellationToken)
            {
                var productToDelete = await _productRepository.GetProductByIdAsync(request.Id);

                if (productToDelete == null) throw new NotFoundException("Product with this ID not exist");

                if (productToDelete.CreatedById !=
                    Guid.Parse(_contextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)!)
                    && !_contextAccessor.HttpContext.User.IsInRole("Admin"))
                    throw new ForbiddenAccessException("You cannot delete this product");

                _productRepository.DeleteProduct(productToDelete);
                var result = await _productRepository.SaveChangesAsync();

                if (result == 0) throw new Exception();

                Log.Information($"Product ({request.Id}) deleted");
            }
        }
    }
}
