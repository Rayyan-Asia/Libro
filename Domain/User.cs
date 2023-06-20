using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class User
    {
        [Required]
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

        [Required]
        public Role Role { get; set; } = Role.Patron;

        [Required]
        public string Salt { get; set; }

        [Required]
        public string HashedPassword { get; set; }
        public List<Loan> Loans { get; set; }
        public List<Reservation> Reservations { get; set; }
        public List<ReadingList> ReadingLists { get; set; }
        public List<Feedback> Feedbacks { get; set; }
        public User()
        {
            Loans = new();
            Reservations = new();
            ReadingLists = new();
            Feedbacks = new();
        }
    }
}
