using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class Feedback
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public Rating Rating { get; set; }

        [Required]
        [MaxLength(500)]
        public string Review { get; set; }
        [Required]
        public int UserId { get; set; }
        public User? User { get; set; }

        [Required]
        public int BookId { get; set; }
        public Book? Book { get; set; }

        public DateTime CreatedDate { get; set; }
    }
}
