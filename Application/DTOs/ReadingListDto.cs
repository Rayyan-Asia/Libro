using System.ComponentModel.DataAnnotations;
using Domain;

namespace Application.DTOs
{
    public class ReadingListDto
    {

        [Required]
        public int Id { get; set; }

        [MaxLength(100)]
        [Required]
        public string Name { get; set; }

        [MaxLength(500)]
        public string Description { get; set; }

        [Required]
        public DateTime CreationDate { get; set; }

        public List<BookDto> Books { get; set; }

    }
}
