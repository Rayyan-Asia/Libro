using Application.Entities.Reservations.Commnads;
using FluentValidation;

namespace Presentation.Validators.Reservations
{
    public class ApproveReservationCommandValidator : AbstractValidator<ApproveReservationCommand>
    {
        public ApproveReservationCommandValidator()
        {
            RuleFor(command => command.ReservationId)
                .NotEmpty().WithMessage("Reservation ID is required.");
        }
    }
}
