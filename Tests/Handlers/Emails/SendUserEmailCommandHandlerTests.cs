using System.Threading;
using System.Threading.Tasks;
using Application.Entities.Emails.Commands;
using Application.Entities.Emails.Handlers;
using Application.Interfaces;
using Application.Services;
using Domain;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Tests.Handlers.Emails
{
    public class SendUserEmailCommandHandlerTests
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<IMailService> _mailServiceMock;

        private readonly SendUserEmailCommandHandler _handler;

        public SendUserEmailCommandHandlerTests()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _mailServiceMock = new Mock<IMailService>();

            _handler = new SendUserEmailCommandHandler(
                _userRepositoryMock.Object,
                _mailServiceMock.Object
            );
        }

        [Fact]
        public async Task Handle_WithValidUserId_SendsEmailAndReturnsNoContentResult()
        {
            // Arrange
            var command = new SendUserEmailCommand { UserId = 1, Subject = "Test Subject", Body = "Test Body" };
            var user = new User { Id = command.UserId, Name = "John Doe", Email = "john.doe@example.com" };

            _userRepositoryMock.Setup(r => r.GetUserByIdAsync(command.UserId)).ReturnsAsync(user);
            _mailServiceMock.Setup(m => m.SendEmailAsync(
                user.Email,
                user.Name,
                command.Subject,
                command.Body
            )).Returns(Task.CompletedTask);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task Handle_WithNonExistingUser_ReturnsNotFoundResult()
        {
            // Arrange
            var command = new SendUserEmailCommand { UserId = 1 };

            _userRepositoryMock.Setup(r => r.GetUserByIdAsync(command.UserId)).ReturnsAsync(null as User);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }
    }
}
