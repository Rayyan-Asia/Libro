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
    public class EditReadingListCommandHandlerTests
    {
        private readonly Mock<IReadingListRepository> _readingListRepositoryMock;
        private readonly Mock<IBookRepository> _bookRepositoryMock;
        private readonly Mock<IReadingListBookRepository> _readingListBookRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<ILogger<EditReadingListCommandHandler>> _loggerMock;

        private readonly EditReadingListCommandHandler _handler;

        public EditReadingListCommandHandlerTests()
        {
            _readingListRepositoryMock = new Mock<IReadingListRepository>();
            _bookRepositoryMock = new Mock<IBookRepository>();
            _readingListBookRepositoryMock = new Mock<IReadingListBookRepository>();
            _mapperMock = new Mock<IMapper>();
            _loggerMock = new Mock<ILogger<EditReadingListCommandHandler>>();

            _handler = new EditReadingListCommandHandler(
                _readingListRepositoryMock.Object,
                _bookRepositoryMock.Object,
                _readingListBookRepositoryMock.Object,
                _mapperMock.Object,
                _loggerMock.Object);
        }

        [Fact]
        public async Task Handle_ValidRequest_ReturnsOkResult()
        {
            // Arrange
            var command = new EditReadingListCommand
            {
                Id = 1,
                UserId = 1,
                Name = "Updated Reading List",
                Description = "Updated description",
                Books = new List<IdDto>
                {
                    new IdDto { Id = 1 },
                    new IdDto { Id = 2 }
                }
            };

            var existingReadingList = new ReadingList
            {
                Id = 1,
                UserId = 1,
                Name = "Original Reading List",
                Description = "Original description",
                Books = new List<Book>
                {
                    new Book { Id = 1, Title = "Book 1" },
                    new Book { Id = 3, Title = "Book 3" }
                }
            };

            var updatedReadingList = new ReadingList
            {
                Id = 1,
                UserId = 1,
                Name = "Updated Reading List",
                Description = "Updated description",
                Books = new List<Book>
                {
                    new Book { Id = 1, Title = "Book 1" },
                    new Book { Id = 2, Title = "Book 2" }
                }
            };

            _readingListRepositoryMock.Setup(r => r.GetReadingListAsync(command.Id))
                .ReturnsAsync(existingReadingList);

            _readingListBookRepositoryMock.Setup(r => r.RemoveBooksFromReadingListAsync(command.Id))
                .Returns(Task.CompletedTask);

            _bookRepositoryMock.Setup(b => b.GetBookByIdAsync(1))
                .ReturnsAsync(new Book { Id = 1, Title = "Book 1" });

            _bookRepositoryMock.Setup(b => b.GetBookByIdAsync(2))
                .ReturnsAsync(new Book { Id = 2, Title = "Book 2" });

            _readingListRepositoryMock.Setup(r => r.UpdateReadingListAsync(It.IsAny<ReadingList>()))
                .ReturnsAsync(updatedReadingList);

            _mapperMock.Setup(m => m.Map<ReadingListDto>(updatedReadingList))
                .Returns(new ReadingListDto { Id = 1, Name = "Updated Reading List" });

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }
    }
}
