using Application;
using Application.Interfaces;
using AutoDependencyRegistration.Attributes;
using Domain;
using Libro.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    [RegisterClassAsScoped]
    public class LoanRepository : ILoanRepository
    {
        private readonly LibroDbContext _context;

        public LoanRepository(LibroDbContext context)
        {
            _context = context;
        }

        public async Task<Loan> AddLoanAsync(Loan loan)
        {
            _context.Loans.Add(loan);
            await _context.SaveChangesAsync();
            return loan;
        }

        public async Task<(PaginationMetadata, List<Loan>)> GetAllLoansAsync(int pageNumber, int pageSize)
        {
            var loans = await _context.Loans
              .AsNoTracking().Where(b => b.ReturnDate == null && !b.isExcused).ToListAsync();

            var count = loans.Count;

            var pageCount = (int)Math.Ceiling((double)count / pageSize) - 1;

            if (pageNumber > pageCount) pageNumber = pageCount;

            var metadata = new PaginationMetadata()
            {
                ItemCount = count,
                PageCount = pageNumber + 1,
                PageSize = pageSize
            };
            var take = pageSize - count % pageSize;
            var filteredLoans = loans.OrderBy(b => b.DueDate)
                .Skip(pageNumber * pageSize).Take(take).ToList();
            return (metadata, filteredLoans);
        }

        public async Task<(PaginationMetadata, List<Loan>)> GetAllLoansByUserIdAsync(int pageNumber, int pageSize, int userId)
        {
            var loans = await _context.Loans
              .AsNoTracking().Where(b => b.ReturnDate == null && b.UserId == userId && !b.isExcused).ToListAsync();

            var count = loans.Count;

            var pageCount = (int)Math.Ceiling((double)count / pageSize) - 1;

            if (pageNumber > pageCount) pageNumber = pageCount;

            var metadata = new PaginationMetadata()
            {
                ItemCount = count,
                PageCount = pageNumber + 1,
                PageSize = pageSize
            };
            var take = pageSize - count % pageSize;
            var filteredLoans = loans.OrderBy(b => b.DueDate)
                .Skip(pageNumber * pageSize).Take(take).ToList();
            return (metadata, filteredLoans);
        }

        public async Task<List<Loan>> GetAllOverdueLoansByUserIdWithoutPaginationAsync(int userId)
        {
            return await _context.Loans
              .AsNoTracking().Where(b => b.ReturnDate == null && b.UserId == userId && b.DueDate < DateTime.Now && !b.isExcused).ToListAsync();
        }

        public async Task<(PaginationMetadata, List<Loan>)> GetAllOverdueLoansByUserIdAsync(int pageNumber, int pageSize, int userId)
        {
            var loans = await _context.Loans
              .AsNoTracking().Where(b => b.ReturnDate == null && b.UserId == userId && b.DueDate < DateTime.Now && !b.isExcused).ToListAsync();

            var count = loans.Count;

            var pageCount = (int)Math.Ceiling((double)count / pageSize) - 1;

            if (pageNumber > pageCount) pageNumber = pageCount;

            var metadata = new PaginationMetadata()
            {
                ItemCount = count,
                PageCount = pageNumber + 1,
                PageSize = pageSize
            };
            var take = pageSize - count % pageSize;
            var filteredLoans = loans.OrderBy(b => b.DueDate)
                .Skip(pageNumber * pageSize).Take(take).ToList();
            return (metadata, filteredLoans);
        }

        public async Task<(PaginationMetadata, List<Loan>)> GetAllOverdueLoansAsync(int pageNumber, int pageSize)
        {
            var loans = await _context.Loans
              .AsNoTracking().Where(b => b.ReturnDate == null && !b.isExcused && b.DueDate < DateTime.Now).ToListAsync();

            var count = loans.Count;

            var pageCount = (int)Math.Ceiling((double)count / pageSize) - 1;

            if (pageNumber > pageCount) pageNumber = pageCount;

            var metadata = new PaginationMetadata()
            {
                ItemCount = count,   
                PageCount = pageNumber + 1,
                PageSize = pageSize
            };
            var take = pageSize - count % pageSize;
            var filteredLoans = loans.OrderBy(b => b.DueDate)
                .Skip(pageNumber * pageSize).Take(take).ToList();
            return (metadata, filteredLoans);
        }

        public async Task<List<Loan>> GetAllLoansByUserIdWithoutPaginationAsync(int userId)
        {
            return await _context.Loans
              .AsNoTracking().Where(b => b.ReturnDate == null && b.UserId == userId && !b.isExcused).ToListAsync();
        }

        public async Task<Loan?> GetLoanByIdAsync(int loanId)
        {
            var loan = await _context.Loans.Include(l=>l.Jobs).SingleOrDefaultAsync(l => l.Id == loanId);
            return loan;
        }

        public async Task<bool> IsPatronEligableForLoanAsync(int userId)
        {
            var count = await _context.Loans.Where(b => b.ReturnDate == null && b.UserId == userId && !b.isExcused).CountAsync();
            return count < 5;
        }

        public async Task<Loan> SetLoanReturnDateAsync(Loan loan, DateTime date)
        {
            loan.ReturnDate = date;
            await _context.SaveChangesAsync();
            return loan;
        }


        public async Task<bool> ExcuseLoanAsync(int loanId)
        {
            var loan = await GetLoanByIdAsync(loanId);
            if (loan == null)
            {
                return false;
            }
            loan.Jobs = new List<Job>();
            loan.isExcused = true;
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<bool> ExcuseLoansFromUserAsync(int userId)
        {
            var loans = await _context.Loans.Where(loan => loan.UserId == userId).ToListAsync();

            foreach(var loan in loans ) 
            {
                loan.isExcused = true;
                loan.Jobs = new List<Job>();
            }

            await _context.SaveChangesAsync();
            return loans.Count > 0;
        }

    }
}
