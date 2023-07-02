using Application.DTOs;
using Application.Entities.Reservations.Commnads;
using Application.Interfaces;
using AutoMapper;
using Domain;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Serilog;

namespace Application.Entities.Reservations.Handlers
{
    public class ReserveBookCommandHandler : IRequestHandler<ReserveBookCommand, IActionResult>
    {
        private readonly IReservationRepository _reservationRepository;
        private readonly IBookRepository _bookRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<ReserveBookCommandHandler> _logger;

        public ReserveBookCommandHandler(IReservationRepository reservationRepository, IBookRepository bookRepository, IMapper mapper, ILogger<ReserveBookCommandHandler> logger)
        {
            _reservationRepository = reservationRepository;
            _bookRepository = bookRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IActionResult> Handle(ReserveBookCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Retrieving book with ID {request.BookId}");
            var book = await _bookRepository.GetBookByIdAsync(request.BookId);

            if (book == null)
            {
                _logger.LogError($"Book NOT FOUND with ID {request.BookId}");
                return new NotFoundObjectResult("Book not found with ID " + request.BookId);
            }
            if (!book.IsAvailable)
            {
                _logger.LogError($"Book is NOT AVAILABLE with ID {request.BookId}");
                return new BadRequestObjectResult("Book is not available for reservation.");
            }
            _logger.LogInformation($"Checking eligibility of user with ID {request.UserId}");
            if (!await _reservationRepository.IsPatronEligableForReservationAsync(request.UserId))
            {
                _logger.LogError($"User NOT ELIGIBLE with ID {request.UserId}");
                return new BadRequestObjectResult("User is not eligible for the reservation.");
            }

            _logger.LogInformation($"Reserving book with ID {request.BookId}");
            await _bookRepository.ReserveBookAsync(book);

            var reservation = new Reservation()
            {
                UserId = request.UserId,
                BookId = request.BookId,
            };

            _logger.LogInformation($"Retrieving previous reservation with book ID {request.BookId} and user ID {request.UserId}");
            var oldReservation = await _reservationRepository.GetReservationByUserIdAndBookIdAsync(request.UserId, request.BookId);
            if (oldReservation != null && oldReservation.IsPendingApproval == true)
            {
                _logger.LogError($"User with ID {request.UserId} already has a reservation with this book {book.Id}");
                return new BadRequestObjectResult("User already has a pending reservation for this book.");
            }

            _logger.LogInformation($"Adding reservation");
            reservation = await _reservationRepository.AddReservationAsync(reservation);

            var result = _mapper.Map<ReservationDto>(reservation);
            return new OkObjectResult(result);
        }
    }
}
