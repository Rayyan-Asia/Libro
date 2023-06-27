using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain;

namespace Application.DTOs
{
    public class BookDto
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

        public List<GenreDto> Genres { get; set; }

        public List<AuthorDto> Authors { get; set; }
        public bool IsAvailable { get; set; } = true;
    }
}
