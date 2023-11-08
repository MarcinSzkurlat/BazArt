using System.Security.Claims;
using Application.Dtos.User;
using Application.Exceptions;
using Application.Interfaces;
using AutoMapper;
using Domain.Models;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using Serilog;

namespace Application.Features.User.Command
{
    public class EditUserDetailsAsync
    {
        public class Command : IRequest<UserDetailDto>
        {
            public EditUserDetailsDto EditUserDetailsDto { get; set; }
        }

        public class CommandValidator : AbstractValidator<EditUserDetailsDto>
        {
            public CommandValidator()
            {
                int minCategoryValue = (int)Enum.GetValues(typeof(Categories)).Cast<Categories>().Min();

                int maxCategoryValue = (int)Enum.GetValues(typeof(Categories)).Cast<Categories>().Max();

                RuleFor(x => x.Description)
                    .Length(5, 1000).WithMessage("Description must be between 5 and 1000 characters long");
                RuleFor(x => x.CategoryId)
                    .InclusiveBetween(minCategoryValue, maxCategoryValue).WithMessage($"Category must be between {minCategoryValue} and {maxCategoryValue}");
                RuleFor(x => x.Country)
                    .Length(2, 100).WithMessage("Country must be between 2 and 100 characters long");
                RuleFor(x => x.City)
                    .Length(2, 100).WithMessage("City must be between 2 and 100 characters long");
                RuleFor(x => x.Street)
                    .Length(2, 100).WithMessage("Street must be between 2 and 100 characters long");
                RuleFor(x => x.PostalCode)
                    .Length(2, 10).WithMessage("Postal code must be between 2 and 10 characters long");
            }
        }

        public class Handler : IRequestHandler<Command, UserDetailDto>
        {
            private readonly IUserRepository _userRepository;
            private readonly IHttpContextAccessor _contextAccessor;
            private readonly IMapper _mapper;

            public Handler(IUserRepository userRepository, IHttpContextAccessor contextAccessor, IMapper mapper)
            {
                _userRepository = userRepository;
                _contextAccessor = contextAccessor;
                _mapper = mapper;
            }

            public async Task<UserDetailDto> Handle(Command request, CancellationToken cancellationToken)
            {
                var userToEdit = await _userRepository.GetUserByIdAsync(Guid.Parse(_contextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)));

                if (userToEdit == null) throw new NotFoundException("User with this ID not exist");

                _mapper.Map(request.EditUserDetailsDto, userToEdit);

                await _userRepository.SaveChangesAsync();

                var userDto = _mapper.Map<UserDetailDto>(userToEdit);

                Log.Information($"User ({userToEdit.Id}) updated");

                return userDto;
            }
        }
    }
}
