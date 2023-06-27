using Application.Entities.Returns.Commands;
using FluentValidation;

namespace Presentation.Validators.Returns
{
    public class BookReturnCommandValidator : AbstractValidator<BookReturnCommand>
    {
        public BookReturnCommandValidator()
        {
            RuleFor(command => command.LoanId)
                .NotEmpty().WithMessage("Loan ID is required.");

            RuleFor(command => command.UserId)
                .NotEmpty().WithMessage("User ID is required.");
        }
    }
}
