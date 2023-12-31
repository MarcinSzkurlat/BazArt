﻿using Application.Dtos.Event;
using Application.Exceptions;
using Application.Interfaces;
using AutoMapper;
using Domain.Models;
using FluentValidation;
using MediatR;
using Serilog;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace Application.Features.Event.Commands
{
    public class CreateEventAsync
    {
        public class Command : IRequest<EventDetailsDto>
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

        public class Handler : IRequestHandler<Command, EventDetailsDto>
        {
            private readonly IEventRepository _eventRepository;
            private readonly IMapper _mapper;
            private readonly IHttpContextAccessor _contextAccessor;
            private readonly IConfiguration _config;

            public Handler(IEventRepository eventRepository, IMapper mapper, IHttpContextAccessor contextAccessor, IConfiguration config)
            {
                _eventRepository = eventRepository;
                _mapper = mapper;
                _contextAccessor = contextAccessor;
                _config = config;
            }

            public async Task<EventDetailsDto> Handle(Command request, CancellationToken cancellationToken)
            {
                var eventFromDb = await _eventRepository.GetEventByIdAsync(request.CreateEventDto.Id);

                if (eventFromDb != null) throw new BadRequestException("Event with this ID exist");

                var eventToCreate = _mapper.Map<Domain.Models.Event.Event>(request.CreateEventDto);

                if (string.IsNullOrWhiteSpace(eventToCreate.ImageUrl)) eventToCreate.ImageUrl = _config["Images:PlaceHolders:Event"];

                eventToCreate.CategoryId = (int)request.CreateEventDto.Category;

                eventToCreate.CreatedById = Guid.Parse(_contextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));

                await _eventRepository.CreateEventAsync(eventToCreate);
                var result = await _eventRepository.SaveChangesAsync();

                if (result == 0) throw new Exception();

                var eventDto = _mapper.Map<EventDetailsDto>(eventToCreate);

                Log.Information($"Event ({eventToCreate.Id}) created");

                return eventDto;
            }
        }
    }
}
