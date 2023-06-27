using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain;

namespace Application.Interfaces
{
    public interface IBookAuthorRepository
    {
        Task RemoveAuthorsFromBookAsync(int bookId);
        Task RemoveBooksFromAuthorAsync(int authorId);
        Task<int> GetBookAuthorsCountAsync(int bookId);
        Task<List<BookAuthor>> GetAuthorBooksAsync(int authorId);
    }
}
