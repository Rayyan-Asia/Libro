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
    public class RemoveGenreFromBookCommandHandlerTests
    {
        private readonly Mock<IBookRepository> _bookRepositoryMock;
        private readonly Mock<IGenreRepository> _genreRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<ILogger<RemoveGenreFromBookCommandHandler>> _loggerMock;

        private readonly RemoveGenreFromBookCommandHandler _handler;

        public RemoveGenreFromBookCommandHandlerTests()
        {
            _bookRepositoryMock = new Mock<IBookRepository>();
            _genreRepositoryMock = new Mock<IGenreRepository>();
            _mapperMock = new Mock<IMapper>();
            _loggerMock = new Mock<ILogger<RemoveGenreFromBookCommandHandler>>();

            _handler = new RemoveGenreFromBookCommandHandler(
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
            var command = new RemoveGenreFromBookCommand { BookId = 1, GenreId = 1 };
            var book = new Book { Id = command.BookId };
            var genre = new Genre { Id = command.GenreId };

            book.Genres.Add(genre);
            book.Genres.Add(new Genre { Id = 5});


            _bookRepositoryMock.Setup(r => r.GetBookByIdAsync(command.BookId)).ReturnsAsync(book);
            _genreRepositoryMock.Setup(r => r.GetGenreByIdAsync(command.GenreId)).ReturnsAsync(genre);
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
            var command = new RemoveGenreFromBookCommand { BookId = 1, GenreId = 1 };

            _bookRepositoryMock.Setup(r => r.GetBookByIdAsync(command.BookId)).ReturnsAsync(null as Book);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public async Task Handle_WithNonExistingGenre_ReturnsNotFoundResult()
        {
            // Arrange
            var command = new RemoveGenreFromBookCommand { BookId = 1, GenreId = 1 };
            var book = new Book { Id = command.BookId };

            _bookRepositoryMock.Setup(r => r.GetBookByIdAsync(command.BookId)).ReturnsAsync(book);
            _genreRepositoryMock.Setup(r => r.GetGenreByIdAsync(command.GenreId)).ReturnsAsync(null as Genre);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public async Task Handle_WithGenreNotAssociatedWithBook_ReturnsBadRequestResult()
        {
            // Arrange
            var command = new RemoveGenreFromBookCommand { BookId = 1, GenreId = 1 };
            var book = new Book { Id = command.BookId };
            var genre = new Genre { Id = 2 };

            book.Genres.Add(genre);

            _bookRepositoryMock.Setup(r => r.GetBookByIdAsync(command.BookId)).ReturnsAsync(book);
            _genreRepositoryMock.Setup(r => r.GetGenreByIdAsync(command.GenreId)).ReturnsAsync(genre);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task Handle_WithOnlyOneGenreInBook_ReturnsBadRequestResult()
        {
            // Arrange
            var command = new RemoveGenreFromBookCommand { BookId = 1, GenreId = 1 };
            var book = new Book { Id = command.BookId };
            var genre = new Genre { Id = command.GenreId };

            book.Genres.Add(genre);


            _bookRepositoryMock.Setup(r => r.GetBookByIdAsync(command.BookId)).ReturnsAsync(book);
            _genreRepositoryMock.Setup(r => r.GetGenreByIdAsync(command.GenreId)).ReturnsAsync(genre);
            _bookRepositoryMock.Setup(r => r.UpdateBookAsync(It.IsAny<Book>())).ReturnsAsync(book);
            _mapperMock.Setup(m => m.Map<BookDto>(book)).Returns(new BookDto());

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);


            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }
    }
}
