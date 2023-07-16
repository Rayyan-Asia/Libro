using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Entities.ReadingLists.Commands;
using Application.Entities.ReadingLists.Handlers;
using Application.Interfaces;
using Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Tests.Handlers.ReadingLists
{
    public class RemoveReadingListCommandHandlerTests
    {
        private readonly Mock<IReadingListRepository> _readingListRepositoryMock;
        private readonly Mock<ILogger<RemoveReadingListCommandHandler>> _loggerMock;

        private readonly RemoveReadingListCommandHandler _handler;

        public RemoveReadingListCommandHandlerTests()
        {
            _readingListRepositoryMock = new Mock<IReadingListRepository>();
            _loggerMock = new Mock<ILogger<RemoveReadingListCommandHandler>>();

            _handler = new RemoveReadingListCommandHandler(
                _readingListRepositoryMock.Object,
                _loggerMock.Object);
        }

        [Fact]
        public async Task Handle_ValidRequest_ReturnsNoContentResult()
        {
            // Arrange
            var command = new RemoveReadingListCommand
            {
                ReadingListId = 1,
                UserId = 1
            };

            var existingReadingList = new ReadingList
            {
                Id = 1,
                UserId = 1,
                Name = "Reading List",
                Description = "Description"
            };

            _readingListRepositoryMock.Setup(r => r.GetReadingListAsync(command.ReadingListId))
                .ReturnsAsync(existingReadingList);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task Handle_UnknownReadingList_ReturnsNotFoundResult()
        {
            // Arrange
            var command = new RemoveReadingListCommand
            {
                ReadingListId = 1,
                UserId = 1
            };

            _readingListRepositoryMock.Setup(r => r.GetReadingListAsync(command.ReadingListId))
                .ReturnsAsync((ReadingList)null);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public async Task Handle_UnauthorizedUser_ReturnsForbidResult()
        {
            // Arrange
            var command = new RemoveReadingListCommand
            {
                ReadingListId = 1,
                UserId = 1
            };

            var existingReadingList = new ReadingList
            {
                Id = 1,
                UserId = 2,
                Name = "Reading  List",
                Description = "Description"
            };

            _readingListRepositoryMock.Setup(r => r.GetReadingListAsync(command.ReadingListId))
                .ReturnsAsync(existingReadingList);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.IsType<ForbidResult>(result);
        }
    }
}
