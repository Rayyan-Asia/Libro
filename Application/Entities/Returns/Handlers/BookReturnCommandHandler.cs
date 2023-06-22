using Application.DTOs;
using Application.Entities.Returns.Commands;
using Application.Entities.Returns.Queries;
using AutoMapper;
using Domain;
using Application.Interfaces;
using MediatR;

namespace Application.Entities.Returns.Handlers
{
    public class BookReturnCommandHandler : IRequestHandler<BookReturnCommand, BookReturnDto>
    {
        private readonly IBookReturnRepository _bookReturnRepository;
        private readonly ILoanRepository _loanRepository;
        private readonly IMapper _mapper;

        public BookReturnCommandHandler(IBookReturnRepository repository, ILoanRepository loanRepository, IMapper mapper)
        {
            _loanRepository = loanRepository;
            _bookReturnRepository = repository;
            _mapper = mapper;
        }

        public async Task<BookReturnDto> Handle(BookReturnCommand request, CancellationToken cancellationToken)
        {
            var loan = await _loanRepository.GetLoanByIdAsync(request.LoanId);
            if (loan == null)
            {
                return null;
            }

            if (loan.UserId != request.UserId )
                return null;

            if (await _bookReturnRepository.GetReturnByLoanIdAsync(loan.Id) != null) // makes sure the user make another return on the same loan.
                return null;

            var bookReturn = new BookReturn
            {
                LoanId = loan.Id,
                IsApproved = false,
                ReturnDate = DateTime.UtcNow,
            };

            bookReturn = await _bookReturnRepository.AddReturnAsync(bookReturn);

            var bookReturnDto = _mapper.Map<BookReturnDto>(bookReturn);
            return bookReturnDto;
        }
    }
}
