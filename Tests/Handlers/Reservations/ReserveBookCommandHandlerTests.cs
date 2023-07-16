using System;
using System.Threading;
using System.Threading.Tasks;
using Application.DTOs;
using Application.Entities.Reservations.Commnads;
using Application.Entities.Reservations.Handlers;
using Application.Interfaces;
using AutoMapper;
using Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Swashbuckle.AspNetCore.SwaggerGen;
using Xunit;

namespace Tests.Handlers.Reservations

{
    public class ReserveBookCommandHandlerTests
    {
        private readonly Mock<IReservationRepository> _reservationRepositoryMock;
        private readonly Mock<IBookRepository> _bookRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<ILogger<ReserveBookCommandHandler>> _loggerMock;
        private readonly ReserveBookCommandHandler _handler;

        public ReserveBookCommandHandlerTests()
        {
            _reservationRepositoryMock = new Mock<IReservationRepository>();
            _bookRepositoryMock = new Mock<IBookRepository>();
            _mapperMock = new Mock<IMapper>();
            _loggerMock = new Mock<ILogger<ReserveBookCommandHandler>>();
            _handler = new ReserveBookCommandHandler(
                _reservationRepositoryMock.Object,
                _bookRepositoryMock.Object,
                _mapperMock.Object,
                _loggerMock.Object);
        }

        [Fact]
        public async Task Handle_ValidRequest_ReturnsOkResult()
        {
            // Arrange
            var command = new ReserveBookCommand
            {
                UserId = 1,
                BookId = 1
            };

            var book = new Book
            {
                Id = 1,
                Title = "Book 1"

            };

            _reservationRepositoryMock.Setup(r => r.GetReservationByUserIdAndBookIdAsync(command.UserId, command.BookId))
                .ReturnsAsync(null as Reservation);

            _bookRepositoryMock.Setup(b => b.GetBookByIdAsync(command.BookId))
                .ReturnsAsync(book);

            _bookRepositoryMock.Setup(b => b.ReserveBookAsync(book))
                .ReturnsAsync(book);
            _reservationRepositoryMock.Setup(r=>r.IsPatronEligableForReservationAsync(command.UserId)).ReturnsAsync(true);
            _reservationRepositoryMock.Setup(r => r.AddReservationAsync(It.IsAny<Reservation>()))
                .ReturnsAsync(new Reservation { Id = 1 });

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }
    }
}