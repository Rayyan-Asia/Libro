using Application.Entities.Feedbacks.Commands;
using FluentValidation;

namespace Presentation.Validators.Feedbacks
{
    public class RemoveFeedbackCommandValidator : AbstractValidator<RemoveFeedbackCommand>
    {
        public RemoveFeedbackCommandValidator()
        {
            RuleFor(command => command.FeedbackId)
                .NotEmpty().WithMessage("Feedback ID is required.");

            RuleFor(command => command.UserId)
                .NotEmpty().WithMessage("User ID is required.");
        }
    }
}
