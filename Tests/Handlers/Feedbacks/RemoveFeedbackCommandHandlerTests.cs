using System.Threading;
using System.Threading.Tasks;
using Application.DTOs;
using Application.Entities.Feedbacks.Commands;
using Application.Entities.Feedbacks.Handlers;
using Application.Interfaces;
using Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Tests.Handlers.Feedbacks
{
    public class RemoveFeedbackCommandHandlerTests
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<IFeebackRepository> _feedbackRepositoryMock;
        private readonly Mock<ILogger<RemoveFeedbackCommandHandler>> _loggerMock;

        private readonly RemoveFeedbackCommandHandler _handler;

        public RemoveFeedbackCommandHandlerTests()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _feedbackRepositoryMock = new Mock<IFeebackRepository>();
            _loggerMock = new Mock<ILogger<RemoveFeedbackCommandHandler>>();

            _handler = new RemoveFeedbackCommandHandler(
                _userRepositoryMock.Object,
                _feedbackRepositoryMock.Object,
                _loggerMock.Object
            );
        }

        [Fact]
        public async Task Handle_WithValidRequest_ReturnsNoContentResult()
        {
            // Arrange
            var command = new RemoveFeedbackCommand
            {
                UserId = 1,
                FeedbackId = 1
            };

            var user = new User { Id = 1 };
            var feedback = new Feedback { Id = 1, UserId = 1 };

            _userRepositoryMock.Setup(r => r.GetUserByIdAsync(command.UserId)).ReturnsAsync(user);
            _feedbackRepositoryMock.Setup(r => r.GetFeedbackByIdAsync(command.FeedbackId)).ReturnsAsync(feedback);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task Handle_WithNonExistingUser_ReturnsNotFoundResult()
        {
            // Arrange
            var command = new RemoveFeedbackCommand
            {
                UserId = 1,
                FeedbackId = 1
            };

            _userRepositoryMock.Setup(r => r.GetUserByIdAsync(command.UserId)).ReturnsAsync((User)null);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
            var notFoundResult = (NotFoundObjectResult)result;
            Assert.Equal($"User NOT FOUND with ID {command.UserId}", notFoundResult.Value);
        }

        [Fact]
        public async Task Handle_WithNonExistingFeedback_ReturnsNotFoundResult()
        {
            // Arrange
            var command = new RemoveFeedbackCommand
            {
                UserId = 1,
                FeedbackId = 1
            };

            var user = new User { Id = 1 };

            _userRepositoryMock.Setup(r => r.GetUserByIdAsync(command.UserId)).ReturnsAsync(user);
            _feedbackRepositoryMock.Setup(r => r.GetFeedbackByIdAsync(command.FeedbackId)).ReturnsAsync((Feedback)null);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
            var notFoundResult = (NotFoundObjectResult)result;
            Assert.Equal($"Feedback NOT FOUND with ID {command.FeedbackId}", notFoundResult.Value);
        }

        [Fact]
        public async Task Handle_WithNonMatchingUser_ReturnsForbidResult()
        {
            // Arrange
            var command = new RemoveFeedbackCommand
            {
                UserId = 1,
                FeedbackId = 1
            };

            var user = new User { Id = 1 };
            var feedback = new Feedback { Id = 1, UserId = 2 };

            _userRepositoryMock.Setup(r => r.GetUserByIdAsync(command.UserId)).ReturnsAsync(user);
            _feedbackRepositoryMock.Setup(r => r.GetFeedbackByIdAsync(command.FeedbackId)).ReturnsAsync(feedback);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.IsType<ForbidResult>(result);
        }
    }
}
