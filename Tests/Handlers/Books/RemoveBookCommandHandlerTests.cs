using System.Threading;
using System.Threading.Tasks;
using Application.Entities.Books.Commands;
using Application.Entities.Books.Handlers;
using Application.Interfaces;
using Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Tests.Handlers.Books
{
    public class RemoveBookCommandHandlerTests
    {
        private readonly Mock<IBookRepository> _bookRepositoryMock;
        private readonly Mock<ILogger<RemoveBookCommandHandler>> _loggerMock;

        private readonly RemoveBookCommandHandler _handler;

        public RemoveBookCommandHandlerTests()
        {
            _bookRepositoryMock = new Mock<IBookRepository>();
            _loggerMock = new Mock<ILogger<RemoveBookCommandHandler>>();

            _handler = new RemoveBookCommandHandler(
                _bookRepositoryMock.Object,
                _loggerMock.Object
            );
        }

        [Fact]
        public async Task Handle_WithValidBookId_ReturnsNoContentResult()
        {
            // Arrange
            var command = new RemoveBookCommand { BookId = 1 };
            var book = new Book { Id = command.BookId };

            _bookRepositoryMock.Setup(r => r.GetBookByIdAsync(command.BookId)).ReturnsAsync(book);
            _bookRepositoryMock.Setup(r => r.RemoveBookAsync(book)).Returns(Task.CompletedTask);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task Handle_WithNonExistingBook_ReturnsNotFoundResult()
        {
            // Arrange
            var command = new RemoveBookCommand { BookId = 1 };

            _bookRepositoryMock.Setup(r => r.GetBookByIdAsync(command.BookId)).ReturnsAsync(null as Book);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }
    }
}
