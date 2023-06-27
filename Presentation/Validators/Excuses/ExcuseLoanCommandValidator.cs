using Application.Entities.Excuses.Commands;
using FluentValidation;

namespace Presentation.Validators.Excuses
{
    public class ExcuseLoanCommandValidator : AbstractValidator<ExcuseLoanCommand>
    {
        public ExcuseLoanCommandValidator()
        {
            RuleFor(command => command.LoanId)
                .NotEmpty().WithMessage("Loan ID is required.");
        }
    }
}
