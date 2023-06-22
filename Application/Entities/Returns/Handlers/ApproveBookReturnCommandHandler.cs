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

namespace Application.Entities.Returns.Handlers
{
    public class ApproveBookReturnCommandHandler : IRequestHandler<ApproveBookReturnCommand, BookReturnDto>
    {
        private readonly IBookReturnRepository _bookReturnRepository;
        private readonly ILoanRepository _loanRepository;
        private readonly IBookRepository _bookRepository;
        private readonly IMapper _mapper;

        public ApproveBookReturnCommandHandler(IBookReturnRepository bookReturnRepository, ILoanRepository loanRepository, IBookRepository bookRepository, IMapper mapper)
        {
            _bookReturnRepository = bookReturnRepository;
            _loanRepository = loanRepository;
            _bookRepository = bookRepository;
            _mapper = mapper;
        }

        public async Task<BookReturnDto> Handle(ApproveBookReturnCommand request, CancellationToken cancellationToken)
        {
            var bookReturn = await _bookReturnRepository.GetReturnByIdAsync(request.BookReturnId);
            if(bookReturn == null || bookReturn.IsApproved) {
                return null;
            }
            var loan = await _loanRepository.GetLoanByIdAsync(bookReturn.LoanId);
            if(loan == null)
            {
                return null;
            }
            var book = await _bookRepository.GetBookByIdAsync(loan.BookId);
            if (book == null)
            {
                return null;
            }
            await _loanRepository.SetLoanReturnDate(loan, bookReturn.ReturnDate);
            await _bookRepository.ChangeBookAsAvailableAsync(book);
            bookReturn = await _bookReturnRepository.SetBookReturnApproved(bookReturn);
            var returnDto = _mapper.Map<BookReturnDto>(bookReturn);
            return returnDto;

        }
    }
}
