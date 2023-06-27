using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class GenreDto
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(32)]
        public string Type { get; set; }

    }
}
