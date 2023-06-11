using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain;

namespace Infrastructure.Interfaces
{
    public interface IBookRepository
    {
        public Task<(PaginationMetadata, List<Book>)> GetBooksAsync(int pageNumber, int pageSize);
        public Task<(PaginationMetadata, List<Book>)> SearchBooksAsync(string? title, string? author, string? genre, int pageNumber, int pageSize);

        public Task<Book?> GetBookById(int bookId);
    }
}
