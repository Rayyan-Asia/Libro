using Application.DTOs;
using Application.Entities.Returns.Commands;
using Application.Entities.Returns.Queries;
using AutoMapper;
using Domain;
using Application.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;

namespace Application.Entities.Returns.Handlers
{
    public class BookReturnCommandHandler : IRequestHandler<BookReturnCommand, IActionResult>
    {
        private readonly IBookReturnRepository _bookReturnRepository;
        private readonly ILoanRepository _loanRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<BookReturnCommandHandler> _logger;

        public BookReturnCommandHandler(IBookReturnRepository repository, ILoanRepository loanRepository, IMapper mapper, ILogger<BookReturnCommandHandler> logger)
        {
            _loanRepository = loanRepository;
            _bookReturnRepository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IActionResult> Handle(BookReturnCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Retrieving loan with ID {request.LoanId}");
            var loan = await _loanRepository.GetLoanByIdAsync(request.LoanId);
            if (loan == null)
            {
                _logger.LogError($"Loan NOT FOUND with ID {request.LoanId}");
                return new NotFoundObjectResult("Loan not found with ID " + request.LoanId);
            }

            if (loan.UserId != request.UserId)
            {
                _logger.LogError($"User IS NOT THE OWNER of the loan with ID {loan.Id}");
                return new ForbidResult("User is not the owner of the loan with ID " + loan.Id);
            }

            _logger.LogInformation($"Retrieving loan with ID {loan.Id}");
            if (await _bookReturnRepository.GetReturnByLoanIdAsync(loan.Id) != null)
            {
                _logger.LogError($"User making another return on the same loan with ID {loan.Id}");
                return new BadRequestObjectResult("User is making another return on the same loan with ID " + loan.Id);
            }

            var bookReturn = new BookReturn
            {
                LoanId = loan.Id,
                IsApproved = false,
                ReturnDate = DateTime.UtcNow,
            };

            _logger.LogInformation($"Adding book return");
            bookReturn = await _bookReturnRepository.AddReturnAsync(bookReturn);
            return new OkObjectResult(_mapper.Map<BookReturnDto>(bookReturn));
        }
    }
}
