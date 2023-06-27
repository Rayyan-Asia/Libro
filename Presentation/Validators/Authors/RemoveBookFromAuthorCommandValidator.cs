﻿using Application.Entities.Authors.Commands;
using FluentValidation;

namespace Presentation.Validators.Authors
{
    public class RemoveBookFromAuthorCommandValidator : AbstractValidator<RemoveBookFromAuthorCommand>
    {
        public RemoveBookFromAuthorCommandValidator()
        {
            RuleFor(command => command.AuthorId)
                .NotEmpty().WithMessage("Author ID is required.");

            RuleFor(command => command.BookId)
                .NotEmpty().WithMessage("Book ID is required.");
        }
    }
}
