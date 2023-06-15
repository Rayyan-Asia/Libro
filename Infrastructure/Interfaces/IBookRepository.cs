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
        public Task<Book?> GetBookByIdAsync(int bookId);
        public Task<Book?> ReserveBookAsync(Book book);
        public Task<Book?> ChangeBookAsAvailableAsync(Book book);
        public Task<Book?> AddBookAsync(Book book);
        public Task<Book?> UpdateBookAsync(Book book);

    }
}
