using Domain.Models;
using FluentValidation;

namespace BazArtAPI.Dtos.User.Validators
{
    public class RegistrationDtoValidator : AbstractValidator<RegistrationDto>
    {
        public RegistrationDtoValidator()
        {
            int minCategoryValue = (int)Enum.GetValues(typeof(Categories)).Cast<Categories>().Min();

            int maxCategoryValue = (int)Enum.GetValues(typeof(Categories)).Cast<Categories>().Max();

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is a required field")
                .EmailAddress().WithMessage("Invalid email format")
                .MaximumLength(50);

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is a required field")
                .Matches("^(?=.*\\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[!@#$%^&*()_+\\-=\\[\\]{};':\"\\\\|,.<>\\/?]).{6,40}$")
                .WithMessage("Password must contain at least one uppercase letter, one lowercase letter, one number, one special character, and be between 6 and 40 characters long");

            RuleFor(x => x.StageName)
                .Length(3, 50).WithMessage("Stage Name must be between 3 and 50 characters long")
                .When(x => x.StageName != null);
            
            RuleFor(x => x.Description)
                .Length(3, 1000).WithMessage("Description must be between 3 and 1000 characters long")
                .When(x => x.Description != null);

            RuleFor(x => (int)x.Category)
                .InclusiveBetween(minCategoryValue, maxCategoryValue)
                .WithMessage($"Category must be between {minCategoryValue} and {maxCategoryValue}")
                .When(x => x.Category != null);
        }
    }
}
