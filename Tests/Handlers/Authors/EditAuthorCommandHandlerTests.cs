using System.Collections.Generic;
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
    public class EditAuthorCommandHandlerTests
    {
        private readonly Mock<IAuthorRepository> _authorRepositoryMock;
        private readonly Mock<IBookRepository> _bookRepositoryMock;
        private readonly Mock<IBookAuthorRepository> _bookAuthorRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<ILogger<EditAuthorCommandHandler>> _loggerMock;
        private readonly EditAuthorCommandHandler _handler;

        public EditAuthorCommandHandlerTests()
        {
            _authorRepositoryMock = new Mock<IAuthorRepository>();
            _bookRepositoryMock = new Mock<IBookRepository>();
            _bookAuthorRepositoryMock = new Mock<IBookAuthorRepository>();
            _mapperMock = new Mock<IMapper>();
            _loggerMock = new Mock<ILogger<EditAuthorCommandHandler>>();
            _handler = new EditAuthorCommandHandler(
                _authorRepositoryMock.Object,
                _bookRepositoryMock.Object,
                _bookAuthorRepositoryMock.Object,
                _mapperMock.Object,
                _loggerMock.Object
            );
        }

        [Fact]
        public async Task Handle_ValidCommand_ReturnsOkObjectResult()
        {
            // Arrange
            var authorId = 1;
            var command = new EditAuthorCommand
            {
                Id = authorId,
                Name = "John Doe",
                Description = "Updated description",
                Books = new List<IdDto> { new IdDto { Id = 1}, new IdDto { Id = 2 } }
            };
            var cancellationToken = new CancellationToken();
            var author = new Author { Id = authorId, Name = "John Smith", Description = "Old description" };
            var updatedAuthor = new Author { Id = authorId, Name = "John Doe", Description = "Updated description" };
            var book1 = new Book { Id = 1, Title = "Book 1" };
            var book2 = new Book { Id = 2, Title = "Book 2" };
            var bookDtos = new List<BookDto> { new BookDto { Id = 1, Title = "Book 1" }, new BookDto { Id = 2, Title = "Book 2" } };
            var authorDto = new AuthorDto { Id = authorId, Name = "John Doe", Description = "Updated description" };
            updatedAuthor.Books .Add( book1 );
            updatedAuthor.Books .Add( book2 );  
            _authorRepositoryMock.Setup(m => m.GetAuthorByIdAsync(authorId)).ReturnsAsync(author);
            _bookRepositoryMock.Setup(m => m.GetBookByIdAsync(book1.Id)).ReturnsAsync(book1);
            _bookRepositoryMock.Setup(m => m.GetBookByIdAsync(book2.Id)).ReturnsAsync(book2);
            _authorRepositoryMock.Setup(m => m.UpdateAuthorAsync(It.IsAny<Author>())).ReturnsAsync(updatedAuthor);
            _mapperMock.Setup(m => m.Map<AuthorDto>(updatedAuthor)).Returns(authorDto);

            // Act
            var result = await _handler.Handle(command, cancellationToken);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<OkObjectResult>(result);
            var okObjectResult = (OkObjectResult)result;
            Assert.Equal(authorDto, okObjectResult.Value);
        }
    }
}
