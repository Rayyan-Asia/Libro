using Application.Entities.Users.Commands;
using Domain;
using FluentValidation;

namespace Presentation.Validators.Users
{
    public class ModifyRoleCommandValidator : AbstractValidator<ModifyRoleCommand>
    {
        public ModifyRoleCommandValidator()
        {
            RuleFor(command => command.UserId)
                .NotEmpty().WithMessage("User ID is required.");

            RuleFor(command => command.Role).NotNull().IsInEnum();
        }
    }
}
