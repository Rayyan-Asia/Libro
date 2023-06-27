using Application.Entities.ReadingLists.Queries;
using FluentValidation;

namespace Presentation.Validators.ReadingLists
{
    public class BrowseReadingListsQueryValidator : AbstractValidator<BrowseReadingListsQuery>
    {
        public BrowseReadingListsQueryValidator()
        {
            RuleFor(query => query.pageSize)
                .InclusiveBetween(0, 5).WithMessage("Page size must be between 0 and 5.");

            RuleFor(query => query.UserId)
                .NotEmpty().WithMessage("User ID is required.");
        }
    }
}
