using Application.Dtos.Product;
using Application.Exceptions;
using Application.Interfaces;
using AutoMapper;
using Domain.Models;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using Serilog;
using System.Security.Claims;
using Microsoft.Extensions.Configuration;

namespace Application.Features.Product.Command
{
    public class CreateProductAsync
    {
        public class Command : IRequest<ProductDetailDto>
        {
            public CreateProductDto ProductToCreate { get; set; }
        }

        public class CommandValidator : AbstractValidator<CreateProductDto>
        {
            public CommandValidator()
            {
                int minCategoryValue = (int)Enum.GetValues(typeof(Categories)).Cast<Categories>().Min();

                int maxCategoryValue = (int)Enum.GetValues(typeof(Categories)).Cast<Categories>().Max();

                RuleFor(x => x.Id)
                    .NotEmpty().WithMessage("Id is a required field");
                RuleFor(x => x.Name)
                    .Length(5, 100).WithMessage("Name must be between 5 and 100 characters long")
                    .NotEmpty().WithMessage("Name is a required field"); ;
                RuleFor(x => x.Description)
                    .Length(5, 1000).WithMessage("Description must be between 5 and 1000 characters long")
                    .NotEmpty().WithMessage("Description is a required field"); ;
                RuleFor(x => x.Price)
                    .InclusiveBetween(0, 99999999).WithMessage("Price must be between 0 and 99 999 999");
                RuleFor(x => x.IsForSell)
                    .NotEmpty().WithMessage("IsForSell is a required field");
                RuleFor(x => x.Quantity)
                    .InclusiveBetween(1, 999).WithMessage("Quantity must be between 1 and 999");
                RuleFor(x => (int)x.Category)
                    .InclusiveBetween(minCategoryValue, maxCategoryValue).WithMessage($"Category must be between {minCategoryValue} and {maxCategoryValue}");
            }
        }

        public class Handler : IRequestHandler<Command, ProductDetailDto>
        {
            private readonly IProductRepository _productRepository;
            private readonly IMapper _mapper;
            private readonly IHttpContextAccessor _contextAccessor;
            private readonly IConfiguration _config;

            public Handler(IProductRepository productRepository, IMapper mapper, IHttpContextAccessor contextAccessor, IConfiguration config)
            {
                _productRepository = productRepository;
                _mapper = mapper;
                _contextAccessor = contextAccessor;
                _config = config;
            }

            public async Task<ProductDetailDto> Handle(Command request, CancellationToken cancellationToken)
            {
                var existProduct = await _productRepository.GetProductByIdAsync(request.ProductToCreate.Id);

                if (existProduct != null) throw new BadRequestException("Product with this ID exist");

                var productToCreate = _mapper.Map<Domain.Models.Product>(request.ProductToCreate);

                if (string.IsNullOrWhiteSpace(productToCreate.ImageUrl))
                {
                    productToCreate.ImageUrl = request.ProductToCreate.Category switch
                    {
                        Categories.Painting => _config["Images:PlaceHolders:Product:Painting"],
                        Categories.Sculpture => _config["Images:PlaceHolders:Product:Sculpture"],
                        Categories.Photography => _config["Images:PlaceHolders:Product:Photography"],
                        Categories.HandMade => _config["Images:PlaceHolders:Product:HandMade"],
                        _ => _config["Images:PlaceHolders:Product:NoCategory"]
                    };
                }

                productToCreate.CategoryId = (int)request.ProductToCreate.Category;
                productToCreate.CreatedById = Guid.Parse(_contextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));

                await _productRepository.CreateProductAsync(productToCreate);
                var result = await _productRepository.SaveChangesAsync();

                if (result == 0) throw new Exception();

                var productDto = _mapper.Map<ProductDetailDto>(productToCreate);

                Log.Information($"Product ({productToCreate.Id}) created");

                return productDto;
            }
        }
    }
}
