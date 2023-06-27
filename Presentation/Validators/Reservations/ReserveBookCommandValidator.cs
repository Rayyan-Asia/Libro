using Application.Entities.Reservations.Commnads;
using FluentValidation;

namespace Presentation.Validators.Reservations
{
    public class ReserveBookCommandValidator : AbstractValidator<ReserveBookCommand>
    {
        public ReserveBookCommandValidator()
        {
            RuleFor(command => command.BookId)
                .NotEmpty().WithMessage("Book ID is required.");

            RuleFor(command => command.UserId)
                .NotEmpty().WithMessage("User ID is required.");
        }
    }
}
