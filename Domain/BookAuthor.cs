﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class BookAuthor
    {
        public Book? Book { get; set; }
        public int BookId { get; set; }
        public Author? Author { get; set; }
        public int AuthorId { get; set; }
        
    }
}
