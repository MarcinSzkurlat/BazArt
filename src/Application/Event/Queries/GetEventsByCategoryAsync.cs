using Application.Dtos.Event;
using Application.Interfaces;
using AutoMapper;
using Domain.Models;
using MediatR;

namespace Application.Event.Queries
{
    public class GetEventsByCategoryAsync
    {
        public class Query : IRequest<IEnumerable<EventDto>>
        {
            public Categories CategoryName { get; set; }
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
                var events = request.CategoryName switch
                {
                    Categories.Painting => await _eventRepository.GetEventsByCategoryAsync(Categories.Painting),
                    Categories.Sculpture => await _eventRepository.GetEventsByCategoryAsync(Categories.Sculpture),
                    Categories.Photography => await _eventRepository.GetEventsByCategoryAsync(Categories.Photography),
                    Categories.HandMade => await _eventRepository.GetEventsByCategoryAsync(Categories.HandMade),
                    _ => new List<Domain.Models.Event.Event>() { }//TODO ErrorHandlingMiddleware (BadRequest)
                };

                var eventsDto = _mapper.Map<List<EventDto>>(events);

                return eventsDto;
            }
        }
    }
}
