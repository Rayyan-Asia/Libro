using Application.Entities.Returns.Commands;
using FluentValidation;

namespace Presentation.Validators.Returns
{
    public class ApproveBookReturnCommandValidator : AbstractValidator<ApproveBookReturnCommand>
    {
        public ApproveBookReturnCommandValidator()
        {
            RuleFor(command => command.BookReturnId)
                .NotEmpty().WithMessage("Book return ID is required.");
        }
    }
}
