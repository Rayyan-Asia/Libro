using Application.Entities.Excuses.Commands;
using FluentValidation;

namespace Presentation.Validators.Excuses
{
    public class ExcuseUserCommandValidator : AbstractValidator<ExcuseUserCommand>
    {
        public ExcuseUserCommandValidator()
        {
            RuleFor(command => command.UserId)
                .NotEmpty().WithMessage("User ID is required.");
        }
    }
}
