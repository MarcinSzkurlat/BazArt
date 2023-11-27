using Application.Dtos.Product;
using Application.Interfaces;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Application.Features.Cart.Queries
{
    public class GetUserCartAsync
    {
        public class Query : IRequest<IEnumerable<ProductDto>>
        {

        }

        public class Handler : IRequestHandler<Query, IEnumerable<ProductDto>>
        {
            private readonly ICartRepository _cartRepository;
            private readonly IMapper _mapper;
            private readonly IHttpContextAccessor _contextAccessor;

            public Handler(ICartRepository cartRepository, IMapper mapper, IHttpContextAccessor contextAccessor)
            {
                _cartRepository = cartRepository;
                _mapper = mapper;
                _contextAccessor = contextAccessor;
            }

            public async Task<IEnumerable<ProductDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                var userId = Guid.Parse(_contextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));

                var userCart = await _cartRepository.GetUserCartAsync(userId);

                var userCartDto = _mapper.Map<IEnumerable<ProductDto>>(userCart);

                return userCartDto;
            }
        }
    }
}
