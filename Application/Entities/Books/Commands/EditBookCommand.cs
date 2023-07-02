using System.ComponentModel.DataAnnotations;
using Application.DTOs;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Application.Entities.Books.Commands
{
    public class EditBookCommand : IRequest<IActionResult>
    {
        [Required]
        public int Id { get; set; }

        [MaxLength(100)]
        [Required]
        public string Title { get; set; }

        [MaxLength(500)]
        public string Description { get; set; }

        [Required]
        public DateTime PublicationDate { get; set; }

        public List<IdDto> Genres { get; set; }

        public List<IdDto> Authors { get; set; }
    }
}
