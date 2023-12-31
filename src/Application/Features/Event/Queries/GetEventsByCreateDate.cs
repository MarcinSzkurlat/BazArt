﻿using Application.Dtos.Event;
using Application.Interfaces;
using AutoMapper;
using MediatR;

namespace Application.Features.Event.Queries
{
    public class GetEventsByCreateDate
    {
        public class Query : IRequest<IEnumerable<EventDto>>
        {

        }

        public class Handler : IRequestHandler<Query, IEnumerable<EventDto>>
        {
            private readonly IEventRepository _eventRepository;
            private readonly IMapper _mapper;
            private const int EventsAmount = 10;

            public Handler(IEventRepository eventRepository, IMapper mapper)
            {
                _eventRepository = eventRepository;
                _mapper = mapper;
            }

            public async Task<IEnumerable<EventDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                var events = await _eventRepository.GetEventsByCreatedDate(EventsAmount);

                var eventsDto = _mapper.Map<IEnumerable<EventDto>>(events);

                return eventsDto;
            }
        }
    }
}
