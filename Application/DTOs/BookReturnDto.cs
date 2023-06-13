using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain;

namespace Application.DTOs
{
    public class BookReturnDto
    {
        public int Id { get; set; }
        [Required]
        public int LoanId { get; set; }

        [Required]
        public DateTime ReturnDate { get; set; } = DateTime.Now;

        public bool IsApproved { get; set; }
    }
}
