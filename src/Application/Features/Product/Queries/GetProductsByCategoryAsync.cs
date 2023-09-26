using Application.Dtos.Product;
using Application.Exceptions;
using Application.Interfaces;
using AutoMapper;
using Domain.Models;
using MediatR;

namespace Application.Features.Product.Queries
{
    public class GetProductsByCategoryAsync
    {
        public class Query : IRequest<IEnumerable<ProductDto>>
        {
            public Categories CategoryName { get; set; }
        }

        public class Handler : IRequestHandler<Query, IEnumerable<ProductDto>>
        {
            private readonly IProductRepository _productRepository;
            private readonly IMapper _mapper;

            public Handler(IProductRepository productRepository, IMapper mapper)
            {
                _productRepository = productRepository;
                _mapper = mapper;
            }

            public async Task<IEnumerable<ProductDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                var products = request.CategoryName switch
                {
                    Categories.Painting => await _productRepository.GetProductsByCategoryAsync(Categories.Painting),
                    Categories.Sculpture => await _productRepository.GetProductsByCategoryAsync(Categories.Sculpture),
                    Categories.Photography => await _productRepository.GetProductsByCategoryAsync(
                        Categories.Photography),
                    Categories.HandMade => await _productRepository.GetProductsByCategoryAsync(Categories.HandMade),
                    _ => throw new BadRequestException("This category not exist")
                };

                var productsDto = _mapper.Map<IEnumerable<ProductDto>>(products);

                return productsDto;
            }
        }
    }
}
