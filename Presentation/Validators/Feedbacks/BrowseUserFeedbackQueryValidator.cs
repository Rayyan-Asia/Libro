using Application.Entities.Feedbacks.Queries;
using FluentValidation;

namespace Presentation.Validators.Feedbacks
{
    public class BrowseUserFeedbackQueryValidator : AbstractValidator<BrowseUserFeedbackQuery>
    {
        public BrowseUserFeedbackQueryValidator()
        {
            RuleFor(query => query.pageSize)
                .InclusiveBetween(0, 5).WithMessage("Page size must be between 0 and 5.");

            RuleFor(query => query.UserId)
                .NotEmpty().WithMessage("User ID is required.");
        }
    }
}
