using System.Threading.Tasks;
using Application.Entities.Emails.Commands;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Presentation.Controllers;
using Presentation.Validators.Emails;
using Xunit;

namespace Tests.Controllers
{
    public class EmailControllerTests
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly SendUserEmailCommandValidator _validator;
        private readonly EmailController _controller;

        public EmailControllerTests()
        {
            _mediatorMock = new Mock<IMediator>();
            _validator = new SendUserEmailCommandValidator();

            _controller = new EmailController(
                _mediatorMock.Object,
                _validator
            );
        }

        [Fact]
        public async Task Add_ValidCommand_ReturnsOkResult()
        {
            // Arrange
            var sendUserEmailCommand = new SendUserEmailCommand() {
            UserId = 1,
            Subject = "Loan",
            Body = "Body"
            };
            _mediatorMock.Setup(m => m.Send(sendUserEmailCommand, default))
                .ReturnsAsync(new OkResult());

            // Act
            var result = await _controller.add(sendUserEmailCommand) as IActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async Task Add_InvalidCommand_ReturnsBadRequest()
        {
            // Arrange
            var sendUserEmailCommand = new SendUserEmailCommand();

            // Act
            var result = await _controller.add(sendUserEmailCommand) as IActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.IsType<BadRequestObjectResult>(result);
        }
    }
}
