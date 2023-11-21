using Application.Dtos;
using Application.Dtos.Product;
using Application.Interfaces;
using AutoMapper;
using MediatR;

namespace Application.Features.Product.Queries
{
    public class GetProductsByUserIdAsync
    {
        public class Query : IRequest<PaginatedItems<IEnumerable<ProductDto>>>
        {
            public Guid Id { get; set; }
            public int PageNumber { get; set; }
            public int PageSize { get; set; }
        }

        public class Handler : IRequestHandler<Query, PaginatedItems<IEnumerable<ProductDto>>>
        {
            private readonly IProductRepository _productRepository;
            private readonly IMapper _mapper;

            public Handler(IProductRepository productRepository, IMapper mapper)
            {
                _productRepository = productRepository;
                _mapper = mapper;
            }

            public async Task<PaginatedItems<IEnumerable<ProductDto>>> Handle(Query request, CancellationToken cancellationToken)
            {
                var products = await _productRepository.GetProductsByUserIdAsync(request.Id, request.PageNumber, request.PageSize);

                var productsQuantity = await _productRepository.GetProductsQuantityByUserIdAsync(request.Id);

                var productsDto = _mapper.Map<IEnumerable<ProductDto>>(products);

                return new PaginatedItems<IEnumerable<ProductDto>>(productsDto, request.PageNumber, (int)Math.Ceiling(productsQuantity / (double)request.PageSize));
            }
        }
    }
}
