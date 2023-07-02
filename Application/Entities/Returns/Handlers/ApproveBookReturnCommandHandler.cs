using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTOs;
using Application.Entities.Returns.Commands;
using AutoMapper;
using Application.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;

namespace Application.Entities.Returns.Handlers
{
    public class ApproveBookReturnCommandHandler : IRequestHandler<ApproveBookReturnCommand, IActionResult>
    {
        private readonly IBookReturnRepository _bookReturnRepository;
        private readonly ILoanRepository _loanRepository;
        private readonly IBookRepository _bookRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<ApproveBookReturnCommandHandler> _logger;

        public ApproveBookReturnCommandHandler(IBookReturnRepository bookReturnRepository, ILoanRepository loanRepository,
            IBookRepository bookRepository, IMapper mapper, ILogger<ApproveBookReturnCommandHandler> logger)
        {
            _bookReturnRepository = bookReturnRepository;
            _loanRepository = loanRepository;
            _bookRepository = bookRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IActionResult> Handle(ApproveBookReturnCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Retrieving book return with ID {request.BookReturnId}");
            var bookReturn = await _bookReturnRepository.GetReturnByIdAsync(request.BookReturnId);
            if (bookReturn == null)
            {
                _logger.LogError($"Book return NOT FOUND with ID {request.BookReturnId}");
                return new NotFoundObjectResult("Book return not found with ID " + request.BookReturnId);
            }
            if (bookReturn.IsApproved)
            {
                _logger.LogError($"Book return ALREADY APPROVED with ID {request.BookReturnId}");
                return new BadRequestObjectResult("Book return is already approved.");
            }
            _logger.LogInformation($"Retrieving loan with ID {bookReturn.LoanId}");
            var loan = await _loanRepository.GetLoanByIdAsync(bookReturn.LoanId);
            if (loan == null)
            {
                _logger.LogError($"Loan NOT FOUND with ID {bookReturn.LoanId}");
                return new NotFoundObjectResult("Loan not found with ID " + bookReturn.LoanId);
            }
            _logger.LogInformation($"Retrieving book with ID {loan.BookId}");
            var book = await _bookRepository.GetBookByIdAsync(loan.BookId);
            if (book == null)
            {
                _logger.LogError($"Book NOT FOUND with ID {loan.BookId}");
                return new NotFoundObjectResult("Book not found with ID " + loan.BookId);
            }
            _logger.LogInformation($"Returning book with ID {book.Id}");
            await _loanRepository.SetLoanReturnDateAsync(loan, bookReturn.ReturnDate);
            await _bookRepository.ChangeBookAsAvailableAsync(book);
            bookReturn = await _bookReturnRepository.SetBookReturnApprovedAsync(bookReturn);
            return new OkObjectResult(_mapper.Map<BookReturnDto>(bookReturn));
        }
    }
}
