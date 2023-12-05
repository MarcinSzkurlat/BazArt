using Application.Exceptions;
using Application.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using Application.Interfaces.Services;
using Serilog;

namespace Application.Features.Product.Command
{
    public class AddProductPhotoAsync
    {
        public class Command : IRequest
        {
            public Guid ProductId { get; set; }
            public IFormFile File { get; set; }
            public int PhotoHeight { get; set; }
            public int PhotoWidth { get; set; }
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
                request.PhotoHeight = request.PhotoHeight == 0 ? 480 : request.PhotoHeight;
                request.PhotoWidth = request.PhotoWidth == 0 ? 640 : request.PhotoWidth;

                var product = await _productRepository.GetProductByIdAsync(request.ProductId);

                if (product == null) throw new NotFoundException("Product with this ID not exist");

                if (product.CreatedById !=
                    Guid.Parse(_contextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)!)
                    && !_contextAccessor.HttpContext.User.IsInRole("Admin"))
                    throw new ForbiddenAccessException("You cannot add photo to this product");

                await _photoService.DeletePhotoAsync(request.ProductId.ToString());

                product.ImageUrl = await _photoService.AddPhotoAsync(request.File, request.ProductId.ToString(), request.PhotoHeight, request.PhotoWidth, "Images");

                await _productRepository.SaveChangesAsync();

                Log.Information($"Photo added to product ({request.ProductId})");
            }
        }
    }
}
