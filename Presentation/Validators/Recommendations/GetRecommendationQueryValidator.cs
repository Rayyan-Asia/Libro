using Application.Entities.Recommendations.Query;
using FluentValidation;

namespace Presentation.Validators.Recommendations
{
    public class GetRecommendationQueryValidator : AbstractValidator<GetRecommendationQuery>
    {
        public GetRecommendationQueryValidator()
        {
            RuleFor(query => query.UserId)
                .NotEmpty().WithMessage("User ID is required.");
        }
    }
}
