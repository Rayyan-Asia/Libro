using Application.Entities.Books.Commands;
using FluentValidation;

namespace Presentation.Validators.Books
{
    public class RemoveAuthorFromBookCommandValidator : AbstractValidator<RemoveAuthorFromBookCommand>
    {
        public RemoveAuthorFromBookCommandValidator()
        {
            RuleFor(command => command.BookId)
                .NotEmpty().WithMessage("Book ID is required.");

            RuleFor(command => command.AuthorId)
                .NotEmpty().WithMessage("Author ID is required.");
        }
    }
}
