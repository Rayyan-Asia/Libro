using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain;

namespace Infrastructure.Interfaces
{
    public interface IBookAuthorRepository
    {
        Task RemoveAuthorsFromBook(int bookId);
        Task RemoveBooksFromAuthor(int authorId);
        Task<int> GetBookAuthorsCountAsync(int bookId);
        Task<List<BookAuthor>> GetAuthorBooksAsync(int authorId);
    }
}
