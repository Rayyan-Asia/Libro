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
    public class EditBookCommandHandlerTests
    {
        private readonly Mock<IBookRepository> _bookRepositoryMock;
        private readonly Mock<IAuthorRepository> _authorRepositoryMock;
        private readonly Mock<IGenreRepository> _genreRepositoryMock;
        private readonly Mock<IBookAuthorRepository> _bookAuthorRepositoryMock;
        private readonly Mock<IBookGenreRepository> _bookGenreRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<ILogger<EditBookCommandHandler>> _loggerMock;

        private readonly EditBookCommandHandler _handler;

        public EditBookCommandHandlerTests()
        {
            _bookRepositoryMock = new Mock<IBookRepository>();
            _authorRepositoryMock = new Mock<IAuthorRepository>();
            _genreRepositoryMock = new Mock<IGenreRepository>();
            _bookAuthorRepositoryMock = new Mock<IBookAuthorRepository>();
            _bookGenreRepositoryMock = new Mock<IBookGenreRepository>();
            _mapperMock = new Mock<IMapper>();
            _loggerMock = new Mock<ILogger<EditBookCommandHandler>>();

            _handler = new EditBookCommandHandler(
                _bookRepositoryMock.Object,
                _authorRepositoryMock.Object,
                _genreRepositoryMock.Object,
                _bookAuthorRepositoryMock.Object,
                _bookGenreRepositoryMock.Object,
                _mapperMock.Object,
                _loggerMock.Object
            );
        }

        [Fact]
        public async Task Handle_WithValidData_ReturnsOkResult()
        {
            // Arrange
            var command = new EditBookCommand
            {
                Id = 1,
                Title = "Sample Book",
                Description = "Sample Description",
                PublicationDate = new DateTime(2023, 7, 13),
                Authors = new List<IdDto> { new IdDto { Id = 1 } },
                Genres = new List<IdDto> { new IdDto { Id = 1 } }
            };
            var book = new Book { Id = command.Id }; // create a book instance
            book .Title = command.Title;
            book .Description = command.Description;
            book.PublicationDate = command.PublicationDate;

            _bookRepositoryMock.Setup(r => r.GetBookByIdAsync(command.Id)).ReturnsAsync(book);
            _authorRepositoryMock.Setup(r => r.AuthorExistsAsync(It.IsAny<int>())).ReturnsAsync(true);
            _genreRepositoryMock.Setup(r => r.GenreExistsAsync(It.IsAny<int>())).ReturnsAsync(true);
            _bookRepositoryMock.Setup(r => r.UpdateBookAsync(It.IsAny<Book>())).ReturnsAsync(book);
            _mapperMock.Setup(m => m.Map<BookDto>(book)).Returns(new BookDto());

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task Handle_WithNonExistingBook_ReturnsNotFoundResult()
        {
            // Arrange
            var command = new EditBookCommand { Id = 1 };

            _bookRepositoryMock.Setup(r => r.GetBookByIdAsync(command.Id)).ReturnsAsync(null as Book);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public async Task Handle_WithInvalidAuthorId_ReturnsBadRequestResult()
        {
            // Arrange
            var command = new EditBookCommand
            {
                Id = 1,
                Authors = new List<IdDto> { new IdDto { Id = 1 } },
                Genres = new List<IdDto> { new IdDto { Id = 1,} }
            };

            _bookRepositoryMock.Setup(r => r.GetBookByIdAsync(command.Id)).ReturnsAsync(new Book());
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
            var command = new EditBookCommand
            {
                Id = 1,
                Authors = new List<IdDto> { new IdDto { Id = 1 } },
                Genres = new List<IdDto> { new IdDto { Id = 1, } }
            };

            _bookRepositoryMock.Setup(r => r.GetBookByIdAsync(command.Id)).ReturnsAsync(new Book());
            _authorRepositoryMock.Setup(r => r.AuthorExistsAsync(It.IsAny<int>())).ReturnsAsync(true);
            _genreRepositoryMock.Setup(r => r.GenreExistsAsync(It.IsAny<int>())).ReturnsAsync(false);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }
    }
}
