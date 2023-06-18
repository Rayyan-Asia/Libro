using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.Interfaces;
using Libro.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class BookGenreRepository : IBookGenreRepository
    {
        private readonly LibroDbContext _context;

        public BookGenreRepository(LibroDbContext context)
        {
            _context = context;
        }

        public async Task RemoveGenresFromBook(int bookId)
        {
            var listToRemove = await _context.BookGenres.Where(ba => ba.BookId == bookId).ToListAsync();
            _context.BookGenres.RemoveRange(listToRemove);
            await _context.SaveChangesAsync();
        }
    }
}
