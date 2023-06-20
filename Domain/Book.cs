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

        [Required]
        public DateTime PublicationDate { get; set; }
        public List<Author> Authors{ get; set; }

        public List<ReadingList> ReadingLists { get; set; }

        public List<Feedback> Feedbacks { get; set; }
        public bool IsAvailable { get; set; } = true;

        public Book()
        {
            Authors = new List<Author>();
            Genres = new List<Genre>();
            ReadingLists = new List<ReadingList>();
            Feedbacks = new List<Feedback>();
        }
    }
}
