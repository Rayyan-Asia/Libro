using System.Threading.Tasks;
using Application.Entities.Authors.Commands;
using Application.Entities.Excuses.Commands;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Presentation.Controllers;
using Presentation.Validators.Excuses;
using Xunit;

namespace Tests.Controllers
{
    public class ExcusesControllerTests
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly ExcuseLoanCommandValidator _excuseLoanValidator;
        private readonly ExcuseUserCommandValidator _excuseUserValidator;
        private readonly ExcusesController _controller;

        public ExcusesControllerTests()
        {
            _mediatorMock = new Mock<IMediator>();
            _excuseLoanValidator = new ExcuseLoanCommandValidator();
            _excuseUserValidator = new ExcuseUserCommandValidator();

            _controller = new ExcusesController(
                _mediatorMock.Object,
                _excuseLoanValidator,
                _excuseUserValidator
            );
        }

        [Fact]
        public async Task ExcuseLoan_ValidLoanId_ReturnsOkResult()
        {
            // Arrange
            var loanId = 123;
            _mediatorMock.Setup(m => m.Send(It.IsAny<ExcuseLoanCommand>(), default))
                .ReturnsAsync(new OkResult());

            // Act
            var result = await _controller.ExcuseLoan(loanId) as IActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async Task ExcuseLoan_InvalidLoanId_ReturnsBadRequest()
        {
            // Arrange
            var loanId = 0;

            // Act
            var result = await _controller.ExcuseLoan(loanId) as IActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public async Task ExcuseUser_ValidUserId_ReturnsOkResult()
        {
            // Arrange
            var userId = 123;
            _mediatorMock.Setup(m => m.Send(It.IsAny<ExcuseUserCommand>(), default))
                .ReturnsAsync(new OkResult());

            // Act
            var result = await _controller.ExcuseUser(userId) as IActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async Task ExcuseUser_InvalidUserId_ReturnsBadRequest()
        {
            // Arrange
            var userId = 0;

            // Act
            var result = await _controller.ExcuseUser(userId) as IActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.IsType<BadRequestResult>(result);
        }
    }
}
