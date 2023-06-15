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
    }
}
