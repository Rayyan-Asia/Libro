using Application.Entities.Feedbacks.Commands;
using FluentValidation;

namespace Presentation.Validators.Feedbacks
{
    public class EditFeedbackCommandValidator : AbstractValidator<EditFeedbackCommand>
    {
        public EditFeedbackCommandValidator()
        {
            RuleFor(command => command.Id)
                .NotEmpty().WithMessage("ID is required.");

            RuleFor(command => command.Rating)
                .NotEmpty().WithMessage("Rating is required.");

            RuleFor(command => command.Review)
                .NotEmpty().WithMessage("Review is required.")
                .MaximumLength(500).WithMessage("Review cannot exceed 500 characters.");

            RuleFor(command => command.UserId)
                .NotEmpty().WithMessage("User ID is required.");

            RuleFor(command => command.BookId)
                .NotEmpty().WithMessage("Book ID is required.");

            RuleFor(command => command.CreatedDate)
                .NotEmpty().WithMessage("Created Date is required.");
        }
    }
}
