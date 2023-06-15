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
    public class GenreRepository : IGenreRepository
    {
        private readonly LibroDbContext _context; 

        public GenreRepository(LibroDbContext context)
        {
            _context = context;
        }

        public async Task<bool> GenreExistsAsync(int genreId)
        {
            return await _context.Genres.AnyAsync(g => g.Id == genreId);
        }

        public async Task<Genre?> GetGenreByIdAsync(int genreId)
        {
            return await _context.Genres.SingleOrDefaultAsync(g => g.Id == genreId);
        }  
    }
}
