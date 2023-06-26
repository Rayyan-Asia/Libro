using System.Linq;
using Application;
using Application.Interfaces;
using AutoDependencyRegistration.Attributes;
using Domain;
using Libro.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    [RegisterClassAsScoped]
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

            var pageCount = (int)Math.Ceiling((double)count / pageSize) -1;

            if (pageNumber > pageCount) pageNumber = pageCount;

            var metadata = new PaginationMetadata()
            {
                ItemCount = count,
                PageCount = pageNumber+1,
                PageSize = pageSize
            };
            var take = pageSize - count % pageSize;
            var filteredBooks = books.OrderBy(b => b.PublicationDate)
                .Skip(pageNumber * pageSize).Take(take).ToList();
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
            return await _context.Books.Include(b => b.Genres).Include(b=>b.Authors).SingleOrDefaultAsync(b => b.Id == bookId);
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
            book.Genres = book.Genres.Select(g => new Genre
            {
                Id = g.Id,
                Type = g.Type
            }).ToList();

            book.Authors = book.Authors.Select(a => new Author
            {
                Id = a.Id,
                Name = a.Name
            }).ToList();
            return book;
        }

        public async Task RemoveBookAsync(Book book)
        {
            _context.Books.Remove(book);
            await _context.SaveChangesAsync();
        }

        public async Task<Book?> GetRecommendedBookAsync(int userId)
        {
            var user = await _context.Users.Include(u => u.Loans)
                                           .ThenInclude(l => l.Book)
                                           .ThenInclude(b=>b.Genres)
                                           .SingleOrDefaultAsync(u => u.Id == userId);

            if (user == null)
            {
                return null;
            }

            // Count the frequency of genres in the user's loan history
            var genreCounts = new Dictionary<Genre, int>();
            foreach (var loan in user.Loans)
            {
                foreach (var genre in loan.Book.Genres)
                {
                    if (!genreCounts.ContainsKey(genre))
                    {
                        genreCounts[genre] = 0;
                    }
                    genreCounts[genre]++;
                }
            }

            // Sort the genres based on frequency in descending order
            var sortedGenres = genreCounts.OrderByDescending(g => g.Value);

            var topGenre = sortedGenres.FirstOrDefault().Key;

            if (topGenre == null)
            {
                // The user has loaned books from all genres, so no recommendation can be made.
                return null;
            }

            // Get a random book from the top genre
            var recommendedBook = await _context.Books
                                  .Where(b => b.Genres.Any(genre => genre.Type == topGenre.Type)
                                                    && b.Id != user.Loans.Select(l => l.Book.Id).FirstOrDefault())
                                  .FirstOrDefaultAsync();

            return recommendedBook;
        }


    }
}
