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
using Microsoft.Extensions.Logging;

namespace Application.Entities.Reservations.Handlers
{
    public class ApproveReservationCommandHandler : IRequestHandler<ApproveReservationCommand, LoanDto>
    {
        private readonly IMapper _mapper;
        private readonly ILoanRepository _loanRepository;
        private readonly IReservationRepository _reservationRepository;
        private readonly IBookRepository _bookRepository;
        private readonly ILogger<ApproveReservationCommandHandler> _logger;

        public ApproveReservationCommandHandler(IMapper mapper, ILoanRepository loanRepository,
            IReservationRepository reservationRepository, IBookRepository bookRepository, ILogger<ApproveReservationCommandHandler> logger)
        {
            _mapper = mapper;
            _loanRepository = loanRepository;
            _reservationRepository = reservationRepository;
            _bookRepository = bookRepository;
            _logger = logger;
        }

        public async Task<LoanDto> Handle(ApproveReservationCommand request, CancellationToken cancellationToken)
        {
            int reservationId = request.ReservationId;
            _logger.LogInformation($"Retrieving reservation with ID {reservationId}");
            var reservation = await _reservationRepository.GetReservationByIdAsync(reservationId);
            if (reservation == null)
            {
                _logger.LogError($"Reservation NOT FOUND with ID {reservationId}");
                return null;
            }
            _logger.LogInformation($"Checking if user with ID {reservation.UserId} is eligable for reservation");
            if (!await _loanRepository.IsPatronEligableForLoanAsync(reservation.UserId))
            {
                _logger.LogError($"Resejected reservation with ID {reservationId}");
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

            _logger.LogInformation($"Adding loan from reservation with Id {reservationId}");
            loan = await _loanRepository.AddLoanAsync(loan);

            var result = _mapper.Map<LoanDto>(loan);
            return result;
        }
    }
}
