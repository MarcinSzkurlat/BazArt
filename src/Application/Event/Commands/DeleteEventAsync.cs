using Application.Interfaces;
using MediatR;

namespace Application.Event.Commands
{
    public class DeleteEventAsync
    {
        public class Command : IRequest
        {
            public Guid Id { get; set; }
        }

        public class Handler : IRequestHandler<Command>
        {
            private readonly IEventRepository _eventRepository;

            public Handler(IEventRepository eventRepository)
            {
                _eventRepository = eventRepository;
            }

            public async Task Handle(Command request, CancellationToken cancellationToken)
            {
                var eventToDelete = await _eventRepository.GetEventByIdAsync(request.Id);

                if (eventToDelete == null) throw new Exception(); //TODO ErrorHandlingMiddleware (NotFound)

                _eventRepository.DeleteEvent(eventToDelete);

                var result = await _eventRepository.SaveChangesAsync();

                if (result == 0) throw new Exception(); //TODO ErrorHandlingMiddleware (InternalError??)
            }
        }
    }
}
