using Application.Interfaces.Services;
using Application.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Http;
using Application.Exceptions;
using Serilog;
using System.Security.Claims;
using Domain.Models;
using Microsoft.Extensions.Configuration;

namespace Application.Features.Product.Command
{
    public class DeleteProductPhotoAsync
    {
        public class Command : IRequest
        {
            public Guid ProductId { get; set; }
        }

        public class Handler : IRequestHandler<Command>
        {
            private readonly IProductRepository _productRepository;
            private readonly IHttpContextAccessor _contextAccessor;
            private readonly IPhotoService _photoService;
            private readonly IConfiguration _config;

            public Handler(IProductRepository productRepository, IHttpContextAccessor contextAccessor, IPhotoService photoService, IConfiguration config)
            {
                _productRepository = productRepository;
                _contextAccessor = contextAccessor;
                _photoService = photoService;
                _config = config;
            }

            public async Task Handle(Command request, CancellationToken cancellationToken)
            {
                var product = await _productRepository.GetProductByIdAsync(request.ProductId);

                if (product == null) throw new NotFoundException("Product with this ID not exist");

                if (product.CreatedById !=
                    Guid.Parse(_contextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)!)
                    && !_contextAccessor.HttpContext.User.IsInRole("Admin"))
                    throw new ForbiddenAccessException("You cannot delete photo from this product");

                await _photoService.DeletePhotoAsync(request.ProductId.ToString());

                product.ImageUrl = (Categories)product.CategoryId switch
                {
                    Categories.Painting => _config["Images:PlaceHolders:Product:Painting"],
                    Categories.Sculpture => _config["Images:PlaceHolders:Product:Sculpture"],
                    Categories.Photography => _config["Images:PlaceHolders:Product:Photography"],
                    Categories.HandMade => _config["Images:PlaceHolders:Product:HandMade"],
                    _ => _config["Images:PlaceHolders:Product:NoCategory"]
                };

                await _productRepository.SaveChangesAsync();

                Log.Information($"Photo deleted from product ({request.ProductId})");
            }
        }
    }
}
