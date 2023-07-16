using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.DTOs;
using Application.Entities.Feedbacks.Handlers;
using Application.Entities.Feedbacks.Queries;
using Application.Interfaces;
using AutoMapper;
using Domain;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Application.Tests.Entities.Feedbacks.Handlers
{
    public class BrowseUserFeedbackQueryHandlerTests
    {
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<IFeebackRepository> _feedbackRepositoryMock;
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<ILogger<BrowseUserFeedbackQueryHandler>> _loggerMock;

        private readonly BrowseUserFeedbackQueryHandler _handler;

        public BrowseUserFeedbackQueryHandlerTests()
        {
            _mapperMock = new Mock<IMapper>();
            _feedbackRepositoryMock = new Mock<IFeebackRepository>();
            _userRepositoryMock = new Mock<IUserRepository>();
            _loggerMock = new Mock<ILogger<BrowseUserFeedbackQueryHandler>>();

            _handler = new BrowseUserFeedbackQueryHandler(
                _mapperMock.Object,
                _feedbackRepositoryMock.Object,
                _userRepositoryMock.Object,
                _loggerMock.Object
            );
        }

        [Fact]
        public async Task Handle_WithValidUserId_ReturnsFeedbacks()
        {
            // Arrange
            var query = new BrowseUserFeedbackQuery
            {
                UserId = 1,
                pageNumber = 1,
                pageSize = 10
            };

            var user = new User { Id = 1, Name = "John Doe" };
            var feedbacks = new List<Feedback> {
                new Feedback { Id = 1, Rating = Rating.Amazing, Review = "Great book!" } ,
                new Feedback { Id = 2, Rating = Rating.Amazing, Review = "Good book." }
            };

            var paginationMetadata = new PaginationMetadata
            {
                ItemCount = feedbacks.Count,
                PageSize = query.pageSize,
                PageCount = query.pageNumber
            };

            _userRepositoryMock.Setup(r => r.GetUserByIdAsync(query.UserId)).ReturnsAsync(user);
            _feedbackRepositoryMock.Setup(r => r.BrowseFeedbackByUserAsync(query.pageNumber, query.pageSize, query.UserId)).ReturnsAsync((paginationMetadata, feedbacks));
            _mapperMock.Setup(m => m.Map<List<FeedbackDto>>(feedbacks)).Returns(new List<FeedbackDto> {
                new FeedbackDto { Id = 1, Rating = Rating.Amazing, Review = "Great book!" } ,
                new FeedbackDto { Id = 2, Rating = Rating.Amazing, Review = "Good book." }
            });

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.Equal(paginationMetadata, result.Item1);
            Assert.Equal(feedbacks.Count, result.Item2.Count);
            Assert.Equal(feedbacks[0].Id, result.Item2[0].Id);
            Assert.Equal(feedbacks[0].Rating, result.Item2[0].Rating);
            Assert.Equal(feedbacks[0].Review, result.Item2[0].Review);
            Assert.Equal(feedbacks[1].Id, result.Item2[1].Id);
            Assert.Equal(feedbacks[1].Rating, result.Item2[1].Rating);
            Assert.Equal(feedbacks[1].Review, result.Item2[1].Review);
        }

        [Fact]
        public async Task Handle_WithInvalidUserId_ReturnsNullFeedbacks()
        {
            // Arrange
            var query = new BrowseUserFeedbackQuery
            {
                UserId = 1,
                pageNumber = 1,
                pageSize = 10
            };

            _userRepositoryMock.Setup(r => r.GetUserByIdAsync(query.UserId)).ReturnsAsync((User)null);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.Null(result.Item1);
            Assert.Null(result.Item2);
        }
    }
}
