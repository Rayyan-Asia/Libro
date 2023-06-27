using Application.Entities.Feedbacks.Queries;
using FluentValidation;

namespace Presentation.Validators.Feedbacks
{
    public class BrowseBookFeedbackQueryValidator : AbstractValidator<BrowseBookFeedbackQuery>
    {
        public BrowseBookFeedbackQueryValidator()
        {
            RuleFor(query => query.pageSize)
                .InclusiveBetween(0,5).WithMessage("Page size must be between 0 and 5.");

            RuleFor(query => query.BookId)
                .NotEmpty().WithMessage("Book ID is required.");
        }
    }
}
