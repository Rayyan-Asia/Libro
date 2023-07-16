using System.Threading;
using System.Threading.Tasks;
using Application.Entities.Authors.Commands;
using Application.Entities.Authors.Handlers;
using Application.Interfaces;
using Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Tests.Handlers.Authors
{
    public class RemoveAuthorCommandHandlerTests
    {
        private readonly Mock<IAuthorRepository> _authorRepositoryMock;
        private readonly Mock<IBookAuthorRepository> _bookAuthorRepositoryMock;
        private readonly Mock<ILogger<RemoveAuthorCommandHandler>> _loggerMock;
        private readonly RemoveAuthorCommandHandler _handler;

        public RemoveAuthorCommandHandlerTests()
        {
            _authorRepositoryMock = new Mock<IAuthorRepository>();
            _bookAuthorRepositoryMock = new Mock<IBookAuthorRepository>();
            _loggerMock = new Mock<ILogger<RemoveAuthorCommandHandler>>();
            _handler = new RemoveAuthorCommandHandler(
                _authorRepositoryMock.Object,
                _bookAuthorRepositoryMock.Object,
                _loggerMock.Object
            );
        }

        [Fact]
        public async Task Handle_ValidCommand_ReturnsNoContentResult()
        {
            // Arrange
            var authorId = 1;
            var command = new RemoveAuthorCommand { Id = authorId };
            var cancellationToken = new CancellationToken();
            var author = new Author { Id = authorId };

            _authorRepositoryMock.Setup(m => m.GetAuthorByIdAsync(authorId)).ReturnsAsync(author);
            _bookAuthorRepositoryMock.Setup(m => m.GetAuthorBooksAsync(authorId)).ReturnsAsync(new List<BookAuthor>());
            _authorRepositoryMock.Setup(m => m.RemoveAuthorAsync(author)).Returns(Task.CompletedTask);

            // Act
            var result = await _handler.Handle(command, cancellationToken);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task Handle_AuthorNotFound_ReturnsNotFoundObjectResult()
        {
            // Arrange
            var authorId = 1;
            var command = new RemoveAuthorCommand { Id = authorId };
            var cancellationToken = new CancellationToken();

            _authorRepositoryMock.Setup(m => m.GetAuthorByIdAsync(authorId)).ReturnsAsync((Author)null);

            // Act
            var result = await _handler.Handle(command, cancellationToken);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public async Task Handle_BookHasOnlyOneAuthor_ReturnsBadRequestObjectResult()
        {
            // Arrange
            var authorId = 1;
            var command = new RemoveAuthorCommand { Id = authorId };
            var cancellationToken = new CancellationToken();
            var author = new Author { Id = authorId };
            var bookAuthor = new BookAuthor { BookId = 1, AuthorId = authorId };

            _authorRepositoryMock.Setup(m => m.GetAuthorByIdAsync(authorId)).ReturnsAsync(author);
            _bookAuthorRepositoryMock.Setup(m => m.GetAuthorBooksAsync(authorId)).ReturnsAsync(new List<BookAuthor> { bookAuthor });
            _bookAuthorRepositoryMock.Setup(m => m.GetBookAuthorsCountAsync(bookAuthor.BookId)).ReturnsAsync(1);

            // Act
            var result = await _handler.Handle(command, cancellationToken);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<BadRequestObjectResult>(result);
        }
    }
}
