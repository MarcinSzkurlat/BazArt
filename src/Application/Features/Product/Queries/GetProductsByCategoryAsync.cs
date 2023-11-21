using Application.Dtos;
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
        public class Query : IRequest<PaginatedItems<IEnumerable<ProductDto>>>
        {
            public string CategoryName { get; set; }
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
                IEnumerable<Domain.Models.Product>? products;

                if (Enum.TryParse<Categories>(request.CategoryName, true, out _))
                {
                    products = await _productRepository.GetProductsByCategoryAsync(request.CategoryName, request.PageNumber, request.PageSize);
                }
                else
                {
                    throw new NotFoundException("This category not exist");
                }

                var productsQuantity =
                    await _productRepository.GetProductsQuantityByCategoryAsync(request.CategoryName);

                var productsDto = _mapper.Map<IEnumerable<ProductDto>>(products);

                return new PaginatedItems<IEnumerable<ProductDto>>(productsDto, request.PageNumber, (int)Math.Ceiling(productsQuantity / (double)request.PageSize));
            }
        }
    }
}
