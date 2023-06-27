using Application.Entities.ReadingLists.Commands;
using FluentValidation;

namespace Presentation.Validators.ReadingLists
{
    public class AddBookToReadingListCommandValidator : AbstractValidator<AddBookToReadingListCommand>
    {
        public AddBookToReadingListCommandValidator()
        {
            RuleFor(command => command.UserId)
                .NotEmpty().WithMessage("User ID is required.");

            RuleFor(command => command.BookId)
                .NotEmpty().WithMessage("Book ID is required.");

            RuleFor(command => command.ReadingListId)
                .NotEmpty().WithMessage("Reading List ID is required.");
        }
    }
}
