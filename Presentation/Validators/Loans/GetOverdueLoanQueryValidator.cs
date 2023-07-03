using Application.Entities.Loans.Queries;
using FluentValidation;
namespace Presentation.Validators.Loans
{
    public class GetOverdueLoanQueryValidator : AbstractValidator<GetOverdueLoanQuery>
    {
        public GetOverdueLoanQueryValidator()
        {
            RuleFor(query => query.Id).NotEmpty().WithMessage("LoanId is required.");
            RuleFor(query => query.Rate).InclusiveBetween(0, 100).WithMessage("Rate must be between 0 and 100.");
        }
    }



}
