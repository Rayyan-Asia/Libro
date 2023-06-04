using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class Genre
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(32)]
        public string Type { get; set; }

        public List<Book> Books { get; set; }

        public Genre() { 
        Books = new List<Book>();
        }
    }
}
