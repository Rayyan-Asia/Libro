using System.Collections.Generic;
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
    public class AddAuthorCommandHandlerTests
    {
        private readonly Mock<IAuthorRepository> _authorRepositoryMock;
        private readonly Mock<IBookRepository> _bookRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<ILogger<AddAuthorCommandHandler>> _loggerMock;
        private readonly AddAuthorCommandHandler _handler;

        public AddAuthorCommandHandlerTests()
        {
            _authorRepositoryMock = new Mock<IAuthorRepository>();
            _bookRepositoryMock = new Mock<IBookRepository>();
            _mapperMock = new Mock<IMapper>();
            _loggerMock = new Mock<ILogger<AddAuthorCommandHandler>>();
            _handler = new AddAuthorCommandHandler(
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
            var books = new List<IdDto> { new IdDto { Id = 1 }, new IdDto { Id = 2 } };
            var author = new Author { Name = "John Doe", Description = "Author description", Books = new List<Book>() };
            var command = new AddAuthorCommand { Name = "John Doe", Description = "Author description", Books = books };
            var cancellationToken = new CancellationToken();
            var authorDto = new AuthorDto { Name = "John Doe", Description = "Author description" };
            var okResult = new OkObjectResult(authorDto);

            _bookRepositoryMock.Setup(m => m.GetBookByIdAsync(1)).ReturnsAsync(new Book { Id = 1 });
            _bookRepositoryMock.Setup(m => m.GetBookByIdAsync(2)).ReturnsAsync(new Book { Id = 2 });
            _authorRepositoryMock.Setup(m => m.AddAuthorAsync(It.IsAny<Author>())).ReturnsAsync(author);
            _mapperMock.Setup(m => m.Map<AuthorDto>(author)).Returns(authorDto);

            // Act
            var result = await _handler.Handle(command, cancellationToken);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<OkObjectResult>(result);
            Assert.Equal(okResult.Value, (result as OkObjectResult).Value);
        }

        [Fact]
        public async Task Handle_BookNotFound_ReturnsNotFoundObjectResult()
        {
            // Arrange
            var books = new List<IdDto> { new IdDto { Id = 1 }, new IdDto { Id = 2 } };
            var command = new AddAuthorCommand { Name = "John Doe", Description = "Author description", Books = books };
            var cancellationToken = new CancellationToken();
            var notFoundResult = new NotFoundObjectResult("Book with id 2 not found.");

            _bookRepositoryMock.Setup(m => m.GetBookByIdAsync(1)).ReturnsAsync(new Book { Id = 1 });
            _bookRepositoryMock.Setup(m => m.GetBookByIdAsync(2)).ReturnsAsync((Book)null);

            // Act
            var result = await _handler.Handle(command, cancellationToken);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal(notFoundResult.Value, (result as NotFoundObjectResult).Value);
        }
    }
}
