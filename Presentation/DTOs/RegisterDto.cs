using System.ComponentModel.DataAnnotations;
using Domain;

namespace Presentation.DTOs
{
    public class RegisterDto
    {
        [Required]
        [MaxLength(32)]
        public string Name { get; set; }

        [Required]
        [Phone]
        public string PhoneNumber { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [MinLength(6)]
        public string Password { get; set; }

        public Role Role { get; set; } = Role.Patron;
    }

}
