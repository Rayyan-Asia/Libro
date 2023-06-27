using System.Net;
using Application.Interfaces;
using AutoDependencyRegistration.Attributes;
using Domain;
using Libro.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    [RegisterClassAsScoped]
    public class BookAuthorRepository : IBookAuthorRepository
    {

        private readonly LibroDbContext _context;

        public BookAuthorRepository(LibroDbContext context)
        {
            _context = context;
        }

        public async Task RemoveAuthorsFromBookAsync(int bookId)
        {
            var listToRemove = await _context.BookAuthors.Where(ba => ba.BookId == bookId).ToListAsync();
            _context.BookAuthors.RemoveRange(listToRemove);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveBooksFromAuthorAsync(int authorId)
        {
            var listToRemove = await _context.BookAuthors.Where(ba => ba.AuthorId == authorId).ToListAsync();
            _context.BookAuthors.RemoveRange(listToRemove);
            await _context.SaveChangesAsync();
        }

        public async Task<int> GetBookAuthorsCountAsync(int bookId)
        {
            var list = await _context.BookAuthors.Where(ba => ba.BookId == bookId).AsNoTracking().ToListAsync();
            return list.Count;
        }

        public async Task<List<BookAuthor>> GetAuthorBooksAsync(int authorId)
        {
            return await _context.BookAuthors.Where(ba => ba.AuthorId == authorId).AsNoTracking().ToListAsync();
        }
    }
}
