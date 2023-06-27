using Application.Entities.Books.Queries;
using FluentValidation;

namespace Presentation.Validators.Books
{
    public class SearchQueryValidator : AbstractValidator<SearchQuery>
    {
        public SearchQueryValidator()
        {
            RuleFor(query => query.Title)
                .MaximumLength(100).WithMessage("Title cannot exceed 100 characters.");

            RuleFor(query => query.Author)
                .MaximumLength(32).WithMessage("Author cannot exceed 32 characters.");

            RuleFor(query => query.Genre)
                .MaximumLength(32).WithMessage("Genre cannot exceed 32 characters.");

            RuleFor(query => query.pageSize)
                .InclusiveBetween(0, 5).WithMessage("Page size must be between 0 and 5.");

            RuleFor(query => query.pageNumber)
                .GreaterThanOrEqualTo(0).WithMessage("Page number must be greater than or equal to 0.");
        }
    }
}
