using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Entities.Reservations.Commnads;
using Application.Entities.Reservations.Handlers;
using Application.Interfaces;
using Application.Services;
using AutoMapper;
using Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Tests.Handlers.Reservations
{
    public class ApproveReservationCommandHandlerTests
    {
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<ILoanRepository> _loanRepositoryMock;
        private readonly Mock<IReservationRepository> _reservationRepositoryMock;
        private readonly Mock<IBookRepository> _bookRepositoryMock;
        private readonly Mock<ILogger<ApproveReservationCommandHandler>> _loggerMock;
        private readonly Mock<IMailService> _mailServiceMock;
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<IJobRepository> _jobRepositoryMock;
        private readonly Mock<IJobService> _jobServiceMock;
        private readonly ApproveReservationCommandHandler _handler;

        public ApproveReservationCommandHandlerTests()
        {
            _mapperMock = new Mock<IMapper>();
            _loanRepositoryMock = new Mock<ILoanRepository>();
            _reservationRepositoryMock = new Mock<IReservationRepository>();
            _bookRepositoryMock = new Mock<IBookRepository>();
            _loggerMock = new Mock<ILogger<ApproveReservationCommandHandler>>();
            _mailServiceMock = new Mock<IMailService>();
            _userRepositoryMock = new Mock<IUserRepository>();
            _jobRepositoryMock = new Mock<IJobRepository>();
            _jobServiceMock = new Mock<IJobService>();
            _handler = new ApproveReservationCommandHandler(
                _mapperMock.Object,
                _loanRepositoryMock.Object,
                _reservationRepositoryMock.Object,
                _bookRepositoryMock.Object,
                _loggerMock.Object,
                _mailServiceMock.Object,
                _userRepositoryMock.Object,
                _jobRepositoryMock.Object,
                _jobServiceMock.Object);
        }

        [Fact]
        public async Task Handle_ValidRequest_ReturnsOkResult()
        {
            // Arrange
            var command = new ApproveReservationCommand
            {
                ReservationId = 1
            };

            var reservation = new Reservation
            {
                Id = 1,
                UserId = 1,
                BookId = 1,
                IsPendingApproval = true,
                IsApproved = false
            };

            var book = new Book
            {
                Id = 1,
                Title = "Book 1"
            };

            _reservationRepositoryMock.Setup(r => r.GetReservationByIdAsync(command.ReservationId))
                .ReturnsAsync(reservation);

            _bookRepositoryMock.Setup(b => b.GetBookByIdAsync(reservation.BookId))
                .ReturnsAsync(book);

            _loanRepositoryMock.Setup(l => l.IsPatronEligableForLoanAsync(reservation.UserId))
                .ReturnsAsync(true);

            _reservationRepositoryMock.Setup(r => r.UpdateReservationAsync(reservation))
                .ReturnsAsync(reservation);

            _bookRepositoryMock.Setup(b => b.ChangeBookAsAvailableAsync(book))
                .ReturnsAsync(book);

            _bookRepositoryMock.Setup(b => b.GetBookWithoutTrackingByIdAsync(1))
                .ReturnsAsync(book);


            _loanRepositoryMock.Setup(l => l.AddLoanAsync(It.IsAny<Loan>()))
                .ReturnsAsync(new Loan { Id = 1 });

            _userRepositoryMock.Setup(r => r.GetUserByIdAsync(1)).ReturnsAsync(new User
            {
                Name = "Rayyan",
                Email = "ray23fast@gmail.com"
            });

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }
    }
}
