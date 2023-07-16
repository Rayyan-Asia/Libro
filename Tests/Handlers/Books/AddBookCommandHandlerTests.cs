using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.DTOs;
using Application.Entities.Books.Commands;
using Application.Entities.Books.Handlers;
using Application.Interfaces;
using AutoMapper;
using Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Tests.Handlers.Books
{
    public class AddBookCommandHandlerTests
    {
        private readonly Mock<IBookRepository> _bookRepositoryMock;
        private readonly Mock<IAuthorRepository> _authorRepositoryMock;
        private readonly Mock<IGenreRepository> _genreRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<ILogger<AddBookCommandHandler>> _loggerMock;

        private readonly AddBookCommandHandler _handler;

        public AddBookCommandHandlerTests()
        {
            _bookRepositoryMock = new Mock<IBookRepository>();
            _authorRepositoryMock = new Mock<IAuthorRepository>();
            _genreRepositoryMock = new Mock<IGenreRepository>();
            _mapperMock = new Mock<IMapper>();
            _loggerMock = new Mock<ILogger<AddBookCommandHandler>>();

            _handler = new AddBookCommandHandler(
                _bookRepositoryMock.Object,
                _authorRepositoryMock.Object,
                _genreRepositoryMock.Object,
                _mapperMock.Object,
                _loggerMock.Object
            );
        }

        [Fact]
        public async Task Handle_WithValidData_ReturnsOkResult()
        {
            // Arrange
            var command = new AddBookCommand
            {
                Title = "Sample Book",
                Description = "Sample Description",
                PublicationDate = new DateTime(2023, 7, 13),
                Authors = new List<IdDto> { new IdDto { Id = 1 } },
                Genres = new List<IdDto> { new IdDto { Id = 1 } }
            };
            var book = new Book { Id = 1 }; // create a book instance

            _authorRepositoryMock.Setup(r => r.AuthorExistsAsync(It.IsAny<int>())).ReturnsAsync(true);
            _genreRepositoryMock.Setup(r => r.GenreExistsAsync(It.IsAny<int>())).ReturnsAsync(true);
            _bookRepositoryMock.Setup(r => r.AddBookAsync(It.IsAny<Book>())).ReturnsAsync(book);
            _bookRepositoryMock.Setup(r => r.UpdateBookAsync(It.IsAny<Book>())).ReturnsAsync(book);
            _mapperMock.Setup(m => m.Map<BookDto>(It.IsAny<Book>())).Returns(new BookDto());

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task Handle_WithInvalidAuthorId_ReturnsBadRequestResult()
        {
            // Arrange
            var command = new AddBookCommand
            {
                Title = "Sample Book",
                Description = "Sample Description",
                PublicationDate = new DateTime(2023, 7, 13),
                Authors = new List<IdDto> { new IdDto { Id = 1 } },
                Genres = new List<IdDto> { new IdDto { Id = 1 } }
            };

            _authorRepositoryMock.Setup(r => r.AuthorExistsAsync(It.IsAny<int>())).ReturnsAsync(false);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task Handle_WithInvalidGenreId_ReturnsBadRequestResult()
        {
            // Arrange
            var command = new AddBookCommand
            {
                Title = "Sample Book",
                Description = "Sample Description",
                PublicationDate = new DateTime(2023, 7, 13),
                Authors = new List<IdDto> { new IdDto { Id = 1 } },
                Genres = new List<IdDto> { new IdDto { Id = 1 } }
            };

            _authorRepositoryMock.Setup(r => r.AuthorExistsAsync(It.IsAny<int>())).ReturnsAsync(true);
            _genreRepositoryMock.Setup(r => r.GenreExistsAsync(It.IsAny<int>())).ReturnsAsync(false);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task Handle_WithNoAuthorsOrGenres_ReturnsBadRequestResult()
        {
            // Arrange
            var command = new AddBookCommand
            {
                Title = "Sample Book",
                Description = "Sample Description",
                PublicationDate = new DateTime(2023, 7, 13),
                Authors = new List<IdDto>(),
                Genres = new List<IdDto>()
            };

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }
    }
}
