using Application.Entities.ReadingLists.Commands;
using FluentValidation;

namespace Presentation.Validators.ReadingLists
{
    public class EditReadingListCommandValidator : AbstractValidator<EditReadingListCommand>
    {
        public EditReadingListCommandValidator()
        {
            RuleFor(command => command.Id)
                .NotEmpty().WithMessage("Reading List ID is required.");

            RuleFor(command => command.Books)
                .NotEmpty().WithMessage("At least one book ID is required.");

            RuleFor(command => command.Name)
                .NotEmpty().WithMessage("Name is required.");

            RuleFor(command => command.Description)
                .NotEmpty().WithMessage("Description is required.");

            RuleFor(command => command.UserId)
                .NotEmpty().WithMessage("User ID is required.");
        }
    }
}
