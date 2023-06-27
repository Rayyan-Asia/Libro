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

namespace Application.Entities.Returns.Handlers
{
    public class ApproveBookReturnCommandHandler : IRequestHandler<ApproveBookReturnCommand, BookReturnDto>
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

        public async Task<BookReturnDto> Handle(ApproveBookReturnCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Retrieving book return with ID {request.BookReturnId}");
            var bookReturn = await _bookReturnRepository.GetReturnByIdAsync(request.BookReturnId);
            if(bookReturn == null ) {
                _logger.LogError($"Book return NOT FOUND with ID {request.BookReturnId}");
                return null;
            }
            if (bookReturn.IsApproved)
            {
                _logger.LogError($"Book return ALREADY APPROVED with ID {request.BookReturnId}");
                return null;
            }
            _logger.LogInformation($"Retrieving book return with ID {bookReturn.LoanId}");
            var loan = await _loanRepository.GetLoanByIdAsync(bookReturn.LoanId);
            if(loan == null)
            {
                _logger.LogError($"Loan NOT FOUND with ID {bookReturn.LoanId}");
                return null;
            }
            _logger.LogInformation($"Retrieving book with ID {loan.BookId}");
            var book = await _bookRepository.GetBookByIdAsync(loan.BookId);
            if (book == null)
            {
                _logger.LogError($"Book NOT FOUND with ID {loan.BookId}");
                return null;
            }
            _logger.LogInformation($"Returning book with ID {book.Id}");
            await _loanRepository.SetLoanReturnDateAsync(loan, bookReturn.ReturnDate);
            await _bookRepository.ChangeBookAsAvailableAsync(book);
            bookReturn = await _bookReturnRepository.SetBookReturnApprovedAsync(bookReturn);
            var returnDto = _mapper.Map<BookReturnDto>(bookReturn);
            return returnDto;

        }
    }
}
