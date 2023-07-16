using System;
using System.Text.Json;
using System.Threading.Tasks;
using Application;
using Application.DTOs;
using Application.Entities.ReadingLists.Commands;
using Application.Entities.ReadingLists.Queries;
using Application.Services;
using Domain;
using FluentValidation.Results;
using Infrastructure;
using Infrastructure.Services;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Presentation.Controllers;
using Presentation.Validators.ReadingLists;
using Xunit;

namespace Tests.Controllers
{
    public class ReadingListsControllerTests
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly AddBookToReadingListCommandValidator _addBookToReadingListValidator;
        private readonly AddReadingListCommandValidator _addReadingListValidator;
        private readonly BrowseReadingListsQueryValidator _browseReadingListQueryValidator;
        private readonly EditReadingListCommandValidator _editReadingListValidator;
        private readonly RemoveBookFromReadingListCommandValidator _removeBookFromReadingListValidator;
        private readonly RemoveReadingListCommandValidator _removeReadingListValidator;
        private readonly ReadingListsController _controller;
        private readonly IJwtService _jwtService;

        public ReadingListsControllerTests()
        {
            _mediatorMock = new Mock<IMediator>();
            _addBookToReadingListValidator = new AddBookToReadingListCommandValidator();
            _addReadingListValidator = new AddReadingListCommandValidator();
            _browseReadingListQueryValidator = new BrowseReadingListsQueryValidator();
            _editReadingListValidator = new EditReadingListCommandValidator();
            _removeBookFromReadingListValidator = new RemoveBookFromReadingListCommandValidator();
            _removeReadingListValidator = new RemoveReadingListCommandValidator();
            _jwtService = new JwtService();

            _controller = new ReadingListsController(
                _mediatorMock.Object,
                _addBookToReadingListValidator,
                _addReadingListValidator,
                _browseReadingListQueryValidator,
                _editReadingListValidator,
                _removeBookFromReadingListValidator,
                _removeReadingListValidator,
                _jwtService 
            );
            var httpContext = new DefaultHttpContext();
            _controller.ControllerContext = new ControllerContext()
            {
                HttpContext = httpContext
            };
        }

        [Fact]
        public async Task Index_ValidQuery_ReturnsOkResult()
        {
            // Arrange
            var query = new BrowseReadingListsQuery();
            var pagination = new PaginationMetadata();
            var readingList = new List<ReadingListDto>();
            SetupPatronContext();
            _mediatorMock.Setup(m => m.Send(query, default))
                .ReturnsAsync((pagination, readingList));
            

            // Act
            var result = await _controller.Index(query) as IActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task Index_InvalidQuery_ReturnsBadRequest()
        {
            // Arrange
            var query = new BrowseReadingListsQuery() { pageSize = 20};
            SetupPatronContext();

            // Act
            var result = await _controller.Index(query) as IActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task AddReadingList_ValidCommand_ReturnsOkResult()
        {
            // Arrange
            var command = new AddReadingListCommand() {
                Name = "name",
                Description = "description",
                Books = new List<IdDto> { new IdDto() {  Id = 1 } },
            };

            _mediatorMock.Setup(m => m.Send(command, default))
                .ReturnsAsync(new OkResult());
            SetupPatronContext();
            // Act
            var result = await _controller.AddReadingList(command) as IActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async Task AddReadingList_InvalidCommand_ReturnsBadRequest()
        {
            // Arrange
            var command = new AddReadingListCommand()
            {
                Description = "description",
                Books = new List<IdDto> { new IdDto() { Id = 1 } },
            };
            SetupPatronContext();

            // Act
            var result = await _controller.AddReadingList(command) as IActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task EditReadingList_ValidCommand_ReturnsOkResult()
        {
            // Arrange
            var command = new EditReadingListCommand()
            {
                Id = 1,
                Name = "name",
                Description = "description",
                Books = new List<IdDto> { new IdDto() { Id = 1 } },
            }; 

            _mediatorMock.Setup(m => m.Send(command, default))
                .ReturnsAsync(new OkResult());
            SetupPatronContext();

            // Act
            var result = await _controller.EditReadingList(command) as IActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async Task EditReadingList_InvalidCommand_ReturnsBadRequest()
        {
            // Arrange
            var command = new EditReadingListCommand();
            SetupPatronContext();
            // Act
            var result = await _controller.EditReadingList(command) as IActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task DeleteReadingList_ValidId_ReturnsOkResult()
        {
            // Arrange
            var id = 1;
            SetupPatronContext();

            _mediatorMock.Setup(m => m.Send(It.IsAny<RemoveReadingListCommand>(), default))
                .ReturnsAsync(new NoContentResult());

            // Act
            var result = await _controller.DeleteReadingList(id) as IActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task DeleteReadingList_InvalidId_ReturnsBadRequest()
        {
            // Arrange
            var id = 0;
            SetupPatronContext();
            // Act
            var result = await _controller.DeleteReadingList(id) as IActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public async Task AddBookToReadingList_ValidCommand_ReturnsOkResult()
        {
            // Arrange
            var command = new AddBookToReadingListCommand() {
                BookId = 1,
                ReadingListId = 1,
            };
            SetupPatronContext();

            _mediatorMock.Setup(m => m.Send(command, default))
                .ReturnsAsync(new OkResult());

            // Act
            var result = await _controller.AddBookToReadingList(command) as IActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async Task AddBookToReadingList_InvalidCommand_ReturnsBadRequest()
        {
            // Arrange
            var command = new AddBookToReadingListCommand();
            SetupPatronContext();
            // Act
            var result = await _controller.AddBookToReadingList(command) as IActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task RemoveBookFromReadingList_ValidCommand_ReturnsOkResult()
        {
            // Arrange
            var command = new RemoveBookFromReadingListCommand() {
                BookId =1,
                ReadingListId=1,
            };
            SetupPatronContext();

            _mediatorMock.Setup(m => m.Send(command, default))
                .ReturnsAsync(new OkResult());

            // Act
            var result = await _controller.RemoveBookFromReadingList(command) as IActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async Task RemoveBookFromReadingList_InvalidCommand_ReturnsBadRequest()
        {
            // Arrange
            var command = new RemoveBookFromReadingListCommand();
            SetupPatronContext();
            // Act
            var result = await _controller.RemoveBookFromReadingList(command) as IActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.IsType<BadRequestObjectResult>(result);
        }

        private void SetupPatronContext()
        {
            var httpContext = new DefaultHttpContext();
            var authorizationHeader = "Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hc" +
                "y54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1laWRlbnRpZmllciI6IjMiLCJodHRwOi8vc2N" +
                "oZW1hcy5taWNyb3NvZnQuY29tL3dzLzIwMDgvMDYvaWRlbnRpdHkvY2xhaW1zL3JvbGUiOiJQYXRyb24iLCJuYmYiOjE2OD" +
                "kxNzExMDUsImV4cCI6MTY4OTE3NDcwNSwiaXNzIjoiaHR0cHM6Ly9sb2NhbGhvc3Q6NzI3OSIsImF1ZCI6IlByZXNlbnRhdGlv" +
                "biJ9.Bc_UUu787Z9DoXGRY0iuHyGUqDwqZmBPjeCMY9-ZNyE";
            httpContext.Request.Headers.Add("Authorization", authorizationHeader);

            _controller.ControllerContext = new ControllerContext()
            {
                HttpContext = httpContext
            };
        }
    }
}
