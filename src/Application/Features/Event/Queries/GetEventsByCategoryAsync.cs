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
        public class Query : IRequest<IEnumerable<EventDto>>
        {
            public string CategoryName { get; set; }
        }

        public class Handler : IRequestHandler<Query, IEnumerable<EventDto>>
        {
            private readonly IEventRepository _eventRepository;
            private readonly IMapper _mapper;

            public Handler(IEventRepository eventRepository, IMapper mapper)
            {
                _eventRepository = eventRepository;
                _mapper = mapper;
            }

            public async Task<IEnumerable<EventDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                IEnumerable<Domain.Models.Event.Event>? events;

                if (Enum.TryParse<Categories>(request.CategoryName, true, out _))
                {
                    events = await _eventRepository.GetEventsByCategoryAsync(request.CategoryName);
                }
                else
                {
                    throw new BadRequestException("This category not exist");
                }

                var eventsDto = _mapper.Map<List<EventDto>>(events);

                return eventsDto;
            }
        }
    }
}
