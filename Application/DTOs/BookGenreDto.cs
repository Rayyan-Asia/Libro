using System.ComponentModel.DataAnnotations;

namespace Application.DTOs
{
    public class BookGenreDto
    {
        [Required]
        public int Id { get; set; }
    }
}
