using System;
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
    public class AddReadingListCommandHandlerTests
    {
        private readonly Mock<IReadingListRepository> _readingListRepositoryMock;
        private readonly Mock<IBookRepository> _bookRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<ILogger<AddReadingListCommandHandler>> _loggerMock;
        private readonly AddReadingListCommandHandler _handler;

        public AddReadingListCommandHandlerTests()
        {
            _readingListRepositoryMock = new Mock<IReadingListRepository>();
            _bookRepositoryMock = new Mock<IBookRepository>();
            _mapperMock = new Mock<IMapper>();
            _loggerMock = new Mock<ILogger<AddReadingListCommandHandler>>();

            _handler = new AddReadingListCommandHandler(
                _readingListRepositoryMock.Object,
                _bookRepositoryMock.Object,
                _mapperMock.Object,
                _loggerMock.Object
            );
        }

        [Fact]
        public async Task Handle_WithValidRequest_ReturnsCreatedReadingList()
        {
            // Arrange
            var command = new AddReadingListCommand
            {
                Name = "Reading List Name",
                UserId = 1,
                Description = "Reading List Description",
                Books = new List<IdDto>
                {
                    new IdDto { Id = 1 },
                    new IdDto { Id = 2 },
                }
            };

            var readingList = new ReadingList
            {
                Id = 1,
                CreationDate = DateTime.Now,
                Name = command.Name,
                UserId = command.UserId,
                Description = command.Description,
                Books = new[]
                {
                    new Book { Id = 1, Title = "Book 1" },
                    new Book { Id = 2, Title = "Book 2" },
                }.ToList()
            };

            var readingListDto = new ReadingListDto
            {
                Id = readingList.Id,
                CreationDate = readingList.CreationDate,
                Name = readingList.Name,
                Description = readingList.Description,
                Books = new[]
                {
                    new BookDto { Id = 1, Title = "Book 1" },
                    new BookDto { Id = 2, Title = "Book 2" },
                }.ToList()
            };

            _bookRepositoryMock.Setup(r => r.GetBookByIdAsync(It.IsAny<int>()))
                .ReturnsAsync((int bookId) =>
                {
                    if (bookId == 1)
                        return new Book { Id = 1, Title = "Book 1" };
                    if (bookId == 2)
                        return new Book { Id = 2, Title = "Book 2" };
                    return null;
                });

            _readingListRepositoryMock.Setup(r => r.AddReadingListAsync(It.IsAny<ReadingList>()))
                .ReturnsAsync(readingList);

            _mapperMock.Setup(m => m.Map<ReadingListDto>(readingList))
                .Returns(readingListDto);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            var expectedResponse = new OkObjectResult(readingListDto);
            Assert.Equal(expectedResponse.StatusCode, (result as OkObjectResult)?.StatusCode);
            Assert.Equal(expectedResponse.Value, (result as OkObjectResult)?.Value);
        }

        [Fact]
        public async Task Handle_WithNonExistentBook_ReturnsNotFound()
        {
            // Arrange
            var command = new AddReadingListCommand
            {
                Name = "Reading List Name",
                UserId = 1,
                Description = "Reading List Description",
                Books = new List<IdDto> { new IdDto { Id = 1 } }
            };

            _bookRepositoryMock.Setup(r => r.GetBookByIdAsync(It.IsAny<int>()))
                .ReturnsAsync((Book)null);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            var expectedResponse = new NotFoundObjectResult("Book not found with ID " + command.Books.First().Id);
            Assert.Equal(expectedResponse.StatusCode, (result as NotFoundObjectResult)?.StatusCode);
            Assert.Equal(expectedResponse.Value, (result as NotFoundObjectResult)?.Value);
        }
    }
}
