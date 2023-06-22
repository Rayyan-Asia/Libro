using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTOs;
using Application.Entities.Reservations.Commnads;
using AutoMapper;
using Domain;
using Application.Interfaces;
using MediatR;

namespace Application.Entities.Reservations.Handlers
{
    public class ApproveReservationCommandHandler : IRequestHandler<ApproveReservationCommand, LoanDto>
    {
        private readonly IMapper _mapper;
        private readonly ILoanRepository _loanRepository;
        private readonly IReservationRepository _reservationRepository;
        private readonly IBookRepository _bookRepository;

        public ApproveReservationCommandHandler(IMapper mapper, ILoanRepository loanRepository, IReservationRepository reservationRepository, IBookRepository bookRepository)
        {
            _mapper = mapper;
            _loanRepository = loanRepository;
            _reservationRepository = reservationRepository;
            _bookRepository = bookRepository;
        }

        public async Task<LoanDto> Handle(ApproveReservationCommand request, CancellationToken cancellationToken)
        {
            int reservationId = request.ReservationId;
            var reservation = await _reservationRepository.GetReservationByIdAsync(reservationId);
            if (reservation == null)
                return null;
            if (!await _loanRepository.IsPatronEligableForLoanAsync(reservation.UserId))
            {
                await _reservationRepository.RejectReservationByIdAsync(reservationId);
                var book = await _bookRepository.GetBookByIdAsync(reservation.BookId);
                await _bookRepository.ChangeBookAsAvailableAsync(book);
                return null;
            }

            Loan loan = new Loan()
            {
                BookId = reservation.BookId,
                UserId = reservation.UserId,
                LoanDate = DateTime.UtcNow,
                DueDate = DateTime.UtcNow.AddDays(28),
            };

            loan = await _loanRepository.AddLoanAsync(loan);

            var result = _mapper.Map<LoanDto>(loan);
            return result;
        }
    }
}
