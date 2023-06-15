using Domain;
using Infrastructure.Interfaces;
using Libro.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class BookRepository : IBookRepository
    {
        private readonly LibroDbContext _context;

        public BookRepository(LibroDbContext context)
        {
            _context = context;
        }

        public async Task<(PaginationMetadata, List<Book>)> GetBooksAsync(int pageNumber, int pageSize)
        {
            var books = await _context.Books
              .Include(b => b.Genres)
              .Include(b => b.Authors)
              .AsNoTracking()
              .Select(b => new Book
              {
                  Id = b.Id,
                  Title = b.Title,
                  IsAvailable = b.IsAvailable,
                  Description = b.Description,
                  PublicationDate = b.PublicationDate,

                  Genres = b.Genres.Select(g => new Genre
                  {
                      Id = g.Id,
                      Type = g.Type
                  }).ToList(),

                  Authors = b.Authors.Select(a => new Author
                  {
                      Id = a.Id,
                      Name = a.Name
                  }).ToList()
              })
              .ToListAsync();

            var count = books.Count;

            var metadata = new PaginationMetadata()
            {
                ItemCount = count,
                PageCount = pageNumber,
                PageSize = pageSize
            };

            var filteredBooks = books.OrderBy(b => b.PublicationDate)
                .Skip(pageNumber * pageSize).Take(pageSize).ToList();
            return (metadata, filteredBooks);
        }
        public async Task<(PaginationMetadata, List<Book>)> SearchBooksAsync(string? title, string? author, string? genre, int pageNumber, int pageSize)
        {
            var query = _context.Books
                .Include(b => b.Genres)
                .Include(b => b.Authors)
                .AsNoTracking();

            if (!string.IsNullOrEmpty(title))
            {
                query = query.Where(b => b.Title.ToLower().Contains(title.ToLower()));
            }

            if (!string.IsNullOrEmpty(author))
            {
                query = query.Where(b => b.Authors.Any(a => a.Name.ToLower().Contains(author.ToLower())));
            }

            if (!string.IsNullOrEmpty(genre))
            {
                query = query.Where(b => b.Genres.Any(g => g.Type.ToLower().Contains(genre.ToLower())));
            }

            var totalCount = await query.CountAsync();

            var filteredBooks = await query
                .OrderBy(b => b.PublicationDate)
                .Skip(pageNumber * pageSize)
                .Take(pageSize)
                .Select(b => new Book
                {
                    Id = b.Id,
                    Title = b.Title,
                    IsAvailable = b.IsAvailable,
                    Description = b.Description,
                    PublicationDate = b.PublicationDate,
                    Genres = b.Genres.Select(g => new Genre
                    {
                        Id = g.Id,
                        Type = g.Type
                    }).ToList(),

                    Authors = b.Authors.Select(a => new Author
                    {
                        Id = a.Id,
                        Name = a.Name
                    }).ToList()
                })
                .ToListAsync();

            var metadata = new PaginationMetadata()
            {
                ItemCount = totalCount,
                PageCount = pageNumber,
                PageSize = pageSize
            };

            return (metadata, filteredBooks);
        }

        public async Task<Book?> GetBookByIdAsync(int bookId)
        {
            return await _context.Books.SingleOrDefaultAsync(b => b.Id == bookId);
        }

        public async Task<Book?> ReserveBookAsync(Book book)
        {
            book.IsAvailable = false;
            await _context.SaveChangesAsync();
            return book;
        }

        public async Task<Book?> ChangeBookAsAvailableAsync(Book book)
        {
            book.IsAvailable = true;
            await _context.SaveChangesAsync();
            return book;
        }

        public async Task<Book?> AddBookAsync(Book book)
        {
            _context.Books.Add(book);
            await _context.SaveChangesAsync();
            return book;
        }

        public async Task<Book?> UpdateBookAsync(Book book)
        {
            _context.Books.Update(book);
            await _context.SaveChangesAsync();
            return book;
        }
    }
}
