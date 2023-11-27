using Application.Interfaces;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using Application.Dtos;
using Application.Dtos.User;

namespace Application.Features.Favorites.Queries
{
    public class GetFavoritesUsersAsync
    {
        public class Query : IRequest<PaginatedItems<IEnumerable<UserDetailDto>>>
        {
            public int PageNumber { get; set; }
            public int PageSize { get; set; }
        }

        public class Handler : IRequestHandler<Query, PaginatedItems<IEnumerable<UserDetailDto>>>
        {
            private readonly IFavoriteUsersRepository _favoriteUsersRepository;
            private readonly IMapper _mapper;
            private readonly IHttpContextAccessor _contextAccessor;

            public Handler(IFavoriteUsersRepository favoriteUsersRepository, IMapper mapper, IHttpContextAccessor contextAccessor)
            {
                _favoriteUsersRepository = favoriteUsersRepository;
                _mapper = mapper;
                _contextAccessor = contextAccessor;
            }

            public async Task<PaginatedItems<IEnumerable<UserDetailDto>>> Handle(Query request, CancellationToken cancellationToken)
            {
                request.PageSize = request.PageSize == 0 ? 10 : request.PageSize;

                var userId = Guid.Parse(_contextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));

                var favoriteUsers = await _favoriteUsersRepository.GetUserFavoritesUsersAsync(userId, request.PageNumber, request.PageSize);

                var favoriteUsersQuantity = await _favoriteUsersRepository.GetUserFavoritesUsersQuantityAsync(userId);

                var favoriteUsersDto = _mapper.Map<IEnumerable<UserDetailDto>>(favoriteUsers);

                return new PaginatedItems<IEnumerable<UserDetailDto>>(favoriteUsersDto, request.PageNumber,
                    (int)Math.Ceiling(favoriteUsersQuantity / (double)request.PageSize));
            }
        }
    }
}
