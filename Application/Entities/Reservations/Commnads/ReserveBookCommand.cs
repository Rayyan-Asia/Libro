using System.ComponentModel.DataAnnotations;
using Application.DTOs;
using MediatR;

namespace Application.Entities.Reservations.Commnads
{
    public class ReserveBookCommand : IRequest<ReservationDto>
    {
        [Required]
        public int BookId { get; set; }

        [Required]
        public int UserId { get; set; }
    }
}
