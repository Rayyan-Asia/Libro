using System.ComponentModel.DataAnnotations;

namespace Application.DTOs
{
    public class OverdueLoanDto
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

        public double Fee { get; set; }

    }
}
