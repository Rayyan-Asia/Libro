using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain;

namespace Application.Interfaces
{
    public interface ILoanRepository
    {
        Task<Loan> AddLoanAsync(Loan loan);
        Task<Loan?> GetLoanByIdAsync(int loanId);
        Task<(PaginationMetadata, List<Loan>)> GetAllLoansAsync(int pageNumber, int pageSize);
        Task<(PaginationMetadata, List<Loan>)> GetAllLoansByUserIdAsync(int pageNumber, int pageSize, int userId);
        Task<(PaginationMetadata, List<Loan>)> GetAllOverdueLoansByUserIdAsync(int pageNumber, int pageSize, int userId);
        Task<(PaginationMetadata, List<Loan>)> GetAllOverdueLoansAsync(int pageNumber, int pageSize);
        Task<List<Loan>> GetAllOverdueLoansByUserIdWithoutPaginationAsync(int userId);
        Task<List<Loan>> GetAllLoansByUserIdWithoutPaginationAsync( int userId);
        Task<bool> IsPatronEligableForLoanAsync(int userId);
        Task<Loan> SetLoanReturnDateAsync(Loan loan, DateTime date);

        Task<bool> ExcuseLoanAsync(int loanId);

        Task<bool> ExcuseLoansFromUserAsync(int userId);


    }
}
