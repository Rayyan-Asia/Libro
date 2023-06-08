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
                  // Include other scalar properties you need

                  Genres = b.Genres.Select(g => new Genre
                  {
                      Id = g.Id,
                      Type = g.Type
                      // Include other scalar properties you need from Genre
                  }).ToList(),

                  Authors = b.Authors.Select(a => new Author
                  {
                      Id = a.Id,
                      Name = a.Name
                      // Include other scalar properties you need from Author
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

            var filteredDoctors = books.OrderBy(b => b.PublicationDate)
                .Skip(pageNumber * pageSize).Take(pageSize).ToList();
            return (metadata, filteredDoctors);
        }
    }
}
