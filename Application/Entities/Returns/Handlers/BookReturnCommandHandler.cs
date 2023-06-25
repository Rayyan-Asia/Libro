using Application.DTOs;
using Application.Entities.Returns.Commands;
using Application.Entities.Returns.Queries;
using AutoMapper;
using Domain;
using Application.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Entities.Returns.Handlers
{
    public class BookReturnCommandHandler : IRequestHandler<BookReturnCommand, BookReturnDto>
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

        public async Task<BookReturnDto> Handle(BookReturnCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Retrieving loan with ID {request.LoanId}");
            var loan = await _loanRepository.GetLoanByIdAsync(request.LoanId);
            if (loan == null)
            {
                _logger.LogError($"Loan NOT FOUND with ID {request.LoanId}");
                return null;
            }

            if (loan.UserId != request.UserId)
            {
                _logger.LogError($"User IS NOT THE OWNER of the loan with ID {loan.Id}");
                return null;
            }

            _logger.LogInformation($"Retrieving loan with ID {loan.Id}");
            if (await _bookReturnRepository.GetReturnByLoanIdAsync(loan.Id) != null) // makes sure the user make another return on the same loan.
            {
                _logger.LogError($"User making another return on the same loan with ID {loan.Id}");
                return null; 
            }

            var bookReturn = new BookReturn
            {
                LoanId = loan.Id,
                IsApproved = false,
                ReturnDate = DateTime.UtcNow,
            };

            _logger.LogInformation($"Adding book return");
            bookReturn = await _bookReturnRepository.AddReturnAsync(bookReturn);

            var bookReturnDto = _mapper.Map<BookReturnDto>(bookReturn);
            return bookReturnDto;
        }
    }
}
