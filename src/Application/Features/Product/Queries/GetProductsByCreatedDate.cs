using Application.Dtos.Product;
using Application.Interfaces;
using AutoMapper;
using MediatR;

namespace Application.Features.Product.Queries
{
    public class GetProductsByCreatedDate
    {
        public class Query : IRequest<IEnumerable<ProductDto>>
        {

        }

        public class Handler : IRequestHandler<Query, IEnumerable<ProductDto>>
        {
            private readonly IProductRepository _productRepository;
            private readonly IMapper _mapper;
            private const int ProductsAmount = 10;

            public Handler(IProductRepository productRepository, IMapper mapper)
            {
                _productRepository = productRepository;
                _mapper = mapper;
            }

            public async Task<IEnumerable<ProductDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                var products = await _productRepository.GetProductsByCreatedDate(ProductsAmount);

                var productsDto = _mapper.Map<IEnumerable<ProductDto>>(products);

                return productsDto;
            }
        }
    }
}
