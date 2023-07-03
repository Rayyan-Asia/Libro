using Application.Entities.Loans.Queries;
using FluentValidation;
namespace Presentation.Validators.Loans
{
    public class GetAllActiveLoansQueryValidator : AbstractValidator<GetAllActiveLoansQuery>
    {
        public GetAllActiveLoansQueryValidator()
        {
            RuleFor(query => query.PageSize).InclusiveBetween(0, 5).WithMessage("PageSize must be between 0 and 5.");
            RuleFor(query => query.PageNumber).GreaterThanOrEqualTo(0).WithMessage("PageNumber must be greater than or equal to 0.");
        }
    }



}
