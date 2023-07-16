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
    public class RemoveAuthorFromBookCommandHandlerTests
    {
        private readonly Mock<IBookRepository> _bookRepositoryMock;
        private readonly Mock<IAuthorRepository> _authorRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<ILogger<RemoveAuthorFromBookCommandHandler>> _loggerMock;

        private readonly RemoveAuthorFromBookCommandHandler _handler;

        public RemoveAuthorFromBookCommandHandlerTests()
        {
            _bookRepositoryMock = new Mock<IBookRepository>();
            _authorRepositoryMock = new Mock<IAuthorRepository>();
            _mapperMock = new Mock<IMapper>();
            _loggerMock = new Mock<ILogger<RemoveAuthorFromBookCommandHandler>>();

            _handler = new RemoveAuthorFromBookCommandHandler(
                _bookRepositoryMock.Object,
                _authorRepositoryMock.Object,
                _mapperMock.Object,
                _loggerMock.Object
            );
        }

        [Fact]
        public async Task Handle_WithValidData_ReturnsOkResult()
        {
            // Arrange
            var command = new RemoveAuthorFromBookCommand { BookId = 1, AuthorId = 1 };
            var book = new Book { Id = command.BookId };
            var author = new Author { Id = command.AuthorId };

            book.Authors.Add(author);
            book.Authors.Add(new Author { Id = 5});
            _bookRepositoryMock.Setup(r => r.GetBookByIdAsync(command.BookId)).ReturnsAsync(book);
            _authorRepositoryMock.Setup(r => r.GetAuthorByIdAsync(command.AuthorId)).ReturnsAsync(author);
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
            var command = new RemoveAuthorFromBookCommand { BookId = 1, AuthorId = 1 };

            _bookRepositoryMock.Setup(r => r.GetBookByIdAsync(command.BookId)).ReturnsAsync(null as Book);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public async Task Handle_WithNonExistingAuthor_ReturnsNotFoundResult()
        {
            // Arrange
            var command = new RemoveAuthorFromBookCommand { BookId = 1, AuthorId = 1 };
            var book = new Book { Id = command.BookId };

            _bookRepositoryMock.Setup(r => r.GetBookByIdAsync(command.BookId)).ReturnsAsync(book);
            _authorRepositoryMock.Setup(r => r.GetAuthorByIdAsync(command.AuthorId)).ReturnsAsync(null as Author);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public async Task Handle_WithAuthorNotAssociatedWithBook_ReturnsBadRequestResult()
        {
            // Arrange
            var command = new RemoveAuthorFromBookCommand { BookId = 1, AuthorId = 1 };
            var book = new Book { Id = command.BookId };
            var author = new Author { Id = 2 };

            book.Authors.Add(author);

            _bookRepositoryMock.Setup(r => r.GetBookByIdAsync(command.BookId)).ReturnsAsync(book);
            _authorRepositoryMock.Setup(r => r.GetAuthorByIdAsync(command.AuthorId)).ReturnsAsync(author);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task Handle_WithOnlyOneAuthorInBook_ReturnsBadRequestResult()
        {
            // Arrange
            var command = new RemoveAuthorFromBookCommand { BookId = 1, AuthorId = 1 };
            var book = new Book { Id = command.BookId };
            var author = new Author { Id = command.AuthorId };

            book.Authors.Add(author);
            _bookRepositoryMock.Setup(r => r.GetBookByIdAsync(command.BookId)).ReturnsAsync(book);
            _authorRepositoryMock.Setup(r => r.GetAuthorByIdAsync(command.AuthorId)).ReturnsAsync(author);
            _bookRepositoryMock.Setup(r => r.UpdateBookAsync(It.IsAny<Book>())).ReturnsAsync(book);
            _mapperMock.Setup(m => m.Map<BookDto>(book)).Returns(new BookDto());

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }
    }
}
