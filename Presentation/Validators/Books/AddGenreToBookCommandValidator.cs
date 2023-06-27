using Application.Entities.Books.Commands;
using FluentValidation;

namespace Presentation.Validators.Books
{
    public class AddGenreToBookCommandValidator : AbstractValidator<AddGenreToBookCommand>
    {
        public AddGenreToBookCommandValidator()
        {
            RuleFor(command => command.BookId)
                .NotEmpty().WithMessage("Book ID is required.");

            RuleFor(command => command.GenreId)
                .NotEmpty().WithMessage("Genre ID is required.");
        }
    }
}
