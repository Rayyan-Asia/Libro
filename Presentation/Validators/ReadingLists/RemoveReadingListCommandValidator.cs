using Application.Entities.ReadingLists.Commands;
using FluentValidation;

namespace Presentation.Validators.ReadingLists
{
    public class RemoveReadingListCommandValidator : AbstractValidator<RemoveReadingListCommand>
    {
        public RemoveReadingListCommandValidator()
        {
            RuleFor(command => command.ReadingListId)
                .NotEmpty().WithMessage("Reading List ID is required.");

            RuleFor(command => command.UserId)
                .NotEmpty().WithMessage("User ID is required.");
        }
    }
}
