using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain;

namespace Application.DTOs
{
    public class LoanDto 
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public int BookId { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public DateTime LoanDate { get; set; }

        [Required]
        public DateTime DueDate { get; set; }
        public DateTime? ReturnDate { get; set; }

        public bool isExcused { get; set; } = false;
    }
}
