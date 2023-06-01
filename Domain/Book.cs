using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography.X509Certificates;

namespace Domain
{
    public class Book
    {
        [Required]
        public int Id { get; set; }

        [MaxLength(100)]
        [Required]
        public string Title { get; set; }

        [MaxLength(500)]
        public string Description { get; set; }

        public List<Genre> Genres { get; set; }
        public bool IsReserved { get; set; }

        [Required]
        public int AuthorId { get; set; }
        public Author Author { get; set; }
    }
}
