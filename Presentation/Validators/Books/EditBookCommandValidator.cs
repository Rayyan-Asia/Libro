using Application.Entities.Books.Commands;
using FluentValidation;

namespace Presentation.Validators.Books
{
    public class EditBookCommandValidator : AbstractValidator<EditBookCommand>
    {
        public EditBookCommandValidator()
        {
            RuleFor(command => command.Id)
                .NotEmpty().WithMessage("Book ID is required.");

            RuleFor(command => command.Title)
                .NotEmpty().WithMessage("Title is required.")
                .MaximumLength(100).WithMessage("Title cannot exceed 100 characters.");

            RuleFor(command => command.Description)
                .MaximumLength(500).WithMessage("Description cannot exceed 500 characters.");

            RuleFor(command => command.PublicationDate)
                .NotEmpty().WithMessage("Publication date is required.")
                .Must(BeAValidDate).WithMessage("Invalid publication date.");

            RuleForEach(command => command.Genres)
                .SetValidator(new IdDtoValidator());

            RuleForEach(command => command.Authors)
                .SetValidator(new IdDtoValidator());
        }

        private bool BeAValidDate(DateTime date)
        {
            return date != default && date <= DateTime.Now;
        }
    }
}
