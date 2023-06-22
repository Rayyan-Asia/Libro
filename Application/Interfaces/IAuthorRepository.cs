using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain;

namespace Application.Interfaces
{
    public interface IAuthorRepository
    {
        public Task<(PaginationMetadata, List<Author>)> GetAuthorsAsync(int pageNumber, int pageSize);
        Task<bool> AuthorExistsAsync(int authorId);
        Task<Author?> GetAuthorByIdAsync(int authorId);
        Task<Author> AddAuthorAsync(Author author);
        Task<Author> UpdateAuthorAsync(Author author);
        Task RemoveAuthorAsync(Author author);
        Task<Author?> GetAuthorByIdIncludingCollectionsAsync(int authorId);
    }
}
