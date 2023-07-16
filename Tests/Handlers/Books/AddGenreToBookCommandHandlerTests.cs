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
    public class AddGenreToBookCommandHandlerTests
    {
        private readonly Mock<IBookRepository> _bookRepositoryMock;
        private readonly Mock<IGenreRepository> _genreRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<ILogger<AddGenreToBookCommandHandler>> _loggerMock;

        private readonly AddGenreToBookCommandHandler _handler;

        public AddGenreToBookCommandHandlerTests()
        {
            _bookRepositoryMock = new Mock<IBookRepository>();
            _genreRepositoryMock = new Mock<IGenreRepository>();
            _mapperMock = new Mock<IMapper>();
            _loggerMock = new Mock<ILogger<AddGenreToBookCommandHandler>>();

            _handler = new AddGenreToBookCommandHandler(
                _bookRepositoryMock.Object,
                _genreRepositoryMock.Object,
                _mapperMock.Object,
                _loggerMock.Object
            );
        }

        [Fact]
        public async Task Handle_WithValidData_ReturnsOkResult()
        {
            // Arrange
            var bookId = 1;
            var genreId = 1;
            var command = new AddGenreToBookCommand { BookId = bookId, GenreId = genreId };
            var book = new Book(); // create a book instance
            var genre = new Genre(); // create a genre instance

            _bookRepositoryMock.Setup(r => r.GetBookByIdAsync(bookId)).ReturnsAsync(book);
            _genreRepositoryMock.Setup(r => r.GetGenreByIdAsync(genreId)).ReturnsAsync(genre);
            _bookRepositoryMock.Setup(r => r.UpdateBookAsync(book)).ReturnsAsync(book);
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
            var bookId = 1;
            var genreId = 1;
            var command = new AddGenreToBookCommand { BookId = bookId, GenreId = genreId };

            _bookRepositoryMock.Setup(r => r.GetBookByIdAsync(bookId)).ReturnsAsync(null as Book);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public async Task Handle_WithNonExistingGenre_ReturnsNotFoundResult()
        {
            // Arrange
            var bookId = 1;
            var genreId = 1;
            var command = new AddGenreToBookCommand { BookId = bookId, GenreId = genreId };
            var book = new Book(); // create a book instance

            _bookRepositoryMock.Setup(r => r.GetBookByIdAsync(bookId)).ReturnsAsync(book);
            _genreRepositoryMock.Setup(r => r.GetGenreByIdAsync(genreId)).ReturnsAsync(null as Genre);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public async Task Handle_WithExistingGenre_ReturnsBadRequestResult()
        {
            // Arrange
            var bookId = 1;
            var genreId = 1;
            var command = new AddGenreToBookCommand { BookId = bookId, GenreId = genreId };
            var book = new Book(); // create a book instance
            var genre = new Genre { Id = genreId };

            book.Genres.Add(genre);

            _bookRepositoryMock.Setup(r => r.GetBookByIdAsync(bookId)).ReturnsAsync(book);
            _genreRepositoryMock.Setup(r => r.GetGenreByIdAsync(genreId)).ReturnsAsync(genre);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }
    }
}
