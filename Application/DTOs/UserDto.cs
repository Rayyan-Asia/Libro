using System.ComponentModel.DataAnnotations;

namespace Application.DTOs
{
    public class UserDto
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(32)]
        public string Name { get; set; }

        [Required]
        [Phone]
        public string PhoneNumber { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }


    }
}
