using System.ComponentModel.DataAnnotations;

namespace Domain
{
    public class Book
    {
        public int Id { get; set; }

        [MaxLength(100)]
        [Required]
        public string Title { get; set; }

        [MaxLength(500)]
        public string Description { get; set; }

        [Required]
        public int AuthorId { get; set; }
        public Author Author { get; set; }

    }
}
