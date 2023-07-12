using Application;
using Application.DTOs;
using Application.Entities.Authors.Commands;
using Application.Entities.Authors.Queries;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Presentation.Controllers;
using Presentation.Validators.Authors;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

namespace Tests.Controllers
{
    public class AuthorsControllerTests
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly AddAuthorCommandValidator _addAuthorValidator;
        private readonly EditAuthorCommandValidator _editAuthorValidator;
        private readonly AddBookToAuthorCommandValidator _addBookToAuthorValidator;
        private readonly RemoveBookFromAuthorCommandValidator _removeBookFromAuthorValidator;
        private readonly RemoveAuthorCommandValidator _removeAuthorValidator;

        private readonly AuthorsController _controller;

        public AuthorsControllerTests()
        {
            _mediatorMock = new Mock<IMediator>();
            _addAuthorValidator = new AddAuthorCommandValidator();
            _editAuthorValidator = new EditAuthorCommandValidator();
            _addBookToAuthorValidator = new AddBookToAuthorCommandValidator();
            _removeBookFromAuthorValidator = new RemoveBookFromAuthorCommandValidator();
            _removeAuthorValidator = new RemoveAuthorCommandValidator();

            _controller = new AuthorsController(
                _mediatorMock.Object,
                _addAuthorValidator,
                _editAuthorValidator,
                _addBookToAuthorValidator,
                _removeBookFromAuthorValidator,
                _removeAuthorValidator
            );
        }

        [Fact]
        public async Task AddAuthor_ValidCommand_ReturnsOkResult()
        {
            // Arrange
            var addAuthorCommand = new AddAuthorCommand() {
            Name = "Rayyan",
            Description = "test",
            Books = new List<IdDto>() { new IdDto { Id = 3} }
            };
            

            _mediatorMock.Setup(m => m.Send(addAuthorCommand, default))
                .ReturnsAsync(new OkResult());

            // Act
            var result = await _controller.AddAuthor(addAuthorCommand) as IActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async Task AddAuthor_InvalidCommand_ReturnsBadRequest()
        {
            // Arrange
            var addAuthorCommand = new AddAuthorCommand()
            {
                Name = "Rayyaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaan",
                Description = "test"
            };

            // Act
            var result = await _controller.AddAuthor(addAuthorCommand) as IActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task EditAuthor_ValidCommand_ReturnsOkResult()
        {
            // Arrange
            var editAuthorCommand = new EditAuthorCommand()
            {
                Id = 1,
                Name = "Rayyan",
                Description = "test",
                Books = new List<IdDto>() { new IdDto { Id = 3 } }
            };

            _mediatorMock.Setup(m => m.Send(editAuthorCommand,default))
                .ReturnsAsync(new OkResult());
            // Act
            var result = await _controller.EditAuthor(editAuthorCommand) as IActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async Task EditAuthor_InvalidCommand_ReturnsBadRequest()
        {
            // Arrange
            var editAuthorCommand = new EditAuthorCommand()
            {
                Name = "Rayyaaaaaaaannnnn",
                Description = "test",
                Books = new List<IdDto>() { new IdDto { Id = 3 } }
            };

            // Act
            var result = await _controller.EditAuthor(editAuthorCommand) as IActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task RemoveAuthor_ValidCommand_ReturnsOkResult()
        {
            // Arrange
            
            _mediatorMock.Setup(m => m.Send(It.IsAny<RemoveAuthorCommand>(), default))
                .ReturnsAsync(new OkResult());

            // Act
            var result = await _controller.RemoveAuthor(1) as IActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async Task RemoveAuthor_InvalidCommand_ReturnsBadRequest()
        {
            // Arrange
            var authorId = 0; // Invalid author ID
            var removeAuthorCommand = new RemoveAuthorCommand { Id = authorId };


            // Act
            var result = await _controller.RemoveAuthor(authorId) as IActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public async Task AddBookToAuthor_ValidCommand_ReturnsOkResult()
        {
            // Arrange
            var addBookToAuthorCommand = new AddBookToAuthorCommand()
            {
                AuthorId = 1,
                BookId = 1,
            };

            _mediatorMock.Setup(m => m.Send(addBookToAuthorCommand, default))
                .ReturnsAsync(new OkResult());

            // Act
            var result = await _controller.AddBookToAuthor(addBookToAuthorCommand) as IActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async Task AddBookToAuthor_InvalidCommand_ReturnsBadRequest()
        {
            // Arrange
            var addBookToAuthorCommand = new AddBookToAuthorCommand()
            {
                AuthorId = 1
            };


            // Act
            var result = await _controller.AddBookToAuthor(addBookToAuthorCommand) as IActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task RemoveBookFromAuthor_ValidCommand_ReturnsOkResult()
        {
            // Arrange
            var removeBookFromAuthorCommand = new RemoveBookFromAuthorCommand()
            {
                AuthorId = 1,
                BookId = 1,
            };

            _mediatorMock.Setup(m => m.Send(removeBookFromAuthorCommand, default))
                .ReturnsAsync(new OkResult());

            // Act
            var result = await _controller.RemoveBookFromAuthor(removeBookFromAuthorCommand) as IActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async Task RemoveBookFromAuthor_InvalidCommand_ReturnsBadRequest()
        {
            // Arrange
            var removeBookFromAuthorCommand = new RemoveBookFromAuthorCommand()
            {
                AuthorId = 1,
            };

            // Act
            var result = await _controller.RemoveBookFromAuthor(removeBookFromAuthorCommand) as IActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task Index_DefaultQuery_ReturnsOkResult()
        {
            // Arrange
            var browseAuthorsQuery = new BrowseAuthorsQuery();
            var pagination = new PaginationMetadata();
            var authors = new List<AuthorDto> { new AuthorDto() };

            _mediatorMock.Setup(m => m.Send(browseAuthorsQuery, default))
                .ReturnsAsync((pagination, authors));

            var httpContext = new DefaultHttpContext();

            _controller.ControllerContext = new ControllerContext()
            {
                HttpContext = httpContext
            };

            // Act
            var result = await _controller.Index(browseAuthorsQuery) as IActionResult;

            // Assert
            Assert.NotNull(result);
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(authors, okResult.Value);

            // Verify response headers
            Assert.True(_controller.Response.Headers.ContainsKey("X-Pagination"));
            var serializedPagination = JsonSerializer.Serialize(pagination);
            Assert.Equal(serializedPagination, _controller.Response.Headers["X-Pagination"]);
        }

    }

}

