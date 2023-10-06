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
            public string CategoryName { get; set; }
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
                IEnumerable<Domain.Models.Product>? products;

                if (Enum.TryParse<Categories>(request.CategoryName, true, out _))
                {
                    products = await _productRepository.GetProductsByCategoryAsync(request.CategoryName);
                }
                else
                {
                    throw new BadRequestException("This category not exist");
                }

                var productsDto = _mapper.Map<IEnumerable<ProductDto>>(products);

                return productsDto;
            }
        }
    }
}
