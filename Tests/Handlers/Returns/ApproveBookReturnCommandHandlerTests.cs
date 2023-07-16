using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Entities.Returns.Commands;
using Application.Entities.Returns.Handlers;
using Application.Interfaces;
using Application.Services;
using AutoMapper;
using Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Tests.Handlers.Returns
{
    public class ApproveBookReturnCommandHandlerTests
    {
        private readonly Mock<IBookReturnRepository> _bookReturnRepositoryMock;
        private readonly Mock<ILoanRepository> _loanRepositoryMock;
        private readonly Mock<IBookRepository> _bookRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<ILogger<ApproveBookReturnCommandHandler>> _loggerMock;
        private readonly Mock<IMailService> _mailServiceMock;
        private readonly Mock<IJobService> _jobServiceMock;

        private readonly ApproveBookReturnCommandHandler _handler;

        public ApproveBookReturnCommandHandlerTests()
        {
            _bookReturnRepositoryMock = new Mock<IBookReturnRepository>();
            _loanRepositoryMock = new Mock<ILoanRepository>();
            _bookRepositoryMock = new Mock<IBookRepository>();
            _mapperMock = new Mock<IMapper>();
            _userRepositoryMock = new Mock<IUserRepository>();
            _loggerMock = new Mock<ILogger<ApproveBookReturnCommandHandler>>();
            _mailServiceMock = new Mock<IMailService>();
            _jobServiceMock = new Mock<IJobService>();

            _handler = new ApproveBookReturnCommandHandler(
                _bookReturnRepositoryMock.Object,
                _loanRepositoryMock.Object,
                _bookRepositoryMock.Object,
                _mapperMock.Object,
                _loggerMock.Object,
                _userRepositoryMock.Object,
                _mailServiceMock.Object,
                _jobServiceMock.Object);
        }

        [Fact]
        public async Task Handle_ValidRequest_ReturnsOkObjectResult()
        {
            // Arrange
            var command = new ApproveBookReturnCommand
            {
                BookReturnId = 1
            };

            

            var existingLoan = new Loan
            {
                Id = 1,
                BookId = 1
            };


            var existingBookReturn = new BookReturn
            {
                Id = 1,
                LoanId = 1,
                IsApproved = false,
                Loan = existingLoan
            };

            var existingBook = new Book
            {
                Id = 1,
                Title = "Book Title"
            };

            _bookReturnRepositoryMock.Setup(r => r.GetReturnByIdAsync(command.BookReturnId))
                .ReturnsAsync(existingBookReturn);

            _loanRepositoryMock.Setup(r => r.GetLoanByIdAsync(existingBookReturn.LoanId))
                .ReturnsAsync(existingLoan);

            _bookRepositoryMock.Setup(r => r.GetBookByIdAsync(existingLoan.BookId))
                .ReturnsAsync(existingBook);

            _bookRepositoryMock.Setup(r => r.ChangeBookAsAvailableAsync(existingBook))
                .ReturnsAsync(existingBook);

            _bookReturnRepositoryMock.Setup(r => r.SetBookReturnApprovedAsync(existingBookReturn))
                .ReturnsAsync(existingBookReturn);
            _userRepositoryMock.Setup(r => r.GetUserByIdAsync(existingBookReturn.Loan.UserId)).ReturnsAsync(new User {
                Name = "Rayyan",
                Email = "ray23fast@gmail.com"
            });

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task Handle_UnknownBookReturn_ReturnsNotFoundResult()
        {
            // Arrange
            var command = new ApproveBookReturnCommand
            {
                BookReturnId = 1
            };

            _bookReturnRepositoryMock.Setup(r => r.GetReturnByIdAsync(command.BookReturnId))
                .ReturnsAsync((BookReturn)null);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public async Task Handle_AlreadyApprovedBookReturn_ReturnsBadRequestResult()
        {
            // Arrange
            var command = new ApproveBookReturnCommand
            {
                BookReturnId = 1
            };

            var existingBookReturn = new BookReturn
            {
                Id = 1,
                LoanId = 1,
                IsApproved = true
            };

            _bookReturnRepositoryMock.Setup(r => r.GetReturnByIdAsync(command.BookReturnId))
                .ReturnsAsync(existingBookReturn);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task Handle_UnknownLoan_ReturnsNotFoundResult()
        {
            // Arrange
            var command = new ApproveBookReturnCommand
            {
                BookReturnId = 1
            };

            var existingBookReturn = new BookReturn
            {
                Id = 1,
                LoanId = 1,
                IsApproved = false
            };

            _bookReturnRepositoryMock.Setup(r => r.GetReturnByIdAsync(command.BookReturnId))
                .ReturnsAsync(existingBookReturn);

            _loanRepositoryMock.Setup(r => r.GetLoanByIdAsync(existingBookReturn.LoanId))
                .ReturnsAsync((Loan)null);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public async Task Handle_UnknownBook_ReturnsNotFoundResult()
        {
            // Arrange
            var command = new ApproveBookReturnCommand
            {
                BookReturnId = 1
            };

            var existingBookReturn = new BookReturn
            {
                Id =  1,
                LoanId = 1,
                IsApproved = false
            };

            var existingLoan = new Loan
            {
                Id = 1,
                BookId = 1
            };

            _bookReturnRepositoryMock.Setup(r => r.GetReturnByIdAsync(command.BookReturnId))
                .ReturnsAsync(existingBookReturn);

            _loanRepositoryMock.Setup(r => r.GetLoanByIdAsync(existingBookReturn.LoanId))
                .ReturnsAsync(existingLoan);

            _bookRepositoryMock.Setup(r => r.GetBookByIdAsync(existingLoan.BookId))
                .ReturnsAsync((Book)null);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }
    }
}
