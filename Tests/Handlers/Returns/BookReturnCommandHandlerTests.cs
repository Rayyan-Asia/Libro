using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Entities.Returns.Commands;
using Application.Entities.Returns.Handlers;
using Application.Entities.Returns.Queries;
using Application.Interfaces;
using AutoMapper;
using Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Tests.Handlers.Returns

{
    public class BookReturnCommandHandlerTests
    {
        private readonly Mock<IBookReturnRepository> _bookReturnRepositoryMock;
        private readonly Mock<ILoanRepository> _loanRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<ILogger<BookReturnCommandHandler>> _loggerMock;

        private readonly BookReturnCommandHandler _handler;

        public BookReturnCommandHandlerTests()
        {
            _bookReturnRepositoryMock = new Mock<IBookReturnRepository>();
            _loanRepositoryMock = new Mock<ILoanRepository>();
            _mapperMock = new Mock<IMapper>();
            _loggerMock = new Mock<ILogger<BookReturnCommandHandler>>();

            _handler = new BookReturnCommandHandler(
                _bookReturnRepositoryMock.Object,
                _loanRepositoryMock.Object,
                _mapperMock.Object,
                _loggerMock.Object);
        }

        [Fact]
        public async Task Handle_ValidRequest_ReturnsOkObjectResult()
        {
            // Arrange
            var command = new BookReturnCommand
            {
                LoanId = 1,
                UserId = 1
            };

            var existingLoan = new Loan
            {
                Id = 1,
                UserId = 1
            };

            _loanRepositoryMock.Setup(r => r.GetLoanByIdAsync(command.LoanId))
                .ReturnsAsync(existingLoan);

            _bookReturnRepositoryMock.Setup(r => r.GetReturnByLoanIdAsync(existingLoan.Id))
                .ReturnsAsync((BookReturn)null);

            var expectedBookReturn = new BookReturn
            {
                LoanId = existingLoan.Id,
                IsApproved = false,
                ReturnDate = It.IsAny<DateTime>()
            };

            _bookReturnRepositoryMock.Setup(r => r.AddReturnAsync(It.IsAny<BookReturn>()))
                .ReturnsAsync(expectedBookReturn);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task Handle_UnknownLoan_ReturnsNotFoundResult()
        {
            // Arrange
            var command = new BookReturnCommand
            {
                LoanId = 1,
                UserId = 1
            };

            _loanRepositoryMock.Setup(r => r.GetLoanByIdAsync(command.LoanId))
                .ReturnsAsync((Loan)null);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public async Task Handle_UserNotOwnerOfLoan_ReturnsBadRequestResult()
        {
            // Arrange
            var command = new BookReturnCommand
            {
                LoanId = 1,
                UserId = 1
            };

            var existingLoan = new Loan
            {
                Id = 1,
                UserId = 2
            };

            _loanRepositoryMock.Setup(r => r.GetLoanByIdAsync(command.LoanId))
                .ReturnsAsync(existingLoan);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task Handle_ReturnAlreadyExistsForLoan_ReturnsBadRequestResult()
        {
            // Arrange
            var command = new BookReturnCommand
            {
                LoanId = 1,
                UserId = 1
            };

            var existingLoan = new Loan
            {
                Id = 1,
                UserId = 1
            };

            _loanRepositoryMock.Setup(r => r.GetLoanByIdAsync(command.LoanId))
                .ReturnsAsync(existingLoan);

            _bookReturnRepositoryMock.Setup(r => r.GetReturnByLoanIdAsync(existingLoan.Id))
                .ReturnsAsync(new BookReturn());

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }
    }
}
