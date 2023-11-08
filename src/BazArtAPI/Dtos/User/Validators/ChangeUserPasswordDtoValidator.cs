using FluentValidation;

namespace BazArtAPI.Dtos.User.Validators
{
    public class ChangeUserPasswordDtoValidator : AbstractValidator<ChangeUserPasswordDto>
    {
        public ChangeUserPasswordDtoValidator()
        {
            RuleFor(x => x.OldPassword)
                .NotEmpty().WithMessage("Old password is a required field")
                .Matches("^(?=.*\\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[!@#$%^&*()_+\\-=\\[\\]{};':\"\\\\|,.<>\\/?]).{6,40}$")
                .WithMessage("Password must contain at least one uppercase letter, one lowercase letter, one number, one special character, and be between 6 and 40 characters long");

            RuleFor(x => x.NewPassword)
                .NotEmpty().WithMessage("New password is a required field")
                .Matches("^(?=.*\\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[!@#$%^&*()_+\\-=\\[\\]{};':\"\\\\|,.<>\\/?]).{6,40}$")
                .WithMessage("Password must contain at least one uppercase letter, one lowercase letter, one number, one special character, and be between 6 and 40 characters long");
        }
    }
}
