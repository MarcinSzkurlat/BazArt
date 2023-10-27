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

namespace Application.Features.Product.Command
{
    public class EditProductAsync
    {
        public class Command : IRequest<ProductDetailDto>
        {
            public Guid Id { get; set; }
            public EditProductDto ProductToEdit { get; set; }
        }

        public class CommandValidator : AbstractValidator<EditProductDto>
        {
            public CommandValidator()
            {
                int minCategoryValue = (int)Enum.GetValues(typeof(Categories)).Cast<Categories>().Min();

                int maxCategoryValue = (int)Enum.GetValues(typeof(Categories)).Cast<Categories>().Max();

                RuleFor(x => x.Name)
                    .Length(5, 100).WithMessage("Name must be between 5 and 100 characters long");
                RuleFor(x => x.Description)
                    .Length(5, 1000).WithMessage("Description must be between 5 and 1000 characters long");
                RuleFor(x => x.Price)
                    .InclusiveBetween(0, 99999999).WithMessage("Price must be between 0 and 99 999 999");
                RuleFor(x => x.Quantity)
                    .InclusiveBetween(1, 999).WithMessage("Quantity must be between 1 and 999");
                RuleFor(x => (int)x.Category)
                    .InclusiveBetween(minCategoryValue, maxCategoryValue)
                    .WithMessage($"Category must be between {minCategoryValue} and {maxCategoryValue}");
            }
        }

        public class Handler : IRequestHandler<Command, ProductDetailDto>
        {
            private readonly IProductRepository _productRepository;
            private readonly IMapper _mapper;
            private readonly ICategoryRepository _categoryRepository;
            private readonly IHttpContextAccessor _contextAccessor;

            public Handler(IProductRepository productRepository, IMapper mapper, ICategoryRepository categoryRepository, IHttpContextAccessor contextAccessor)
            {
                _productRepository = productRepository;
                _mapper = mapper;
                _categoryRepository = categoryRepository;
                _contextAccessor = contextAccessor;
            }

            public async Task<ProductDetailDto> Handle(Command request, CancellationToken cancellationToken)
            {
                var productToEdit = await _productRepository.GetProductByIdAsync(request.Id);

                if (productToEdit == null) throw new NotFoundException("Product with this ID not exist");

                if (productToEdit.CreatedById !=
                    Guid.Parse(_contextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)!)
                    && !_contextAccessor.HttpContext.User.IsInRole("Admin"))
                    throw new ForbiddenAccessException("You cannot edit this product");

                if (!string.IsNullOrWhiteSpace(request.ProductToEdit.Category.ToString()))
                {
                    productToEdit.CategoryId = (int)request.ProductToEdit.Category;
                    productToEdit.Category =
                        await _categoryRepository.GetCategoryByNameAsync(request.ProductToEdit.Category.ToString());
                }

                string imageUrl = productToEdit.ImageUrl!;

                _mapper.Map(request.ProductToEdit, productToEdit);

                if(request.ProductToEdit.ImageUrl == null) productToEdit.ImageUrl = imageUrl;

                await _productRepository.SaveChangesAsync();

                var productDto = _mapper.Map<ProductDetailDto>(productToEdit);

                Log.Information($"Product ({request.Id}) updated");

                return productDto;
            }
        }
    }
}
