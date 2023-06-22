using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain;

namespace Application.Interfaces
{
    public interface IBookReturnRepository
    {
        Task<BookReturn> AddReturnAsync(BookReturn bookReturn);
        Task<BookReturn?> GetReturnByIdAsync(int bookReturnId);
        Task<(PaginationMetadata, List<BookReturn>)> GetAllReturnsAsync(int pageNumber, int pageSize);
        Task<(PaginationMetadata, List<BookReturn>)> GetAllReturnsByUserIdAsync(int pageNumber, int pageSize, int userId);
        Task<BookReturn?> ApproveReturnByIdAsync(int ReturnId);
        Task<BookReturn?> GetReturnByLoanIdAsync(int loanId);
        Task<BookReturn> SetBookReturnApproved(BookReturn bookReturn);
    }
}
