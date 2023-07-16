using System.Threading;
using System.Threading.Tasks;
using Application.DTOs;
using Application.Entities.Authors.Commands;
using Application.Entities.Authors.Handlers;
using Application.Interfaces;
using AutoMapper;
using Domain;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Tests.Handlers.Authors
{
    public class AddBookToAuthorCommandHandlerTests
    {
        private readonly Mock<IAuthorRepository> _authorRepositoryMock;
        private readonly Mock<IBookRepository> _bookRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<ILogger<AddBookToAuthorCommandHandler>> _loggerMock;
        private readonly AddBookToAuthorCommandHandler _handler;

        public AddBookToAuthorCommandHandlerTests()
        {
            _authorRepositoryMock = new Mock<IAuthorRepository>();
            _bookRepositoryMock = new Mock<IBookRepository>();
            _mapperMock = new Mock<IMapper>();
            _loggerMock = new Mock<ILogger<AddBookToAuthorCommandHandler>>();
            _handler = new AddBookToAuthorCommandHandler(
                _authorRepositoryMock.Object,
                _bookRepositoryMock.Object,
                _mapperMock.Object,
                _loggerMock.Object
            );
        }

        [Fact]
        public async Task Handle_ValidCommand_ReturnsOkObjectResult()
        {
            // Arrange
            var authorId = 1;
            var bookId = 1;
            var author = new Author { Id = authorId, Name = "John Doe", Description = "Author description", Books = new List<Book>() };
            var book = new Book { Id = bookId, Title = "Book Title", Description = "Book description", PublicationDate = DateTime.Now };
            var command = new AddBookToAuthorCommand { AuthorId = authorId, BookId = bookId };
            var cancellationToken = new CancellationToken();
            var authorDto = new AuthorDto { Id = authorId, Name = "John Doe", Description = "Author description", Books = new List<BookDto>() };
            var okResult = new OkObjectResult(authorDto);

            _authorRepositoryMock.Setup(m => m.GetAuthorByIdIncludingCollectionsAsync(authorId)).ReturnsAsync(author);
            _bookRepositoryMock.Setup(m => m.GetBookByIdAsync(bookId)).ReturnsAsync(book);
            _authorRepositoryMock.Setup(m => m.UpdateAuthorAsync(It.IsAny<Author>())).ReturnsAsync(author);
            _mapperMock.Setup(m => m.Map<AuthorDto>(author)).Returns(authorDto);

            // Act
            var result = await _handler.Handle(command, cancellationToken);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<OkObjectResult>(result);
            Assert.Equal(okResult.Value, (result as OkObjectResult).Value);
        }

        [Fact]
        public async Task Handle_AuthorNotFound_ReturnsNotFoundObjectResult()
        {
            // Arrange
            var authorId = 1;
            var bookId = 1;
            var command = new AddBookToAuthorCommand { AuthorId = authorId, BookId = bookId };
            var cancellationToken = new CancellationToken();
            var notFoundResult = new NotFoundObjectResult("Author with Id 1 was not found.");

            _authorRepositoryMock.Setup(m => m.GetAuthorByIdIncludingCollectionsAsync(authorId)).ReturnsAsync((Author)null);

            // Act
            var result = await _handler.Handle(command, cancellationToken);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal(notFoundResult.Value, (result as NotFoundObjectResult).Value);
        }

        [Fact]
        public async Task Handle_BookNotFound_ReturnsNotFoundObjectResult()
        {
            // Arrange
            var authorId = 1;
            var bookId = 1;
            var author = new Author { Id = authorId, Name = "John Doe", Description = "Author description", Books = new List<Book>() };
            var command = new AddBookToAuthorCommand { AuthorId = authorId, BookId = bookId };
            var cancellationToken = new CancellationToken();
            var notFoundResult = new NotFoundObjectResult("Book with Id 1 was not found.");

            _authorRepositoryMock.Setup(m => m.GetAuthorByIdIncludingCollectionsAsync(authorId)).ReturnsAsync(author);
            _bookRepositoryMock.Setup(m => m.GetBookByIdAsync(bookId)).ReturnsAsync((Book)null);

            // Act
            var result = await _handler.Handle(command, cancellationToken);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal(notFoundResult.Value, (result as NotFoundObjectResult).Value);
        }

        [Fact]
        public async Task Handle_BookAlreadyInAuthorCollection_ReturnsBadRequestObjectResult()
        {
            // Arrange
            var authorId = 1;
            var bookId = 1;
            var author = new Author { Id = authorId, Name = "John Doe", Description = "Author description", Books = new List<Book> { new Book { Id = bookId } } };
            var book = new Book { Id = bookId, Title = "Book Title", Description = "Book description", PublicationDate = DateTime.Now };
            var command = new AddBookToAuthorCommand { AuthorId = authorId, BookId = bookId };
            var cancellationToken = new CancellationToken();
            var badRequestResult = new BadRequestObjectResult("Book with Id 1 is already in the author's collection.");

            _authorRepositoryMock.Setup(m => m.GetAuthorByIdIncludingCollectionsAsync(authorId)).ReturnsAsync(author);
            _bookRepositoryMock.Setup(m => m.GetBookByIdAsync(bookId)).ReturnsAsync(book);

            // Act
            var result = await _handler.Handle(command, cancellationToken);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(badRequestResult.Value, (result as BadRequestObjectResult).Value);
        }
    }
}
