using System;
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

namespace Tests.Handlers.Feedbacks
{
    public class AddFeedbackCommandHandlerTests
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<IBookRepository> _bookRepositoryMock;
        private readonly Mock<IFeebackRepository> _feedbackRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<ILogger<AddFeedbackCommandHandler>> _loggerMock;

        private readonly AddFeedbackCommandHandler _handler;

        public AddFeedbackCommandHandlerTests()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _bookRepositoryMock = new Mock<IBookRepository>();
            _feedbackRepositoryMock = new Mock<IFeebackRepository>();
            _mapperMock = new Mock<IMapper>();
            _loggerMock = new Mock<ILogger<AddFeedbackCommandHandler>>();

            _handler = new AddFeedbackCommandHandler(
                _userRepositoryMock.Object,
                _bookRepositoryMock.Object,
                _feedbackRepositoryMock.Object,
                _mapperMock.Object,
                _loggerMock.Object
            );
        }

        [Fact]
        public async Task Handle_WithValidUserIdAndBookId_CreatesFeedbackAndReturnsOkResult()
        {
            // Arrange
            var command = new AddFeedbackCommand
            {
                UserId = 1,
                BookId = 1,
                Rating = Rating.Amazing,
                Review = "Great book!"
            };

            var user = new User { Id = 1, Name = "John Doe" };
            var book = new Book { Id = 1, Title = "Sample Book" };
            var feedback = new Feedback { Id = 1, Rating = command.Rating, Review = command.Review, CreatedDate = DateTime.UtcNow 
            , Book = book};

            _userRepositoryMock.Setup(r => r.GetUserByIdAsync(command.UserId)).ReturnsAsync(user);
            _bookRepositoryMock.Setup(r => r.GetBookByIdAsync(command.BookId)).ReturnsAsync(book);
            _feedbackRepositoryMock.Setup(r => r.AddFeedbackAsync(It.IsAny<Feedback>())).ReturnsAsync(feedback);
            _mapperMock.Setup(m => m.Map<UserDto>(user)).Returns(new UserDto { Id = user.Id, Name = user.Name });

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var feedbackDto = Assert.IsType<FeedbackDto>(okResult.Value);

            Assert.Equal(feedback.Id, feedbackDto.Id);
            Assert.Equal(feedback.Rating, feedbackDto.Rating);
            Assert.Equal(feedback.Review, feedbackDto.Review);
            Assert.Equal(user.Id, feedbackDto.User.Id);
            Assert.Equal(user.Name, feedbackDto.User.Name);
            Assert.Equal(book.Id, feedbackDto.Book.Id);
            Assert.Equal(book.Title, feedbackDto.Book.Title);
        }

        [Fact]
        public async Task Handle_WithInvalidUserId_ReturnsNotFoundResult()
        {
            // Arrange
            var command = new AddFeedbackCommand { UserId = 1, BookId = 1, Rating = Rating.Amazing, Review = "Great book!" };

            _userRepositoryMock.Setup(r => r.GetUserByIdAsync(command.UserId)).ReturnsAsync((User)null);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public async Task Handle_WithInvalidBookId_ReturnsNotFoundResult()
        {
            // Arrange
            var command = new AddFeedbackCommand { UserId = 1, BookId = 1, Rating = Rating.Amazing, Review = "Great book!" };

            var user = new User { Id = 1, Name = "John Doe" };

            _userRepositoryMock.Setup(r => r.GetUserByIdAsync(command.UserId)).ReturnsAsync(user);
            _bookRepositoryMock.Setup(r => r.GetBookByIdAsync(command.BookId)).ReturnsAsync((Book)null);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }
    }
}
