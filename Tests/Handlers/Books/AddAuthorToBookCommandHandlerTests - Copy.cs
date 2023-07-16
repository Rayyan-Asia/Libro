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
    public class AddAuthorToBookCommandHandlerTests
    {
        private readonly Mock<IBookRepository> _bookRepositoryMock;
        private readonly Mock<IAuthorRepository> _authorRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<ILogger<AddAuthorToBookCommandHandler>> _loggerMock;

        private readonly AddAuthorToBookCommandHandler _handler;

        public AddAuthorToBookCommandHandlerTests()
        {
            _bookRepositoryMock = new Mock<IBookRepository>();
            _authorRepositoryMock = new Mock<IAuthorRepository>();
            _mapperMock = new Mock<IMapper>();
            _loggerMock = new Mock<ILogger<AddAuthorToBookCommandHandler>>();

            _handler = new AddAuthorToBookCommandHandler(
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
            var bookId = 1;
            var authorId = 1;
            var command = new AddAuthorToBookCommand { BookId = bookId, AuthorId = authorId };
            var book = new Book(); // create a book instance
            var author = new Author(); // create an author instance

            _bookRepositoryMock.Setup(r => r.GetBookByIdAsync(bookId)).ReturnsAsync(book);
            _authorRepositoryMock.Setup(r => r.GetAuthorByIdAsync(authorId)).ReturnsAsync(author);
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
            var authorId = 1;
            var command = new AddAuthorToBookCommand { BookId = bookId, AuthorId = authorId };

            _bookRepositoryMock.Setup(r => r.GetBookByIdAsync(bookId)).ReturnsAsync(null as Book);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public async Task Handle_WithNonExistingAuthor_ReturnsNotFoundResult()
        {
            // Arrange
            var bookId = 1;
            var authorId = 1;
            var command = new AddAuthorToBookCommand { BookId = bookId, AuthorId = authorId };
            var book = new Book(); // create a book instance

            _bookRepositoryMock.Setup(r => r.GetBookByIdAsync(bookId)).ReturnsAsync(book);
            _authorRepositoryMock.Setup(r => r.GetAuthorByIdAsync(authorId)).ReturnsAsync(null as Author);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public async Task Handle_WithExistingAuthor_ReturnsBadRequestResult()
        {
            // Arrange
            var bookId = 1;
            var authorId = 1;
            var command = new AddAuthorToBookCommand { BookId = bookId, AuthorId = authorId };
            var book = new Book(); // create a book instance
            var author = new Author { Id = authorId };

            book.Authors.Add(author);

            _bookRepositoryMock.Setup(r => r.GetBookByIdAsync(bookId)).ReturnsAsync(book);
            _authorRepositoryMock.Setup(r => r.GetAuthorByIdAsync(authorId)).ReturnsAsync(author);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }
    }
}
