using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain;
using Infrastructure.Interfaces;
using Libro.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class BookReturnRepository : IBookReturnRepository
    {
        private readonly LibroDbContext _context;

        public BookReturnRepository(LibroDbContext context)
        {
            _context = context;
        }
        public async Task<BookReturn> AddReturnAsync(BookReturn bookReturn)
        {
            _context.BookReturns.Add(bookReturn);
            await _context.SaveChangesAsync();
            return bookReturn;
        }

        public Task<BookReturn?> ApproveReturnByIdAsync(int ReturnId)
        {
            throw new NotImplementedException();
        }

        public async Task<(PaginationMetadata, List<BookReturn>)> GetAllReturnsAsync(int pageNumber, int pageSize)
        {
            var reservations = await _context.BookReturns.AsNoTracking()
                .Where(r => r.IsApproved == false).OrderBy(r => r.ReturnDate)
                .ToListAsync();

            var count = reservations.Count;

            var metadata = new PaginationMetadata()
            {
                ItemCount = count,
                PageCount = pageNumber,
                PageSize = pageSize
            };

            var filteredDoctors = reservations
                .Skip(pageNumber * pageSize).Take(pageSize).ToList();
            return (metadata, filteredDoctors);
        }

        public Task<(PaginationMetadata, List<BookReturn>)> GetAllReturnsByUserIdAsync(int pageNumber, int pageSize, int userId)
        {
            throw new NotImplementedException();
        }

        public async Task<BookReturn?> GetReturnByIdAsync(int bookReturnId)
        {
            return await _context.BookReturns.SingleOrDefaultAsync(b => b.Id == bookReturnId);
        }

        public async Task<BookReturn?> GetReturnByLoanIdAsync(int loanId)
        {
            return await _context.BookReturns.SingleOrDefaultAsync(b => b.LoanId == loanId && b.IsApproved == false);
        }

        public async Task<BookReturn> SetBookReturnApproved(BookReturn bookReturn)
        {
            bookReturn.IsApproved = true;
            await _context.SaveChangesAsync();
            return bookReturn;
        }
    }
}
