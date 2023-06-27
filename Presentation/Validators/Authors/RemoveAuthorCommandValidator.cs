using Application.Entities.Authors.Commands;
using FluentValidation;

namespace Presentation.Validators.Authors
{
    public class RemoveAuthorCommandValidator : AbstractValidator<RemoveAuthorCommand>
    {
        public RemoveAuthorCommandValidator()
        {
            RuleFor(command => command.Id)
                .NotEmpty().WithMessage("Author ID is required.");
        }
    }
}
