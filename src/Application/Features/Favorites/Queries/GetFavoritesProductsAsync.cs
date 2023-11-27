using Application.Dtos;
using Application.Dtos.Product;
using Application.Interfaces;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Application.Features.Favorites.Queries
{
    public class GetFavoritesProductsAsync
    {
        public class Query : IRequest<PaginatedItems<IEnumerable<ProductDto>>>
        {
            public int PageNumber { get; set; }
            public int PageSize { get; set; }
        }

        public class Handler : IRequestHandler<Query, PaginatedItems<IEnumerable<ProductDto>>>
        {
            private readonly IFavoriteProductsRepository _favoriteProductsRepository;
            private readonly IMapper _mapper;
            private readonly IHttpContextAccessor _contextAccessor;

            public Handler(IFavoriteProductsRepository favoriteProductsRepository, IMapper mapper, IHttpContextAccessor contextAccessor)
            {
                _favoriteProductsRepository = favoriteProductsRepository;
                _mapper = mapper;
                _contextAccessor = contextAccessor;
            }

            public async Task<PaginatedItems<IEnumerable<ProductDto>>> Handle(Query request, CancellationToken cancellationToken)
            {
                request.PageSize = request.PageSize == 0 ? 10 : request.PageSize;

                var userId = Guid.Parse(_contextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));

                var favoriteProducts = await _favoriteProductsRepository.GetUserFavoritesProductsAsync(userId, request.PageNumber, request.PageSize);

                var favoriteProductsQuantity =
                    await _favoriteProductsRepository.GetUserFavoritesProductsQuantityAsync(userId);

                var favoriteProductsDto = _mapper.Map<IEnumerable<ProductDto>>(favoriteProducts);

                return new PaginatedItems<IEnumerable<ProductDto>>(favoriteProductsDto, request.PageNumber, (int)Math.Ceiling(favoriteProductsQuantity / (double)request.PageSize));
            }
        }
    }
}
