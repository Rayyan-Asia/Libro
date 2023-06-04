using Domain;
using FluentValidation;

namespace Presentation.Validators
{
    public class UserValidator : AbstractValidator<User>
    {
        public UserValidator() {
            RuleFor(user => user.Name).NotNull().MaximumLength(32).NotEmpty()
                .WithMessage("Name must not be empty and less than or equal 32 characters.");
            RuleFor(user => user.PhoneNumber).NotNull().NotEmpty()
                .WithMessage("Phone Number must not be empty");
            RuleFor(user => user.Email).NotNull().NotEmpty()
                .WithMessage("Email must not be empty or less than or equal to 100 characters.");
        }
    }
}
