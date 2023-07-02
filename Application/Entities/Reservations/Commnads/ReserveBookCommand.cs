using System.ComponentModel.DataAnnotations;
using Application.DTOs;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Application.Entities.Reservations.Commnads
{
    public class ReserveBookCommand : IRequest<IActionResult>
    {
        [Required]
        public int BookId { get; set; }

        [Required]
        public int UserId { get; set; }
    }
}
