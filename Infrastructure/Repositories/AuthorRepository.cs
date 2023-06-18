using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Domain;
using Infrastructure.Interfaces;
using Libro.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class AuthorRepository : IAuthorRepository
    {
        private readonly LibroDbContext _context;

        public AuthorRepository(LibroDbContext context)
        {
            _context = context;
        }

        public async Task<(PaginationMetadata, List<Author>)> GetAuthorsAsync(int pageNumber, int pageSize)
        {
            var authors = await _context.Authors
              .AsNoTracking()
              .Include(a=>a.Books)
              .Select(a => new Author
              {
                  Id = a.Id,
                  Name = a.Name,
                  Description = a.Description,
                  Books = a.Books.Select(b => new Book
                  {
                      Id = b.Id,
                      Title = b.Title,
                  }).ToList(),
              })
              .ToListAsync();

            var count = authors.Count;

            var metadata = new PaginationMetadata()
            {
                ItemCount = count,
                PageCount = pageNumber,
                PageSize = pageSize
            };

            var filteredAuthors = authors.Skip(pageNumber * pageSize)
                .Take(pageSize).ToList();
            return (metadata, filteredAuthors);
        }

        public async Task<bool> AuthorExistsAsync(int authorId)
        {
            return await _context.Authors.AnyAsync(a => a.Id == authorId);
        }

        public async Task<Author?> GetAuthorByIdAsync(int authorId)
        {
            return await _context.Authors.SingleOrDefaultAsync(a => a.Id == authorId);
        }

        public async Task<Author> AddAuthorAsync(Author author)
        {
            _context.Authors.Add(author);
            await _context.SaveChangesAsync();
            return author;
        }

        public async Task<Author> UpdateAuthorAsync(Author author)
        {
            _context.Authors.Update(author);
            await _context.SaveChangesAsync();
            return author;
        }

        public async Task RemoveAuthorAsync(Author author)
        {
            _context.Authors.Remove(author);
            await _context.SaveChangesAsync();
        }

        public async Task<Author?> GetAuthorByIdIncludingCollectionsAsync(int authorId)
        {
            return await _context.Authors.Include(a=>a.Books).SingleOrDefaultAsync(a => a.Id == authorId);
        }
    }
}
