using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.DTOs;
using Application.Entities.ReadingLists.Commands;
using Application.Entities.ReadingLists.Handlers;
using Application.Interfaces;
using AutoMapper;
using Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Tests.Handlers.ReadingLists
{
    public class RemoveBookFromReadingListCommandHandlerTests
    {
        private readonly Mock<IBookRepository> _bookRepositoryMock;
        private readonly Mock<IReadingListRepository> _readingListRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<ILogger<RemoveBookFromReadingListCommandHandler>> _loggerMock;

        private readonly RemoveBookFromReadingListCommandHandler _handler;

        public RemoveBookFromReadingListCommandHandlerTests()
        {
            _bookRepositoryMock = new Mock<IBookRepository>();
            _readingListRepositoryMock = new Mock<IReadingListRepository>();
            _mapperMock = new Mock<IMapper>();
            _loggerMock = new Mock<ILogger<RemoveBookFromReadingListCommandHandler>>();

            _handler = new RemoveBookFromReadingListCommandHandler(
                _bookRepositoryMock.Object,
                _readingListRepositoryMock.Object,
                _mapperMock.Object,
                _loggerMock.Object);
        }

        [Fact]
        public async Task Handle_ValidRequest_ReturnsOkResult()
        {
            // Arrange
            var command = new RemoveBookFromReadingListCommand
            {
                ReadingListId = 1,
                UserId = 1,
                BookId = 1
            };

            var existingReadingList = new ReadingList
            {
                Id = 1,
                UserId = 1,
                Name = "Reading List",
                Description = "Description",
                Books = new List<Book>
                {
                    new Book { Id = 1, Title = "Book 1" },
                    new Book { Id = 2, Title = "Book 2" }
                }
            };

            var updatedReadingList = new ReadingList
            {
                Id = 1,
                UserId = 1,
                Name = "Reading List",
                Description = "Description",
                Books = new List<Book>
                {
                    new Book { Id = 1, Title = "Book 2" }
                }
            };

            _readingListRepositoryMock.Setup(r => r.GetReadingListIncludingCollectionsAsync(command.ReadingListId))
                .ReturnsAsync(existingReadingList);

            _bookRepositoryMock.Setup(b => b.GetBookByIdAsync(command.BookId))
                .ReturnsAsync(new Book { Id = 1, Title = "Book 1" });

            _readingListRepositoryMock.Setup(r => r.UpdateReadingListAsync(It.IsAny<ReadingList>()))
                .ReturnsAsync(updatedReadingList);

            _mapperMock.Setup(m => m.Map<ReadingListDto>(updatedReadingList))
                .Returns(new ReadingListDto { Id = 1, Name = "Reading List" });

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }
    }
}
