using System.ComponentModel.DataAnnotations;

namespace Application.DTOs
{
    public class IdDto
    {
        [Required]
        public int Id { get; set; }
    }
}
