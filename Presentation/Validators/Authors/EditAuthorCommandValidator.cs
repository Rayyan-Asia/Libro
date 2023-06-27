using Application.Entities.Authors.Commands;
using FluentValidation;

namespace Presentation.Validators.Authors
{
    public class EditAuthorCommandValidator : AbstractValidator<EditAuthorCommand>
    {
        public EditAuthorCommandValidator()
        {
            RuleFor(command => command.Id)
                .NotEmpty().WithMessage("Author ID is required.");

            RuleFor(command => command.Name)
                .NotEmpty().WithMessage("Name is required.")
                .MaximumLength(32).WithMessage("Name cannot exceed 32 characters.");

            RuleFor(command => command.Description)
                .MaximumLength(500).WithMessage("Description cannot exceed 500 characters.");

            RuleForEach(command => command.Books)
                .SetValidator(new IdDtoValidator());
        }
    }
}
