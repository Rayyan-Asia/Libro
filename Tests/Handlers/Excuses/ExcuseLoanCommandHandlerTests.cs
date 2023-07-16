using System.Threading;
using System.Threading.Tasks;
using Application.Entities.Excuses.Commands;
using Application.Entities.Excuses.Handlers;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Tests.Handlers.Excuses
{
    public class ExcuseLoanCommandHandlerTests
    {
        private readonly Mock<ILoanRepository> _loanRepositoryMock;
        private readonly Mock<ILogger<ExcuseLoanCommandHandler>> _loggerMock;

        private readonly ExcuseLoanCommandHandler _handler;

        public ExcuseLoanCommandHandlerTests()
        {
            _loanRepositoryMock = new Mock<ILoanRepository>();
            _loggerMock = new Mock<ILogger<ExcuseLoanCommandHandler>>();

            _handler = new ExcuseLoanCommandHandler(
                _loanRepositoryMock.Object,
                _loggerMock.Object
            );
        }

        [Fact]
        public async Task Handle_WithValidLoanId_ExcusesLoanAndReturnsNoContentResult()
        {
            // Arrange
            var command = new ExcuseLoanCommand { LoanId = 1 };

            _loanRepositoryMock.Setup(r => r.ExcuseLoanAsync(command.LoanId)).ReturnsAsync(true);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task Handle_WithNonExistingLoan_ReturnsNotFoundResult()
        {
            // Arrange
            var command = new ExcuseLoanCommand { LoanId = 1 };

            _loanRepositoryMock.Setup(r => r.ExcuseLoanAsync(command.LoanId)).ReturnsAsync(false);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }
    }
}
