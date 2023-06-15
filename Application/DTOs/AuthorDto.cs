using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain;

namespace Application.DTOs
{
    public class AuthorDto
    {

        [Required]
        public int Id { get; set; }

        [Required]
        [MaxLength(32)]
        public string Name { get; set; }

        [MaxLength(500)]
        public string Description { get; set; }
    }
}
