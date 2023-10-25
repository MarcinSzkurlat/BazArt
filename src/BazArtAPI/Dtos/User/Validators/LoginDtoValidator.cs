using FluentValidation;

namespace BazArtAPI.Dtos.User.Validators
{
    public class LoginDtoValidator : AbstractValidator<LoginDto>
    {
        public LoginDtoValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is a required field")
                .EmailAddress().WithMessage("Invalid email format")
                .MaximumLength(50);

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is a required field")
                .Matches("^(?=.*\\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[!@#$%^&*()_+\\-=\\[\\]{};':\"\\\\|,.<>\\/?]).{6,40}$")
                .WithMessage("Password must contain at least one uppercase letter, one lowercase letter, one number, one special character, and be between 6 and 40 characters long");
        }
    }
}
