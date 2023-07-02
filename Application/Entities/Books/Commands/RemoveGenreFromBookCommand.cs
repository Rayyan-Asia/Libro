using System.ComponentModel.DataAnnotations;
using Application.DTOs;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Application.Entities.Books.Commands
{
    public class RemoveGenreFromBookCommand : IRequest<IActionResult>
    {
        [Required]
        public int BookId { get; set; }

        [Required]
        public int GenreId { get; set; }
    }

}
