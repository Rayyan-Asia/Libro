using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain;

namespace Infrastructure.Interfaces
{
    public interface IGenreRepository
    {
        Task<bool> GenreExistsAsync(int genreId);
        Task<Genre?> GetGenreByIdAsync(int genreId);
    }
}
