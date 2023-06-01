using System.ComponentModel.DataAnnotations;

namespace Domain
{
    public class Loan
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public int BookId { get; set; }
        public Book? Book { get; set; }

        [Required]
        public int UserId { get; set; }
        public User? User { get; set; }

        [Required]
        public DateTime LoanDate { get; set; }

        [Required]
        public DateTime DueDate { get; set; }
        public DateTime? ReturnDate { get; set; }
    }
}
