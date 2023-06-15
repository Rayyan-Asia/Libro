using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain;

namespace Infrastructure.Interfaces
{
    public interface IAuthorRepository
    {
        public Task<(PaginationMetadata, List<Author>)> GetAuthorsAsync(int pageNumber, int pageSize);
        Task<bool> AuthorExistsAsync(int authorId);
        Task<Author?> GetAuthorByIdAsync(int authorId);
    }
}
