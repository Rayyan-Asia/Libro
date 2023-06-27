using Application.Entities.Profiles.Queries;
using FluentValidation;

namespace Presentation.Validators.Profiles
{
    public class ViewProfileQueryValidator : AbstractValidator<ViewProfileQuery>
    {
        public ViewProfileQueryValidator()
        {
            RuleFor(query => query.PatronId)
                .NotEmpty().WithMessage("Patron ID is required.");
        }
    }
}
