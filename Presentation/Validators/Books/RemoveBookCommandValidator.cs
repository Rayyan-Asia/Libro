using Application.Entities.Books.Commands;
using FluentValidation;

namespace Presentation.Validators.Books
{
    public class RemoveBookCommandValidator : AbstractValidator<RemoveBookCommand>
    {
        public RemoveBookCommandValidator()
        {
            RuleFor(command => command.BookId)
                .NotEmpty().WithMessage("Book ID is required.");
        }
    }
}
