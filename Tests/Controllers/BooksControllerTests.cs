using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using Application.DTOs;
using Application;
using Application.Entities.Authors.Commands;
using Application.Entities.Books.Commands;
using Application.Entities.Books.Queries;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Presentation.Controllers;
using Presentation.Validators.Books;
using Xunit;

namespace Tests.Controllers
{
    public class BooksControllerTests
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly SearchQueryValidator _searchQueryValidator;
        private readonly AddBookCommandValidator _addBookCommandValidator;
        private readonly EditBookCommandValidator _editBookCommandValidator;
        private readonly AddAuthorToBookCommandValidator _addAuthorToBookCommandValidator;
        private readonly RemoveAuthorFromBookCommandValidator _removeAuthorFromBookCommandValidator;
        private readonly AddGenreToBookCommandValidator _addGenreToBookCommandValidator;
        private readonly RemoveGenreFromBookCommandValidator _removeGenreFromBookCommandValidator;
        private readonly RemoveBookCommandValidator _removeBookCommandValidator;

        private readonly BooksController _controller;

        public BooksControllerTests()
        {
            _mediatorMock = new Mock<IMediator>();
            _searchQueryValidator = new SearchQueryValidator();
            _addBookCommandValidator = new AddBookCommandValidator();
            _editBookCommandValidator = new EditBookCommandValidator();
            _addAuthorToBookCommandValidator = new AddAuthorToBookCommandValidator();
            _removeAuthorFromBookCommandValidator = new RemoveAuthorFromBookCommandValidator();
            _addGenreToBookCommandValidator = new AddGenreToBookCommandValidator();
            _removeGenreFromBookCommandValidator = new RemoveGenreFromBookCommandValidator();
            _removeBookCommandValidator = new RemoveBookCommandValidator();

            _controller = new BooksController(
                _mediatorMock.Object,
                _searchQueryValidator,
                _addBookCommandValidator,
                _editBookCommandValidator,
                _addAuthorToBookCommandValidator,
                _removeAuthorFromBookCommandValidator,
                _addGenreToBookCommandValidator,
                _removeGenreFromBookCommandValidator,
                _removeBookCommandValidator
            );
        }

        [Fact]
        public async Task Index_DefaultQuery_ReturnsOkResultWithBooks()
        {
            // Arrange
            var browseBooksQuery = new BrowseBooksQuery();
            var pagination = new PaginationMetadata();
            var books = new List<BookDto> { new BookDto() };

            _mediatorMock.Setup(m => m.Send(browseBooksQuery, default))
                .ReturnsAsync((pagination, books));

            // Create and set up the HttpContext with response headers
            var httpContext = new DefaultHttpContext();

            _controller.ControllerContext = new ControllerContext()
            {
                HttpContext = httpContext
            };

            // Act
            var result = await _controller.Index(browseBooksQuery) as IActionResult;

            // Assert
            Assert.NotNull(result);
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(books, okResult.Value);

            // Verify response headers
            Assert.True(_controller.Response.Headers.ContainsKey("X-Pagination"));
            var serializedPagination = JsonSerializer.Serialize(pagination);
            Assert.Equal(serializedPagination, _controller.Response.Headers["X-Pagination"]);
        }

        [Fact]
        public async Task Index_NullQuery_ReturnsOkResultWithBooks()
        {
            // Arrange
            var pagination = new PaginationMetadata();
            var books = new List<BookDto> { new BookDto() };

            _mediatorMock.Setup(m => m.Send(It.IsAny<BrowseBooksQuery>(), default))
                .ReturnsAsync((pagination, books));

            // Create and set up the HttpContext with response headers
            var httpContext = new DefaultHttpContext();

            _controller.ControllerContext = new ControllerContext()
            {
                HttpContext = httpContext
            };

            // Act
            var result = await _controller.Index(null) as IActionResult;

            // Assert
            Assert.NotNull(result);
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(books, okResult.Value);

            // Verify response headers
            Assert.True(_controller.Response.Headers.ContainsKey("X-Pagination"));
            var serializedPagination = JsonSerializer.Serialize(pagination);
            Assert.Equal(serializedPagination, _controller.Response.Headers["X-Pagination"]);
        }

        [Fact]
        public async Task Search_ValidQuery_ReturnsOkResultWithBooks()
        {
            // Arrange
            var searchQuery = new SearchQuery();
            var pagination = new PaginationMetadata();
            var books = new List<BookDto> { new BookDto() };

            _mediatorMock.Setup(m => m.Send(searchQuery, default))
                .ReturnsAsync((pagination, books));

            // Create and set up the HttpContext with response headers
            var httpContext = new DefaultHttpContext();

            _controller.ControllerContext = new ControllerContext()
            {
                HttpContext = httpContext
            };

            // Act
            var result = await _controller.Search(searchQuery) as IActionResult;

            // Assert
            Assert.NotNull(result);
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(books, okResult.Value);

            // Verify response headers
            Assert.True(_controller.Response.Headers.ContainsKey("X-Pagination"));
            var serializedPagination = JsonSerializer.Serialize(pagination);
            Assert.Equal(serializedPagination, _controller.Response.Headers["X-Pagination"]);
        }

        [Fact]
        public async Task Search_NullQuery_ReturnsOkResultWithBooks()
        {
            // Arrange
            var pagination = new PaginationMetadata();
            var books = new List<BookDto> { new BookDto() };

            _mediatorMock.Setup(m => m.Send(It.IsAny<SearchQuery>(), default))
                .ReturnsAsync((pagination, books));

            // Create and set up the HttpContext with response headers
            var httpContext = new DefaultHttpContext();

            _controller.ControllerContext = new ControllerContext()
            {
                HttpContext = httpContext
            };

            // Act
            var result = await _controller.Search(null) as IActionResult;

            // Assert
            Assert.NotNull(result);
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(books, okResult.Value);

            // Verify response headers
            Assert.True(_controller.Response.Headers.ContainsKey("X-Pagination"));
            var serializedPagination = JsonSerializer.Serialize(pagination);
            Assert.Equal(serializedPagination, _controller.Response.Headers["X-Pagination"]);
        }

        [Fact]
        public async Task Add_ValidCommand_ReturnsOkResult()
        {
            // Arrange
            var addBookCommand = new AddBookCommand() 
            { 
                Title= "Title",
                Description= "Description",
                PublicationDate= DateTime.Now,
                Genres = new List<IdDto> { new IdDto() { Id = 1 } },
                Authors = new List<IdDto> { new IdDto() { Id = 1 } }
            };
            _mediatorMock.Setup(m => m.Send(addBookCommand, default))
                .ReturnsAsync(new OkResult());

            // Act
            var result = await _controller.Add(addBookCommand) as IActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async Task Add_InvalidCommand_ReturnsBadRequest()
        {
            // Arrange
            var addBookCommand = new AddBookCommand();
            var validationResult = new ValidationResult(new List<ValidationFailure>());

            _mediatorMock.Setup(m => m.Send(addBookCommand, default))
                .ReturnsAsync(new BadRequestObjectResult(validationResult));

            // Act
            var result = await _controller.Add(addBookCommand) as IActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task Edit_ValidCommand_ReturnsOkResult()
        {
            // Arrange
            var editBookCommand = new EditBookCommand()
            {
                Id = 1,
                Title = "Title",
                Description = "Description",
                PublicationDate = DateTime.Now,
                Genres = new List<IdDto> { new IdDto() { Id = 1 } },
                Authors = new List<IdDto> { new IdDto() { Id = 1 } }
            };

            _mediatorMock.Setup(m => m.Send(editBookCommand, default))
                .ReturnsAsync(new OkResult());

            // Act
            var result = await _controller.Edit(editBookCommand) as IActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async Task Edit_InvalidCommand_ReturnsBadRequest()
        {
            // Arrange
            var editBookCommand = new EditBookCommand();
            var validationResult = new ValidationResult(new List<ValidationFailure>());

            _mediatorMock.Setup(m => m.Send(editBookCommand, default))
                .ReturnsAsync(new BadRequestObjectResult(validationResult));

            // Act
            var result = await _controller.Edit(editBookCommand) as IActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task AddAuthorToBook_ValidCommand_ReturnsOkResult()
        {
            // Arrange
            var addAuthorToBookCommand = new AddAuthorToBookCommand()
            {
                BookId = 1,
                AuthorId = 2
            };
            _mediatorMock.Setup(m => m.Send(addAuthorToBookCommand, default))
                .ReturnsAsync(new OkResult());

            // Act
            var result = await _controller.AddAuthorToBook(addAuthorToBookCommand) as IActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async Task AddAuthorToBook_InvalidCommand_ReturnsBadRequest()
        {
            // Arrange
            var addAuthorToBookCommand = new AddAuthorToBookCommand()
            {
                BookId = 1,
                AuthorId = 2
            }; 
            var validationResult = new ValidationResult(new List<ValidationFailure>());

            _mediatorMock.Setup(m => m.Send(addAuthorToBookCommand, default))
                .ReturnsAsync(new BadRequestObjectResult(validationResult));

            // Act
            var result = await _controller.AddAuthorToBook(addAuthorToBookCommand) as IActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task RemoveAuthorFromBook_ValidCommand_ReturnsOkResult()
        {
            // Arrange
            var removeAuthorFromBookCommand = new RemoveAuthorFromBookCommand()
            {
                BookId = 1,
                AuthorId = 2
            };
            _mediatorMock.Setup(m => m.Send(removeAuthorFromBookCommand, default))
                .ReturnsAsync(new OkResult());

            // Act
            var result = await _controller.RemoveAuthorFromBook(removeAuthorFromBookCommand) as IActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async Task RemoveAuthorFromBook_InvalidCommand_ReturnsBadRequest()
        {
            // Arrange
            var removeAuthorFromBookCommand = new RemoveAuthorFromBookCommand();
            var validationResult = new ValidationResult(new List<ValidationFailure>());

            _mediatorMock.Setup(m => m.Send(removeAuthorFromBookCommand, default))
                .ReturnsAsync(new BadRequestObjectResult(validationResult));

            // Act
            var result = await _controller.RemoveAuthorFromBook(removeAuthorFromBookCommand) as IActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task AddGenreToBook_ValidCommand_ReturnsOkResult()
        {
            // Arrange
            var addGenreToBookCommand = new AddGenreToBookCommand()
            {
                GenreId = 1,
                BookId = 1,
            };
            _mediatorMock.Setup(m => m.Send(addGenreToBookCommand, default))
                .ReturnsAsync(new OkResult());

            // Act
            var result = await _controller.AddGenreToBook(addGenreToBookCommand) as IActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async Task AddGenreToBook_InvalidCommand_ReturnsBadRequest()
        {
            // Arrange
            var addGenreToBookCommand = new AddGenreToBookCommand();
            var validationResult = new ValidationResult(new List<ValidationFailure>());

            _mediatorMock.Setup(m => m.Send(addGenreToBookCommand, default))
                .ReturnsAsync(new BadRequestObjectResult(validationResult));

            // Act
            var result = await _controller.AddGenreToBook(addGenreToBookCommand) as IActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task RemoveGenreFromBook_ValidCommand_ReturnsOkResult()
        {
            // Arrange
            var removeGenreFromBookCommand = new RemoveGenreFromBookCommand()
            {
                GenreId = 1,
                BookId = 1,
            };

            _mediatorMock.Setup(m => m.Send(removeGenreFromBookCommand, default))
                .ReturnsAsync(new OkResult());

            // Act
            var result = await _controller.RemoveGenreFromBook(removeGenreFromBookCommand) as IActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async Task RemoveGenreFromBook_InvalidCommand_ReturnsBadRequest()
        {
            // Arrange
            var removeGenreFromBookCommand = new RemoveGenreFromBookCommand();
            var validationResult = new ValidationResult(new List<ValidationFailure>());

            _mediatorMock.Setup(m => m.Send(removeGenreFromBookCommand, default))
                .ReturnsAsync(new BadRequestObjectResult(validationResult));

            // Act
            var result = await _controller.RemoveGenreFromBook(removeGenreFromBookCommand) as IActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task RemoveBook_ValidCommand_ReturnsOkResult()
        {
            // Arrange
            _mediatorMock.Setup(m => m.Send(It.IsAny<RemoveBookCommand>(), default))
                .ReturnsAsync(new OkResult());

            // Act
            var result = await _controller.RemoveBook(1) as IActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async Task RemoveBook_InvalidCommand_ReturnsBadRequest()
        {
            //Arrange
            int bookId = 0;

            // Act
            var result = await _controller.RemoveBook(bookId) as IActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.IsType<BadRequestResult>(result);
        }
    }
}