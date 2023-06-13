using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class BookReturn
    {
        public int Id { get; set; }
        [Required]
        public int LoanId { get; set; }

        public Loan? Loan { get; set; }

        [Required]
        public DateTime ReturnDate { get; set; } = DateTime.Now;

        public bool IsApproved { get; set; } = false;
    }
}
