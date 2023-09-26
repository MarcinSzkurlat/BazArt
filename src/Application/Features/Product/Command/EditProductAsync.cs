using Application.Dtos.Product;
using Application.Exceptions;
using Application.Interfaces;
using AutoMapper;
using Domain.Models;
using FluentValidation;
using MediatR;
using Serilog;

namespace Application.Features.Product.Command
{
    public class EditProductAsync
    {
        public class Command : IRequest
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

        public class Handler : IRequestHandler<Command>
        {
            private readonly IProductRepository _productRepository;
            private readonly IMapper _mapper;

            public Handler(IProductRepository productRepository, IMapper mapper)
            {
                _productRepository = productRepository;
                _mapper = mapper;
            }

            public async Task Handle(Command request, CancellationToken cancellationToken)
            {
                var productToEdit = await _productRepository.GetProductByIdAsync(request.Id);

                if (productToEdit == null) throw new NotFoundException("Product with this ID not exist");

                if (!string.IsNullOrWhiteSpace(request.ProductToEdit.Category.ToString())) productToEdit.CategoryId = (int)request.ProductToEdit.Category;

                _mapper.Map(request.ProductToEdit, productToEdit);

                await _productRepository.SaveChangesAsync();

                Log.Information($"Product ({request.Id}) updated");
            }
        }
    }
}
