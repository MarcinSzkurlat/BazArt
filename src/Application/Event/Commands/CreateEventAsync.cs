using Application.Dtos.Event;
using Application.Interfaces;
using AutoMapper;
using Domain.Models;
using Domain.Models.User;
using FluentValidation;
using MediatR;

namespace Application.Event.Commands
{
    public class CreateEventAsync
    {
        public class Command : IRequest
        {
            public CreateEventDto CreateEventDto { get; set; }
        }

        public class CommandValidator : AbstractValidator<CreateEventDto>
        {
            public CommandValidator()
            {
                int minCategoryValue = (int)Enum.GetValues(typeof(Categories)).Cast<Categories>().Min();

                int maxCategoryValue = (int)Enum.GetValues(typeof(Categories)).Cast<Categories>().Max();

                RuleFor(x => x.Id)
                    .NotEmpty().WithMessage("Id is a required field");
                RuleFor(x => x.Name)
                    .Length(5, 100).WithMessage("Name must be between 5 and 100 characters long")
                    .NotEmpty().WithMessage("Name is a required field");
                RuleFor(x => x.Description)
                    .Length(5, 1000).WithMessage("Description must be between 5 and 1000 characters long")
                    .NotEmpty().WithMessage("Description is a required field");
                RuleFor(x => x.ImageUrl)
                    .MaximumLength(500).WithMessage("Maximum length for ImageUrl is 500");
                RuleFor(x => (int)x.Category)
                    .InclusiveBetween(minCategoryValue, maxCategoryValue).WithMessage($"Category must be between {minCategoryValue} and {maxCategoryValue}")
                    .NotNull().WithMessage("Category is a required field");
                RuleFor(x => x.Country)
                    .Length(2, 100).WithMessage("Country must be between 2 and 100 characters long");
                RuleFor(x => x.City)
                    .Length(2, 100).WithMessage("City must be between 2 and 100 characters long");
                RuleFor(x => x.Street)
                    .Length(2, 100).WithMessage("Street must be between 2 and 100 characters long");
                RuleFor(x => x.PostalCode)
                    .Length(2, 10).WithMessage("Postal code must be between 2 and 10 characters long");
                RuleFor(x => x.StartingDate)
                    .Must(date => date >= DateTime.Now)
                    .WithMessage("StartingDate must be later than the current date and time");
                RuleFor(x => x.EndingDate)
                    .Must((x, date) => date >= x.StartingDate)
                    .WithMessage("EndingDate must be later than StartingDate");
            }
        }

        public class Handler : IRequestHandler<Command>
        {
            private readonly IEventRepository _eventRepository;
            private readonly IMapper _mapper;
            private const string EventImagePlaceHolder = "https://upload.wikimedia.org/wikipedia/commons/9/9b/Exposition_Richard_Prince%2C_American_Prayer_-_sc%C3%A9nographie_21.jpg";

            public Handler(IEventRepository eventRepository, IMapper mapper)
            {
                _eventRepository = eventRepository;
                _mapper = mapper;
            }

            public async Task Handle(Command request, CancellationToken cancellationToken)
            {
                var eventFromDb = await _eventRepository.GetEventByIdAsync(request.CreateEventDto.Id);

                if (eventFromDb != null) throw new Exception(); //TODO ErrorHandlingMiddleware (BadRequest)

                var eventToCreate = _mapper.Map<Domain.Models.Event.Event>(request.CreateEventDto);

                if (string.IsNullOrWhiteSpace(eventToCreate.ImageUrl)) eventToCreate.ImageUrl = EventImagePlaceHolder;

                eventToCreate.CategoryId = (int)request.CreateEventDto.Category;

                //TODO Add User as event creator
                eventToCreate.CreatedBy = new User();

                await _eventRepository.CreateEventAsync(eventToCreate);
                var result = await _eventRepository.SaveChangesAsync();

                if (result == 0) throw new Exception(); //TODO ErrorHandlingMiddleware (InternalError??)
            }
        }
    }
}
