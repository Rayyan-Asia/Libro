using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain;
using Infrastructure.Interfaces;
using Libro.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class BookAuthorRepository : IBookAuthorRepository
    {

        private readonly LibroDbContext _context;

        public BookAuthorRepository(LibroDbContext context)
        {
            _context = context;
        }

        public async Task RemoveAuthorsFromBook(int bookId)
        {
            var listToRemove = await _context.BookAuthors.Where(ba => ba.BookId == bookId).ToListAsync();
            _context.BookAuthors.RemoveRange(listToRemove);
            await _context.SaveChangesAsync();
        } 
    }
}
