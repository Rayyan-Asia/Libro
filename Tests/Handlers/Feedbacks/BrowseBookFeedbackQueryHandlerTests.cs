using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application;
using Application.DTOs;
using Application.Entities.Feedbacks.Handlers;
using Application.Entities.Feedbacks.Queries;
using Application.Interfaces;
using AutoMapper;
using Domain;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Tests.Handlers.Feedbacks
{
    public class BrowseBookFeedbackQueryHandlerTests
    {
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<IFeebackRepository> _feedbackRepositoryMock;
        private readonly Mock<IBookRepository> _bookRepositoryMock;
        private readonly Mock<ILogger<BrowseBookFeedbackQueryHandler>> _loggerMock;

        private readonly BrowseBookFeedbackQueryHandler _handler;

        public BrowseBookFeedbackQueryHandlerTests()
        {
            _mapperMock = new Mock<IMapper>();
            _feedbackRepositoryMock = new Mock<IFeebackRepository>();
            _bookRepositoryMock = new Mock<IBookRepository>();
            _loggerMock = new Mock<ILogger<BrowseBookFeedbackQueryHandler>>();

            _handler = new BrowseBookFeedbackQueryHandler(
                _mapperMock.Object,
                _feedbackRepositoryMock.Object,
                _bookRepositoryMock.Object,
                _loggerMock.Object
            );
        }

        [Fact]
        public async Task Handle_WithValidBookId_ReturnsFeedbacks()
        {
            // Arrange
            var query = new BrowseBookFeedbackQuery
            {
                BookId = 1,
                pageNumber = 1,
                pageSize = 10
            };

            var book = new Book { Id = 1, Title = "Sample Book" };
            var feedbacks = new List<Feedback> {
                new Feedback { Id = 1, Rating = Rating.Amazing, Review = "Great book!" },
                new Feedback { Id = 2, Rating = Rating.Amazing, Review = "Good book." }
            };
            _bookRepositoryMock.Setup(r => r.GetBookByIdAsync(query.BookId)).ReturnsAsync(book);

            var paginationMetadata = new PaginationMetadata
            {
                ItemCount = feedbacks.Count,
                PageSize = query.pageSize,
                PageCount = query.pageNumber,
            };
            _feedbackRepositoryMock.Setup(r => r.BrowseFeedbackByBookAsync(query.pageNumber, query.pageSize, query.BookId)).ReturnsAsync((paginationMetadata, feedbacks));
            _mapperMock.Setup(m => m.Map<List<FeedbackDto>>(feedbacks)).Returns(new List<FeedbackDto> {
                new FeedbackDto { Id = 1, Rating = Rating.Amazing, Review = "Great book!" },
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
        public async Task Handle_WithInvalidBookId_ReturnsNullFeedbacks()
        {
            // Arrange
            var query = new BrowseBookFeedbackQuery
            {
                BookId = 1,
                pageNumber = 1,
                pageSize = 10
            };

            _bookRepositoryMock.Setup(r => r.GetBookByIdAsync(query.BookId)).ReturnsAsync((Book)null);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.Null(result.Item1);
            Assert.Null(result.Item2);
        }
    }
}
