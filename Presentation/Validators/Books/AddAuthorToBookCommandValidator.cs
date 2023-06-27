using Application.Entities.Books.Commands;
using FluentValidation;

namespace Presentation.Validators.Books
{
    public class AddAuthorToBookCommandValidator : AbstractValidator<AddAuthorToBookCommand>
    {
        public AddAuthorToBookCommandValidator()
        {
            RuleFor(command => command.BookId)
                .NotEmpty().WithMessage("Book ID is required.");

            RuleFor(command => command.AuthorId)
                .NotEmpty().WithMessage("Author ID is required.");
        }
    }
}
