using Application.Dtos;
using Application.Dtos.Event;
using Application.Interfaces;
using AutoMapper;
using MediatR;

namespace Application.Features.Event.Queries
{
    public class GetEventsByUserIdAsync
    {
        public class Query : IRequest<PaginatedItems<IEnumerable<EventDto>>>
        {
            public Guid Id { get; set; }
            public int PageNumber { get; set; }
            public int PageSize { get; set; }
        }

        public class Handler : IRequestHandler<Query, PaginatedItems<IEnumerable<EventDto>>>
        {
            private readonly IEventRepository _eventRepository;
            private readonly IMapper _mapper;

            public Handler(IEventRepository eventRepository, IMapper mapper)
            {
                _eventRepository = eventRepository;
                _mapper = mapper;
            }
            
            public async Task<PaginatedItems<IEnumerable<EventDto>>> Handle(Query request, CancellationToken cancellationToken)
            {
                var events = await _eventRepository.GetEventsByUserIdAsync(request.Id, request.PageNumber, request.PageSize);

                var eventsQuantity = await _eventRepository.GetEventsQuantityByUserIdAsync(request.Id);

                var eventsDto = _mapper.Map<IEnumerable<EventDto>>(events);

                return new PaginatedItems<IEnumerable<EventDto>>(eventsDto, request.PageNumber, (int)Math.Ceiling(eventsQuantity / (double)request.PageSize));
            }
        }
    }
}
