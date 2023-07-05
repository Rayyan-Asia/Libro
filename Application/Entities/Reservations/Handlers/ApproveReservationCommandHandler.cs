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
using Microsoft.AspNetCore.Mvc;
using Hangfire;

namespace Application.Entities.Reservations.Handlers
{
    public class ApproveReservationCommandHandler : IRequestHandler<ApproveReservationCommand, IActionResult>
    {
        private readonly IMapper _mapper;
        private readonly ILoanRepository _loanRepository;
        private readonly IReservationRepository _reservationRepository;
        private readonly IBookRepository _bookRepository;
        private readonly ILogger<ApproveReservationCommandHandler> _logger;
        private readonly IMailService _mailService;
        private readonly IUserRepository _userRepository;
        private readonly IJobRepository _jobRepository;


        public ApproveReservationCommandHandler(IMapper mapper, ILoanRepository loanRepository,
            IReservationRepository reservationRepository, IBookRepository bookRepository, ILogger<ApproveReservationCommandHandler> logger, IMailService mailService,
            IUserRepository userRepository, IJobRepository jobRepository)

        {
            _mapper = mapper;
            _loanRepository = loanRepository;
            _reservationRepository = reservationRepository;
            _bookRepository = bookRepository;
            _logger = logger;
            _mailService = mailService;
            _userRepository = userRepository;
            _jobRepository = jobRepository;
        }

        public async Task<IActionResult> Handle(ApproveReservationCommand request, CancellationToken cancellationToken)
        {
            int reservationId = request.ReservationId;
            _logger.LogInformation($"Retrieving reservation with ID {reservationId}");
            var reservation = await _reservationRepository.GetReservationByIdAsync(reservationId);
            if (reservation == null)
            {
                _logger.LogError($"Reservation NOT FOUND with ID {reservationId}");
                return new NotFoundObjectResult("Reservation not found with ID " + reservationId);
            }

            Book book = await _bookRepository.GetBookByIdAsync(reservation.BookId);

            _logger.LogInformation($"Checking if user with ID {reservation.UserId} is eligible for reservation");
            if (!await _loanRepository.IsPatronEligableForLoanAsync(reservation.UserId))
            {
                _logger.LogError($"Rejected reservation with ID {reservationId}");
                await _reservationRepository.RejectReservationByIdAsync(reservationId);


                await _bookRepository.ChangeBookAsAvailableAsync(book);

                return new NotFoundObjectResult("User is not eligible for the loan.");
            }
            book = await _bookRepository.GetBookWithoutTrackingByIdAsync(reservation.BookId);

            reservation.IsPendingApproval = false;
            reservation.IsApproved = true;

            await _reservationRepository.UpdateReservationAsync(reservation);

            Loan loan = new Loan()
            {
                BookId = reservation.BookId,
                UserId = reservation.UserId,
                LoanDate = DateTime.UtcNow,
                DueDate = DateTime.UtcNow.AddDays(28),
            };

            await SendNewLoanEmail(loan.UserId, book);
            await CreateJobs(reservationId, book, loan);
            loan = await _loanRepository.AddLoanAsync(loan);
            return new OkObjectResult(_mapper.Map<LoanDto>(loan));
        }

        public async Task CreateJobs(int reservationId, Book book, Loan loan)
        {
            var newGenres = new List<Genre>();
            var newAuthors = new List<Author>();

            // Assign values from existing lists to new instances
            foreach (var genre in book.Genres)
            {
                newGenres.Add(new Genre { Id = genre.Id, Type = genre.Type });
            }

            foreach (var author in book.Authors)
            {
                newAuthors.Add(new Author { Id = author.Id, Name = author.Name });
            }

            // Use the new lists with detached entities for further processing
            book.Genres = newGenres;
            book.Authors = newAuthors;
            var week = DateTime.Now.AddDays(21);
            var day = DateTime.Now.AddDays(27);
            var due = DateTime.Now.AddDays(28);
            var overdue = DateTime.Now.AddDays(29);
            var stringId = BackgroundJob.Schedule(() => SendLoanWeekBeforeDueEmail(loan.UserId, book), week);
            Job job = await _jobRepository.AddJobAsync(new Job { BackgroundJobId = stringId });
            loan.Jobs.Add(job);

            stringId = BackgroundJob.Schedule(() => SendLoanDayBeforeDueEmail(loan.UserId, book), day);
            job = await _jobRepository.AddJobAsync(new Job { BackgroundJobId = stringId });
            loan.Jobs.Add(job);

            stringId = BackgroundJob.Schedule(() => SendLoanDayDueEmail(loan.UserId, book), due);
            job = await _jobRepository.AddJobAsync(new Job { BackgroundJobId = stringId });
            loan.Jobs.Add(job);

            stringId = BackgroundJob.Schedule(() => SendLoanOverdueEmail(loan.UserId, book), overdue);
            job = await _jobRepository.AddJobAsync(new Job { BackgroundJobId = stringId });
            loan.Jobs.Add(job);

            _logger.LogInformation($"Adding loan from reservation with Id {reservationId}");
        }

        public async Task SendNewLoanEmail(int userId, Book book)
        {
            User user = await _userRepository.GetUserByIdAsync(userId);
            string to = user.Name;
            string toAddress = user.Email;
            string subject = "New Book Loan From Libro";
            string body = $"Hello There {user.Name},\nYou have been approved for a book loan for the book {book.Title}.\nPlease come to your nearest Libro station for pickup";
            await _mailService.SendEmailAsync(toAddress, to, subject, body);
        }

        public async Task SendLoanWeekBeforeDueEmail(int userId, Book book)
        {
            User user = await _userRepository.GetUserByIdAsync(userId);
            string to = user.Name;
            string toAddress = user.Email;
            string subject = "Book Loan Due Date From Libro";
            string body = $"Hello There {user.Name},\nYou have one week left with the book {book.Title}.\nPlease come to your nearest Libro station for returning it before your time runs out";
            await _mailService.SendEmailAsync(toAddress, to, subject, body);
        }
        public async Task SendLoanDayBeforeDueEmail(int userId, Book book)
        {
            User user = await _userRepository.GetUserByIdAsync(userId);
            string to = user.Name;
            string toAddress = user.Email;
            string subject = "Book Loan Due Date From Libro";
            string body = $"Hello There {user.Name},\nYou have one day left with the book {book.Title}.\nPlease come to your nearest Libro station for returning it before your time runs out";
            await _mailService.SendEmailAsync(toAddress, to, subject, body);
        }

        public async Task SendLoanDayDueEmail(int userId, Book book)
        {
            User user = await _userRepository.GetUserByIdAsync(userId);
            string to = user.Name;
            string toAddress = user.Email;
            string subject = "Last day to return book to Libro";
            string body = $"Hello There {user.Name},\nThe book: {book.Title} is due for a return today.\nPlease come to your nearest Libro station for returning it before your time runs out";
            await _mailService.SendEmailAsync(toAddress, to, subject, body);
        }

        public async Task SendLoanOverdueEmail(int userId, Book book)
        {
            User user = await _userRepository.GetUserByIdAsync(userId);
            string to = user.Name;
            string toAddress = user.Email;
            string subject = "Book Loan over due Libro";
            string body = $"Hello There {user.Name},\nThe book: {book.Title} is overdue for a return.\nPlease come to your nearest Libro station for returning it, and be cautious because there will be charges for overdue books";
            await _mailService.SendEmailAsync(toAddress, to, subject, body);
        }
    }
}
