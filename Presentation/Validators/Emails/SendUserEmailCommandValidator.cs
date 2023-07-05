using Application.Entities.Emails.Commands;
using FluentValidation;

namespace Presentation.Validators.Emails
{
    public class SendUserEmailCommandValidator : AbstractValidator<SendUserEmailCommand>
    {
        public SendUserEmailCommandValidator()
        {
            RuleFor(x => x.UserId).GreaterThan(0).WithMessage("UserId must be greater than 0.");
            RuleFor(x => x.Subject).NotEmpty().WithMessage("Subject is required.");
            RuleFor(x => x.Body).NotEmpty().WithMessage("Body is required.");
        }
    }
}
