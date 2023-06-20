using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain;

namespace Application.DTOs
{
    public class FeedbackDto
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public Rating Rating { get; set; }

        [Required]
        [MaxLength(500)]
        public string Review { get; set; }
        public UserDto User { get; set; }
        public BookDto Book { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
