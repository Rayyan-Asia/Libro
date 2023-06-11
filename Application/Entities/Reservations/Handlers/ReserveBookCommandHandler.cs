using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTOs;
using Application.Entities.Reservations.Commnads;
using AutoMapper;
using Domain;
using Infrastructure.Interfaces;
using MediatR;

namespace Application.Entities.Reservations.Handlers
{
    public class ReserveBookCommandHandler : IRequestHandler<ReserveBookCommand, ReservationDto>
    {
        private readonly IReservationRepository _reservationRepository;
        private readonly IBookRepository _bookRepository;
        private readonly IMapper _mapper;

        public ReserveBookCommandHandler(IReservationRepository reservationRepository, IBookRepository bookRepository, IMapper mapper)
        {
            _reservationRepository = reservationRepository;
            _bookRepository = bookRepository;
            _mapper = mapper;
        }

        public async Task<ReservationDto> Handle(ReserveBookCommand request, CancellationToken cancellationToken)
        {
            var book = await _bookRepository.GetBookById(request.BookId);

            if(book==null)
                return null;
            if (!book.IsAvailable)
                return null;
            var reservation = new Reservation() {
                UserId = request.UserId,
                BookId = request.BookId,
            };
            if (await _reservationRepository.GetReservationByUserIdAndBookIdAsync(request.UserId, request.BookId) != null)
                return null;

            reservation = await _reservationRepository.AddReservationAsync(reservation);
            
            return _mapper.Map<ReservationDto>(reservation);
        }
    }
}
