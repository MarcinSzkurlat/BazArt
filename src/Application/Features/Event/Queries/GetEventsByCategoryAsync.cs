using Application.Dtos;
using Application.Dtos.Event;
using Application.Exceptions;
using Application.Interfaces;
using AutoMapper;
using Domain.Models;
using MediatR;

namespace Application.Features.Event.Queries
{
    public class GetEventsByCategoryAsync
    {
        public class Query : IRequest<PaginatedItems<IEnumerable<EventDto>>>
        {
            public string CategoryName { get; set; }
            public int PageNumber { get; set; }
            public int PageSize { get; set; } = 10;
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
                IEnumerable<Domain.Models.Event.Event>? events;

                if (Enum.TryParse<Categories>(request.CategoryName, true, out _))
                {
                    events = await _eventRepository.GetEventsByCategoryAsync(request.CategoryName, request.PageNumber, request.PageSize);
                }
                else
                {
                    throw new NotFoundException("This category not exist");
                }

                var eventsQuantity = await _eventRepository.GetEventsQuantityByCategoryAsync(request.CategoryName);

                var eventsDto = _mapper.Map<IEnumerable<EventDto>>(events);

                return new PaginatedItems<IEnumerable<EventDto>>(eventsDto, request.PageNumber, (int)Math.Ceiling(eventsQuantity / (double)request.PageSize));
            }
        }
    }
}
