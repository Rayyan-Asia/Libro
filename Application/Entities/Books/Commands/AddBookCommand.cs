using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTOs;
using Domain;
using MediatR;

namespace Application.Entities.Books.Commands
{
    public class AddBookCommand : IRequest<BookDto>
    {
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
