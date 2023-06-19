using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class ReadingListBook
    {
        public int ReadingListId { get; set; }
        public ReadingList? ReadingList { get; set; }
        public int BookId { get; set; }
        public Book? Book { get; set; }
    }
}
