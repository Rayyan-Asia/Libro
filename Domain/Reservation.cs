using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class Reservation
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }
        public User? User { get; set; }

        [Required]
        public int BookId { get; set; }
        public Book? Book { get; set; }

        [Required]
        public DateTime ReservationDate { get; set; } = DateTime.Now;
        public bool IsPendingApproval { get; set; } = true;
        public bool IsApproved { get; set; } = false;
    }
}
