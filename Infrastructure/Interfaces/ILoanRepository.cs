using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain;

namespace Infrastructure.Interfaces
{
    public interface ILoanRepository
    {
        Task<Loan> AddLoanAsync(Loan loan);
        Task<Loan?> GetLoanByIdAsync(int loanId);
        Task<(PaginationMetadata, List<Loan>)> GetAllLoansAsync(int pageNumber, int pageSize);
        Task<(PaginationMetadata, List<Loan>)> GetAllLoansByUserIdAsync(int pageNumber, int pageSize, int userId);
        Task<bool> IsPatronEligableForLoanAsync(int userId);
        Task<Loan> SetLoanReturnDate(Loan loan, DateTime date);
    }
}
