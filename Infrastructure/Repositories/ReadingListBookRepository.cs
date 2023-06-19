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
    public class ReadingListBookRepository : IReadingListBookRepository
    {
        private readonly LibroDbContext _context;

        public ReadingListBookRepository(LibroDbContext context)
        {
            _context = context;
        }

        public async Task RemoveBooksFromReadingList(int listId)
        {
            var list = await _context.ReadingListBooks.Where(l => l.ReadingListId == listId).ToListAsync();
            _context.ReadingListBooks.RemoveRange(list);
            await _context.SaveChangesAsync();
        }
    }
}
