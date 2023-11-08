using System.Security.Claims;
using Application.Dtos.Event;
using Application.Exceptions;
using Application.Interfaces;
using AutoMapper;
using Domain.Models;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using Serilog;

namespace Application.Features.Event.Commands
{
    public class EditEventAsync
    {
        public class Command : IRequest<EventDetailsDto>
        {
            public Guid Id { get; set; }
            public EditEventDto EditEventDto { get; set; }
        }

        public class CommandValidator : AbstractValidator<EditEventDto>
        {
            public CommandValidator()
            {
                int minCategoryValue = (int)Enum.GetValues(typeof(Categories)).Cast<Categories>().Min();

                int maxCategoryValue = (int)Enum.GetValues(typeof(Categories)).Cast<Categories>().Max();

                RuleFor(x => x.Name)
                    .Length(5, 100).WithMessage("Name must be between 5 and 100 characters long");
                RuleFor(x => x.Description)
                    .Length(5, 1000).WithMessage("Description must be between 5 and 1000 characters long");
                RuleFor(x => x.ImageUrl)
                    .MaximumLength(500).WithMessage("Maximum length for ImageUrl is 500");
                RuleFor(x => (int)x.Category)
                    .InclusiveBetween(minCategoryValue, maxCategoryValue).WithMessage($"Category must be between {minCategoryValue} and {maxCategoryValue}");
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

        public class Handler : IRequestHandler<Command, EventDetailsDto>
        {
            private readonly IEventRepository _eventRepository;
            private readonly IMapper _mapper;
            private readonly ICategoryRepository _categoryRepository;
            private readonly IHttpContextAccessor _contextAccessor;

            public Handler(IEventRepository eventRepository, IMapper mapper, ICategoryRepository categoryRepository, IHttpContextAccessor contextAccessor)
            {
                _eventRepository = eventRepository;
                _mapper = mapper;
                _categoryRepository = categoryRepository;
                _contextAccessor = contextAccessor;
            }

            public async Task<EventDetailsDto> Handle(Command request, CancellationToken cancellationToken)
            {
                var eventToEdit = await _eventRepository.GetEventByIdAsync(request.Id);

                if (eventToEdit == null) throw new NotFoundException("Event with this ID not exist");

                if (eventToEdit.CreatedById !=
                    Guid.Parse(_contextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)!)
                    && !_contextAccessor.HttpContext.User.IsInRole("Admin"))
                    throw new ForbiddenAccessException("You cannot edit this product");

                if (!string.IsNullOrWhiteSpace(request.EditEventDto.Category.ToString()))
                {
                    eventToEdit.CategoryId = (int)request.EditEventDto.Category;
                    eventToEdit.Category =
                        await _categoryRepository.GetCategoryByNameAsync(request.EditEventDto.Category.ToString());
                }

                _mapper.Map(request.EditEventDto, eventToEdit);

                await _eventRepository.SaveChangesAsync();

                var eventDto = _mapper.Map<EventDetailsDto>(eventToEdit);

                Log.Information($"Event ({request.Id}) updated");

                return eventDto;
            }
        }
    }
}
