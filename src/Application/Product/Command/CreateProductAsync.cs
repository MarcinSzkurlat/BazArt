using Application.Dtos.Product;
using Application.Exceptions;
using Application.Interfaces;
using AutoMapper;
using Domain.Models;
using Domain.Models.User;
using FluentValidation;
using MediatR;
using Serilog;

namespace Application.Product.Command
{
    public class CreateProductAsync
    {
        public class Command : IRequest
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

        public class Handler : IRequestHandler<Command>
        {
            private readonly IProductRepository _productRepository;
            private readonly IMapper _mapper;

            private const string PlaceHolderPainting =
                "https://img.freepik.com/free-photo/close-up-oil-paints-brushes-palette_176420-2835.jpg?w=1380&t=st=1695635072~exp=1695635672~hmac=8633b9d82a1b0fc1d69e43572fdd05dc8036594b8ca05999f28264f5367db1c7";
            private const string PlaceHolderSculpture =
                "https://img.freepik.com/free-photo/graveyard-background-concept_23-2149585167.jpg?w=1380&t=st=1695635369~exp=1695635969~hmac=22150521383ded7251408335a4064e1afb2ee854130e46b52a924dbaa4107871";
            private const string PlaceHolderPhotography = "https://img.freepik.com/free-photo/selective-focus-shot-girl-using-professional-camera_181624-60663.jpg?w=740&t=st=1695635713~exp=1695636313~hmac=4e8da269e10e762f5a8a207bc023790f9b1df77a9682f0af3883dcfa8c30392a";
            private const string PlaceHolderHandMade = "https://img.freepik.com/free-photo/young-woman-using-macrame-technique_23-2149064470.jpg?w=1380&t=st=1695635983~exp=1695636583~hmac=5c6d0be0cf9c8851b78363f60d3b7caf03239c5b2c1a22016eb76645b204129a";
            private const string PlaceHolderNoCategory = "https://img.freepik.com/free-vector/abstract-watercolor-squared-frame_23-2149090424.jpg?w=826&t=st=1695636939~exp=1695637539~hmac=36515d62294093d9b1def0d3068780cdc06858d00d47ead805d5ceb0ac6dd82f";

            public Handler(IProductRepository productRepository, IMapper mapper)
            {
                _productRepository = productRepository;
                _mapper = mapper;
            }

            public async Task Handle(Command request, CancellationToken cancellationToken)
            {
                var existProduct = await _productRepository.GetProductByIdAsync(request.ProductToCreate.Id);

                if (existProduct != null) throw new BadRequestException("Product with this ID exist");

                var productToCreate = _mapper.Map<Domain.Models.Product>(request.ProductToCreate);

                if (string.IsNullOrWhiteSpace(productToCreate.ImageUrl))
                {
                    productToCreate.ImageUrl = request.ProductToCreate.Category switch
                    {
                        Categories.Painting => PlaceHolderPainting,
                        Categories.Sculpture => PlaceHolderSculpture,
                        Categories.Photography => PlaceHolderPhotography,
                        Categories.HandMade => PlaceHolderHandMade,
                        _ => PlaceHolderNoCategory
                    };
                }

                productToCreate.CategoryId = (int)request.ProductToCreate.Category;

                //TODO Add User as product creator
                productToCreate.CreatedBy = new User();

                await _productRepository.CreateProductAsync(productToCreate);
                var result = await _productRepository.SaveChangesAsync();

                if (result == 0) throw new Exception();

                Log.Information($"Product ({productToCreate.Id}) created");
            }
        }
    }
}
