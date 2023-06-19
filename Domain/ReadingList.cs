using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class ReadingList
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }

        public User? User { get; set; }

        public List<Book> Books { get; set; }

        [Required]
        public string Name { get; set; }
        
        [Required]
        public string Description { get; set; }

        [Required]
        public DateTime CreationDate { get; set; }

        public ReadingList()
        {
            Books = new();
        }
    }
}
