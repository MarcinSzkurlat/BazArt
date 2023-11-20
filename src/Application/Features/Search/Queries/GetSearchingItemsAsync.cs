using Application.Dtos.Search;
using Application.Interfaces;
using AutoMapper;
using MediatR;

namespace Application.Features.Search.Queries
{
    public class GetSearchingItemsAsync
    {
        public class Query : IRequest<SearchResultDto>
        {
            public string SearchQuery { get; set; }
        }

        public class Handler : IRequestHandler<Query, SearchResultDto>
        {
            private readonly IProductRepository _productRepository;
            private readonly IEventRepository _eventRepository;
            private readonly IUserRepository _userRepository;
            private readonly IMapper _mapper;

            public Handler(IProductRepository productRepository, IEventRepository eventRepository, IUserRepository userRepository, IMapper mapper)
            {
                _productRepository = productRepository;
                _eventRepository = eventRepository;
                _userRepository = userRepository;
                _mapper = mapper;
            }

            public async Task<SearchResultDto> Handle(Query request, CancellationToken cancellationToken)
            {
                var products = await _productRepository.GetProductsBySearchQueryAsync(request.SearchQuery);

                var events = await _eventRepository.GetEventsBySearchQueryAsync(request.SearchQuery);

                var users = await _userRepository.GetUsersBySearchQueryAsync(request.SearchQuery);

                var productsDto = _mapper.Map<IEnumerable<SearchItemDto>>(products);
                var eventsDto = _mapper.Map<IEnumerable<SearchItemDto>>(events);
                var usersDto = _mapper.Map<IEnumerable<SearchItemDto>>(users);

                return new SearchResultDto(new Dictionary<string, SearchCategoryResultDto>
                {
                    {"Products", new SearchCategoryResultDto("Products", productsDto)},
                    {"Events", new SearchCategoryResultDto("Events", eventsDto)},
                    {"Users", new SearchCategoryResultDto("Users", usersDto)}
                    }
                );
            }
        }
    }
}
