using System;
using System.Threading.Tasks;
using Application;
using Application.DTOs;
using Application.Entities.Feedbacks.Commands;
using Application.Entities.Feedbacks.Queries;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Presentation.Controllers;
using Presentation.Validators.Feedbacks;
using Xunit;

namespace Tests.Controllers
{
    public class FeedbacksControllerTests
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly BrowseBookFeedbackQueryValidator _browseBookFeedbackValidator;
        private readonly AddFeedbackCommandValidator _addFeedbackValidator;
        private readonly BrowseUserFeedbackQueryValidator _browseUserFeedbackValidator;
        private readonly EditFeedbackCommandValidator _editFeedbackValidator;
        private readonly RemoveFeedbackCommandValidator _removeFeedbackValidator;
        private readonly FeedbacksController _controller;

        public FeedbacksControllerTests()
        {
            _mediatorMock = new Mock<IMediator>();
            _browseBookFeedbackValidator = new BrowseBookFeedbackQueryValidator();
            _addFeedbackValidator = new AddFeedbackCommandValidator();
            _browseUserFeedbackValidator = new BrowseUserFeedbackQueryValidator();
            _editFeedbackValidator = new EditFeedbackCommandValidator();
            _removeFeedbackValidator = new RemoveFeedbackCommandValidator();
            
            _controller = new FeedbacksController(
                _mediatorMock.Object,
                _browseBookFeedbackValidator,
                _addFeedbackValidator,
                _browseUserFeedbackValidator,
                _editFeedbackValidator,
                _removeFeedbackValidator
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
            var browseBookFeedbackQuery = new BrowseBookFeedbackQuery() { 
                BookId = 1,
            };
            _mediatorMock.Setup(m => m.Send(browseBookFeedbackQuery, default))
                .ReturnsAsync((new PaginationMetadata(), new List<FeedbackDto>()));

            // Act
            var result = await _controller.Index(browseBookFeedbackQuery) as IActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task Index_InvalidQuery_ReturnsBadRequest()
        {
            // Arrange
            var browseBookFeedbackQuery = new BrowseBookFeedbackQuery();

            // Act
            var result = await _controller.Index(browseBookFeedbackQuery) as IActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public async Task ListFeedbacksByUser_ValidQuery_ReturnsOkResult()
        {
            // Arrange
            var browseUserFeedbackQuery = new BrowseUserFeedbackQuery() { 
                UserId = 1,
            };
            _mediatorMock.Setup(m => m.Send(browseUserFeedbackQuery, default))
                .ReturnsAsync((new PaginationMetadata(), new List<FeedbackDto>()));

            // Act
            var result = await _controller.ListFeedbacksByUser(browseUserFeedbackQuery) as IActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task ListFeedbacksByUser_InvalidQuery_ReturnsBadRequest()
        {
            // Arrange
            var browseUserFeedbackQuery = new BrowseUserFeedbackQuery();
            _mediatorMock.Setup(M => M.Send(It.IsAny<BrowseUserFeedbackQuery>(), default))
                .ReturnsAsync((new PaginationMetadata(), new List<FeedbackDto>()));
            
            // Act
            var result = await _controller.ListFeedbacksByUser(browseUserFeedbackQuery) as IActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public async Task AddFeedbackAsync_ValidCommand_ReturnsOkResult()
        {
            // Arrange
            var addFeedbackCommand = new AddFeedbackCommand() {
                BookId = 1,
                UserId = 1,
                CreatedDate = DateTime.Now,
                Rating = Domain.Rating.Good,
                Review = "OK"
            };
            _mediatorMock.Setup(m => m.Send(addFeedbackCommand, default))
                .ReturnsAsync(new OkResult());
            SetupPatronContext();
           
            // Act
            var result = await _controller.AddFeedbackAsync(addFeedbackCommand) as IActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async Task AddFeedbackAsync_InvalidCommand_ReturnsBadRequest()
        {
            // Arrange
            var addFeedbackCommand = new AddFeedbackCommand();
            SetupPatronContext();
            // Act
            var result = await _controller.AddFeedbackAsync(addFeedbackCommand) as IActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task EditFeedbackAsync_ValidCommand_ReturnsOkResult()
        {
            // Arrange
            var editFeedbackCommand = new EditFeedbackCommand()
            {
                Id = 1,
                BookId = 1,
                UserId = 1,
                CreatedDate = DateTime.Now,
                Rating = Domain.Rating.Good,
                Review = "OK"
            };
            _mediatorMock.Setup(m => m.Send(editFeedbackCommand, default))
                .ReturnsAsync(new OkResult());
            SetupPatronContext();
            // Act
            var result = await _controller.EditFeedbackAsync(editFeedbackCommand) as IActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async Task EditFeedbackAsync_InvalidCommand_ReturnsBadRequest()
        {
            // Arrange
            var editFeedbackCommand = new EditFeedbackCommand();
            SetupPatronContext();
            // Act
            var result = await _controller.EditFeedbackAsync(editFeedbackCommand) as IActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task RemoveFeedbackAsync_ValidId_ReturnsOkResult()
        {
            // Arrange
            var id = 1;
            _mediatorMock.Setup(m => m.Send(It.IsAny<RemoveFeedbackCommand>(), default))
                .ReturnsAsync(new OkResult());
            SetupPatronContext();

            // Act
            var result = await _controller.RemoveFeedbackAsync(id) as IActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async Task RemoveFeedbackAsync_InvalidId_ReturnsBadRequest()
        {
            // Arrange
            var id = 0;
            SetupPatronContext();
            // Act
            var result = await _controller.RemoveFeedbackAsync(id) as IActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.IsType<BadRequestResult>(result);
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