using System.Threading;
using System.Threading.Tasks;
using Application.DTOs;
using Application.Entities.Authors.Commands;
using Application.Entities.Authors.Handlers;
using Application.Interfaces;
using AutoMapper;
using Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Tests.Handlers.Authors
{
    public class RemoveBookFromAuthorCommandHandlerTests
    {
        private readonly Mock<IAuthorRepository> _authorRepositoryMock;
        private readonly Mock<IBookRepository> _bookRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<ILogger<RemoveBookFromAuthorCommandHandler>> _loggerMock;
        private readonly RemoveBookFromAuthorCommandHandler _handler;

        public RemoveBookFromAuthorCommandHandlerTests()
        {
            _authorRepositoryMock = new Mock<IAuthorRepository>();
            _bookRepositoryMock = new Mock<IBookRepository>();
            _mapperMock = new Mock<IMapper>();
            _loggerMock = new Mock<ILogger<RemoveBookFromAuthorCommandHandler>>();
            _handler = new RemoveBookFromAuthorCommandHandler(
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
            var command = new RemoveBookFromAuthorCommand { AuthorId = authorId, BookId = bookId };
            var cancellationToken = new CancellationToken();
            var author = new Author { Id = authorId };
            var book = new Book { Id = bookId };

            author.Books.Add(book);

            _authorRepositoryMock.Setup(m => m.GetAuthorByIdIncludingCollectionsAsync(authorId)).ReturnsAsync(author);
            _bookRepositoryMock.Setup(m => m.GetBookByIdAsync(bookId)).ReturnsAsync(book);
            _authorRepositoryMock.Setup(m => m.UpdateAuthorAsync(author)).ReturnsAsync(author);
            _mapperMock.Setup(m => m.Map<AuthorDto>(author)).Returns(new AuthorDto());

            // Act
            var result = await _handler.Handle(command, cancellationToken);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task Handle_AuthorNotFound_ReturnsNotFoundObjectResult()
        {
            // Arrange
            var authorId = 1;
            var bookId = 1;
            var command = new RemoveBookFromAuthorCommand { AuthorId = authorId, BookId = bookId };
            var cancellationToken = new CancellationToken();

            _authorRepositoryMock.Setup(m => m.GetAuthorByIdIncludingCollectionsAsync(authorId)).ReturnsAsync((Author)null);

            // Act
            var result = await _handler.Handle(command, cancellationToken);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public async Task Handle_BookNotFound_ReturnsNotFoundObjectResult()
        {
            // Arrange
            var authorId = 1;
            var bookId = 1;
            var command = new RemoveBookFromAuthorCommand { AuthorId = authorId, BookId = bookId };
            var cancellationToken = new CancellationToken();
            var author = new Author { Id = authorId };

            _authorRepositoryMock.Setup(m => m.GetAuthorByIdIncludingCollectionsAsync(authorId)).ReturnsAsync(author);
            _bookRepositoryMock.Setup(m => m.GetBookByIdAsync(bookId)).ReturnsAsync((Book)null);

            // Act
            var result = await _handler.Handle(command, cancellationToken);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public async Task Handle_BookNotAssociatedWithAuthor_ReturnsBadRequestObjectResult()
        {
            // Arrange
            var authorId = 1;
            var bookId = 1;
            var command = new RemoveBookFromAuthorCommand { AuthorId = authorId, BookId = bookId };
            var cancellationToken = new CancellationToken();
            var author = new Author { Id = authorId };
            var book = new Book { Id = 1 };

            author.Books.Add(new Book {Id = 2 });

            _authorRepositoryMock.Setup(m => m.GetAuthorByIdIncludingCollectionsAsync(authorId)).ReturnsAsync(author);
            _bookRepositoryMock.Setup(m => m.GetBookByIdAsync(bookId)).ReturnsAsync(book);

            // Act
            var result = await _handler.Handle(command, cancellationToken);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<BadRequestObjectResult>(result);
        }
    }
}
