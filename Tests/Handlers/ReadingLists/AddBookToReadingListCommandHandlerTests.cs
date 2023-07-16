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
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Tests.Handlers.ReadingLists
{
    public class AddBookToReadingListCommandHandlerTests
    {
        private readonly Mock<IBookRepository> _bookRepositoryMock;
        private readonly Mock<IReadingListRepository> _readingListRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<ILogger<AddBookToReadingListCommandHandler>> _loggerMock;
        private readonly AddBookToReadingListCommandHandler _handler;

        public AddBookToReadingListCommandHandlerTests()
        {
            _bookRepositoryMock = new Mock<IBookRepository>();
            _readingListRepositoryMock = new Mock<IReadingListRepository>();
            _mapperMock = new Mock<IMapper>();
            _loggerMock = new Mock<ILogger<AddBookToReadingListCommandHandler>>();

            _handler = new AddBookToReadingListCommandHandler(
                _bookRepositoryMock.Object,
                _readingListRepositoryMock.Object,
                _mapperMock.Object,
                _loggerMock.Object
            );
        }

        [Fact]
        public async Task Handle_WithValidRequest_ReturnsUpdatedReadingList()
        {
            // Arrange
            var command = new AddBookToReadingListCommand
            {
                ReadingListId = 1,
                UserId = 1,
                BookId = 1
            };

            var readingList = new ReadingList
            {
                Id = 1,
                UserId = command.UserId,
                Books = new List<Book>()
            };

            var book = new Book
            {
                Id = command.BookId,
                Title = "Book Title",
                Description = "Book Description",
                PublicationDate = DateTime.UtcNow
            };

            var updatedReadingList = new ReadingList
            {
                Id = readingList.Id,
                UserId = readingList.UserId,
                Books = new List<Book> { book }
            };

            var readingListDto = new ReadingListDto
            {
                Id = updatedReadingList.Id,
                Books = new List<BookDto>
                {
                    new BookDto
                    {
                        Id = book.Id,
                        Title = book.Title,
                        Description = book.Description,
                        PublicationDate = book.PublicationDate
                    }
                }
            };

            _readingListRepositoryMock.Setup(r => r.GetReadingListIncludingCollectionsAsync(command.ReadingListId))
                .ReturnsAsync(readingList);

            _bookRepositoryMock.Setup(r => r.GetBookByIdAsync(command.BookId))
                .ReturnsAsync(book);

            _readingListRepositoryMock.Setup(r => r.UpdateReadingListAsync(readingList))
                .ReturnsAsync(updatedReadingList);

            _mapperMock.Setup(m => m.Map<ReadingListDto>(updatedReadingList))
                .Returns(readingListDto);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            var expectedResponse = new OkObjectResult(readingListDto);
            Assert.Equal(expectedResponse.StatusCode, (result as OkObjectResult)?.StatusCode);
            Assert.Equal(expectedResponse.Value, (result as OkObjectResult)?.Value);
        }

        [Fact]
        public async Task Handle_WithNonExistentReadingList_ReturnsNotFound()
        {
            // Arrange
            var command = new AddBookToReadingListCommand
            {
                ReadingListId = 1,
                UserId = 1,
                BookId = 1
            };

            _readingListRepositoryMock.Setup(r => r.GetReadingListIncludingCollectionsAsync(command.ReadingListId))
                .ReturnsAsync((ReadingList)null);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            var expectedResponse = new NotFoundObjectResult($"Reading list NOT FOUND with ID {command.ReadingListId}");
            Assert.Equal(expectedResponse.StatusCode, (result as NotFoundObjectResult)?.StatusCode);
            Assert.Equal(expectedResponse.Value, (result as NotFoundObjectResult)?.Value);
        }

        [Fact]
        public async Task Handle_WithUnauthorizedUser_ReturnsForbidden()
        {
            // Arrange
            var command = new AddBookToReadingListCommand
            {
                ReadingListId = 1,
                UserId = 1,
                BookId = 1
            };

            var readingList = new ReadingList
            {
                Id = 1,
                UserId = 2,
                Books = new List<Book>()
            };

            _readingListRepositoryMock.Setup(r => r.GetReadingListIncludingCollectionsAsync(command.ReadingListId))
                .ReturnsAsync(readingList);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            var expectedResponse = new ForbidResult($"User does not own reading list with ID {command.ReadingListId}");
            Assert.IsType<ForbidResult>(result);
        }

        [Fact]
        public async Task Handle_WithNonExistentBook_ReturnsNotFound()
        {
            // Arrange
            var command = new AddBookToReadingListCommand
            {
                ReadingListId = 1,
                UserId = 1,
                BookId = 1
            };

            var readingList = new ReadingList
            {
                Id = 1,
                UserId = command.UserId,
                Books = new List<Book>()
            };

            _readingListRepositoryMock.Setup(r => r.GetReadingListIncludingCollectionsAsync(command.ReadingListId))
                .ReturnsAsync(readingList);

            _bookRepositoryMock.Setup(r => r.GetBookByIdAsync(command.BookId))
                .ReturnsAsync((Book)null);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            var expectedResponse = new NotFoundObjectResult($"Book NOT FOUND with ID {command.BookId}");
            Assert.Equal(expectedResponse.StatusCode, (result as NotFoundObjectResult)?.StatusCode);
            Assert.Equal(expectedResponse.Value, (result as NotFoundObjectResult)?.Value);
        }

        [Fact]
        public async Task Handle_WithExistingBookInReadingList_ReturnsNoContent()
        {
            // Arrange
            
            var command = new AddBookToReadingListCommand
            {
                ReadingListId = 1,
                UserId = 1,
                BookId = 1
            };
            Book book = new Book
            {
                Id = command.BookId
            };

            var readingList = new ReadingList
            {
                Id = 1,
                UserId = command.UserId,
                Books = new List<Book> { book }
            };

            _readingListRepositoryMock.Setup(r => r.GetReadingListIncludingCollectionsAsync(command.ReadingListId))
                .ReturnsAsync(readingList);
            _bookRepositoryMock.Setup(r=>r.GetBookByIdAsync(command.BookId)).ReturnsAsync(book);


            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }
    }
}
