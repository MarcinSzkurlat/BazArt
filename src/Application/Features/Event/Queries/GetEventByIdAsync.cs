using Application.Dtos.Event;
using Application.Exceptions;
using Application.Interfaces;
using AutoMapper;
using MediatR;

namespace Application.Features.Event.Queries
{
    public class GetEventByIdAsync
    {
        public class Query : IRequest<EventDetailsDto>
        {
            public Guid Id { get; set; }
        }

        public class Handler : IRequestHandler<Query, EventDetailsDto>
        {
            private readonly IEventRepository _eventRepository;
            private readonly IMapper _mapper;

            public Handler(IEventRepository eventRepository, IMapper mapper)
            {
                _eventRepository = eventRepository;
                _mapper = mapper;
            }

            public async Task<EventDetailsDto> Handle(Query request, CancellationToken cancellationToken)
            {
                var eventDetails = await _eventRepository.GetEventByIdAsync(request.Id);

                if (eventDetails == null) throw new NotFoundException("Event with this ID not exist");

                var eventDetailsDto = _mapper.Map<EventDetailsDto>(eventDetails);

                return eventDetailsDto;
            }
        }
    }
}
