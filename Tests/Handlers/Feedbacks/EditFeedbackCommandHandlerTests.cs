using System.Threading;
using System.Threading.Tasks;
using Application.DTOs;
using Application.Entities.Feedbacks.Commands;
using Application.Entities.Feedbacks.Handlers;
using Application.Interfaces;
using AutoMapper;
using Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Application.Tests.Entities.Feedbacks.Handlers
{
    public class EditFeedbackCommandHandlerTests
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<IBookRepository> _bookRepositoryMock;
        private readonly Mock<IFeebackRepository> _feedbackRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<ILogger<EditFeedbackCommandHandler>> _loggerMock;

        private readonly EditFeedbackCommandHandler _handler;

        public EditFeedbackCommandHandlerTests()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _bookRepositoryMock = new Mock<IBookRepository>();
            _feedbackRepositoryMock = new Mock<IFeebackRepository>();
            _mapperMock = new Mock<IMapper>();
            _loggerMock = new Mock<ILogger<EditFeedbackCommandHandler>>();

            _handler = new EditFeedbackCommandHandler(
                _userRepositoryMock.Object,
                _bookRepositoryMock.Object,
                _feedbackRepositoryMock.Object,
                _mapperMock.Object,
                _loggerMock.Object
            );
        }

        [Fact]
        public async Task Handle_WithValidRequest_ReturnsOkResultWithUpdatedFeedback()
        {
            // Arrange
            var command = new EditFeedbackCommand
            {
                Id = 1,
                UserId = 1,
                BookId = 1,
                Rating = Rating.Amazing,
                CreatedDate = new DateTime(2022, 1, 1),
                Review = "Updated review"
            };

            var originalFeedback = new Feedback
            {
                Id = 1,
                UserId = 1,
                BookId = 1,
                Rating = Rating.Amazing,
                CreatedDate = new DateTime(2021, 1, 1),
                Review = "Original review"
            };

            var updatedFeedback = new Feedback
            {
                Id = 1,
                UserId = 1,
                BookId = 1,
                Rating = Rating.Amazing,
                CreatedDate = new DateTime(2022, 1, 1),
                Review = "Updated review"
            };

            _userRepositoryMock.Setup(r => r.GetUserByIdAsync(command.UserId)).ReturnsAsync(new User { Id = 1 });
            _bookRepositoryMock.Setup(r => r.GetBookByIdAsync(command.BookId)).ReturnsAsync(new Book { Id = 1 });
            _feedbackRepositoryMock.Setup(r => r.GetFeedbackByIdAsync(command.Id)).ReturnsAsync(originalFeedback);
            _feedbackRepositoryMock.Setup(r => r.UpdateFeedbackAsync(originalFeedback)).ReturnsAsync(updatedFeedback);
            _mapperMock.Setup(m => m.Map<FeedbackDto>(updatedFeedback)).Returns(new FeedbackDto { Id = updatedFeedback.Id,
                Rating = updatedFeedback.Rating, CreatedDate = updatedFeedback.CreatedDate, Review = updatedFeedback.Review});

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.IsType<OkObjectResult>(result);
            var okResult = (OkObjectResult)result;
            Assert.IsType<FeedbackDto>(okResult.Value);
            var feedbackDto = (FeedbackDto)okResult.Value;
            Assert.Equal(updatedFeedback.Id, feedbackDto.Id);
            Assert.Equal(updatedFeedback.Rating, feedbackDto.Rating);
            Assert.Equal(updatedFeedback.CreatedDate, feedbackDto.CreatedDate);
            Assert.Equal(updatedFeedback.Review, feedbackDto.Review);
        }

        [Fact]
        public async Task Handle_WithNonExistingFeedback_ReturnsNotFoundResult()
        {
            // Arrange
            var command = new EditFeedbackCommand
            {
                Id = 1,
                UserId = 1,
                BookId = 1,
                Rating = Rating.Amazing,
                CreatedDate = new DateTime(2022, 1, 1),
                Review = "Updated review"
            };

            _feedbackRepositoryMock.Setup(r => r.GetFeedbackByIdAsync(command.Id)).ReturnsAsync((Feedback)null);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
            var notFoundResult = (NotFoundObjectResult)result;
            Assert.Equal($"Feedback NOT FOUND with ID {command.Id}", notFoundResult.Value);
        }

        [Fact]
        public async Task Handle_WithNonExistingUser_ReturnsNotFoundResult()
        {
            // Arrange
            var command = new EditFeedbackCommand
            {
                Id = 1,
                UserId = 1,
                BookId = 1,
                Rating = Rating.Amazing,
                CreatedDate = new DateTime(2022, 1, 1),
                Review = "Updated review"
            };

            _feedbackRepositoryMock.Setup(r => r.GetFeedbackByIdAsync(command.Id)).ReturnsAsync(new Feedback { Id = 1 });
            _userRepositoryMock.Setup(r => r.GetUserByIdAsync(command.UserId)).ReturnsAsync((User)null);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
            var notFoundResult = (NotFoundObjectResult)result;
            Assert.Equal($"User NOT FOUND with ID {command.UserId}", notFoundResult.Value);
        }

        [Fact]
        public async Task Handle_WithNonExistingBook_ReturnsNotFoundResult()
        {
            // Arrange
            var command = new EditFeedbackCommand
            {
                Id = 1,
                UserId = 1,
                BookId = 1,
                Rating = Rating.Amazing,
                CreatedDate = new DateTime(2022, 1, 1),
                Review = "Updated review"
            };

            _feedbackRepositoryMock.Setup(r => r.GetFeedbackByIdAsync(command.Id)).ReturnsAsync(new Feedback { Id = 1 });
            _userRepositoryMock.Setup(r => r.GetUserByIdAsync(command.UserId)).ReturnsAsync(new User { Id = 1 });
            _bookRepositoryMock.Setup(r => r.GetBookByIdAsync(command.BookId)).ReturnsAsync((Book)null);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
            var notFoundResult = (NotFoundObjectResult)result;
            Assert.Equal($"Book NOT FOUND with ID {command.BookId}", notFoundResult.Value);
        }
    }
}
