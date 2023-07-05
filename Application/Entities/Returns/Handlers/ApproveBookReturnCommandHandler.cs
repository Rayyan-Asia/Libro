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
using Domain;

namespace Application.Entities.Returns.Handlers
{
    public class ApproveBookReturnCommandHandler : IRequestHandler<ApproveBookReturnCommand, IActionResult>
    {
        private readonly IBookReturnRepository _bookReturnRepository;
        private readonly ILoanRepository _loanRepository;
        private readonly IBookRepository _bookRepository;
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;
        private readonly ILogger<ApproveBookReturnCommandHandler> _logger;
        private readonly IMailService _mailService;
        private readonly IJobService _jobService;

        public ApproveBookReturnCommandHandler(IBookReturnRepository bookReturnRepository, ILoanRepository loanRepository,
            IBookRepository bookRepository, IMapper mapper, ILogger<ApproveBookReturnCommandHandler> logger, IUserRepository userRepository, IMailService mailService, IJobService jobService)
        {
            _bookReturnRepository = bookReturnRepository;
            _loanRepository = loanRepository;
            _bookRepository = bookRepository;
            _mapper = mapper;
            _logger = logger;
            _userRepository = userRepository;
            _mailService = mailService;
            _jobService = jobService;
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
            await SendBookReturnEmail(bookReturn.Loan.UserId, book);
            bookReturn = await _bookReturnRepository.SetBookReturnApprovedAsync(bookReturn);
            await _jobService.RemoveJobsAsync(loan.Jobs);
            return new OkObjectResult(_mapper.Map<BookReturnDto>(bookReturn));
        }

        private async Task SendBookReturnEmail(int userId, Book book)
        {
            User user = await _userRepository.GetUserByIdAsync(userId);
            string to = user.Name;
            string toAddress = user.Email;
            string subject = "Libro Book Return";
            string body = $"Hello There {user.Name},\nYou have been approved for a book return for the book {book.Title}.\nThanks for returning the book.";
            await _mailService.SendEmailAsync(toAddress, to, subject, body);
        }
    }
}
