using System.ComponentModel.DataAnnotations;
using Application.DTOs;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Application.Entities.Books.Commands
{
    public class RemoveAuthorFromBookCommand : IRequest<IActionResult>
    {
        [Required]
        public int BookId { get; set; }

        [Required]
        public int AuthorId { get; set; }
    }
}
