using System.Security.Claims;
using Application.Exceptions;
using Application.Interfaces;
using Application.Interfaces.Services;
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
            private readonly IPhotoService _photoService;

            public Handler(IProductRepository productRepository, IHttpContextAccessor contextAccessor, IPhotoService photoService)
            {
                _productRepository = productRepository;
                _contextAccessor = contextAccessor;
                _photoService = photoService;
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

                await _photoService.DeletePhotoAsync(request.Id.ToString());

                Log.Information($"Product ({request.Id}) deleted");
            }
        }
    }
}
