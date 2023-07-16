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
    public class ExcuseUserCommandHandlerTests
    {
        private readonly Mock<ILoanRepository> _loanRepositoryMock;
        private readonly Mock<ILogger<ExcuseUserCommandHandler>> _loggerMock;

        private readonly ExcuseUserCommandHandler _handler;

        public ExcuseUserCommandHandlerTests()
        {
            _loanRepositoryMock = new Mock<ILoanRepository>();
            _loggerMock = new Mock<ILogger<ExcuseUserCommandHandler>>();

            _handler = new ExcuseUserCommandHandler(
                _loanRepositoryMock.Object,
                _loggerMock.Object
            );
        }

        [Fact]
        public async Task Handle_WithValidUserId_ExcusesUserLoansAndReturnsNoContentResult()
        {
            // Arrange
            var command = new ExcuseUserCommand { UserId = 1 };

            _loanRepositoryMock.Setup(r => r.ExcuseLoansFromUserAsync(command.UserId)).ReturnsAsync(true);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task Handle_WithNonExistingUserLoans_ReturnsBadRequestResult()
        {
            // Arrange
            var command = new ExcuseUserCommand { UserId = 1 };

            _loanRepositoryMock.Setup(r => r.ExcuseLoansFromUserAsync(command.UserId)).ReturnsAsync(false);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }
    }
}
