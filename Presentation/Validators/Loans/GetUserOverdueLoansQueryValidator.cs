using Application.Entities.Loans.Queries;
using FluentValidation;
namespace Presentation.Validators.Loans
{
    public class GetUserOverdueLoansQueryValidator : AbstractValidator<GetUserOverdueLoansQuery>
    {
        public GetUserOverdueLoansQueryValidator()
        {
            RuleFor(query => query.UserId).NotEmpty().WithMessage("UserId is required.");
            RuleFor(query => query.Rate).InclusiveBetween(0, 100).WithMessage("Rate must be between 0 and 100.");
            RuleFor(query => query.PageSize).InclusiveBetween(0, 5).WithMessage("PageSize must be between 0 and 5.");
            RuleFor(query => query.PageNumber).GreaterThanOrEqualTo(0).WithMessage("PageNumber must be greater than or equal to 0.");
        }
    }
}
