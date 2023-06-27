using Application.DTOs;
using FluentValidation;

namespace Presentation.Validators
{
    public class IdDtoValidator : AbstractValidator<IdDto>
    {
        public IdDtoValidator()
        {
            RuleFor(dto => dto.Id)
                .NotEmpty().WithMessage("ID is required.");
        }
    }
}
