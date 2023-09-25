﻿using Application.Dtos.Product;
using Application.Interfaces;
using AutoMapper;
using MediatR;

namespace Application.Product.Queries
{
    public class GetProductByIdAsync
    {
        public class Query : IRequest<ProductDetailDto>
        {
            public Guid Id { get; set; }
        }

        public class Handler : IRequestHandler<Query, ProductDetailDto>
        {
            private readonly IProductRepository _productRepository;
            private readonly IMapper _mapper;

            public Handler(IProductRepository productRepository, IMapper mapper)
            {
                _productRepository = productRepository;
                _mapper = mapper;
            }

            public async Task<ProductDetailDto> Handle(Query request, CancellationToken cancellationToken)
            {
                var product = await _productRepository.GetProductByIdAsync(request.Id);

                if (product == null) throw new Exception(); //TODO ErrorHandlingMiddleware (NotFound)

                var productDetailDto = _mapper.Map<ProductDetailDto>(product);

                return productDetailDto;
            }
        }
    }
}
