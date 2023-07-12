using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Application;
using Application.DTOs;
using Application.Entities.Returns.Commands;
using Application.Entities.Returns.Queries;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Presentation.Controllers;
using Presentation.Validators.Returns;
using Xunit;

namespace Tests.Controllers
{
    public class ReturnsControllerTests
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly ApproveBookReturnCommandValidator _approveValidator;
        private readonly BookReturnCommandValidator _bookReturnValidator;
        private readonly ReturnsController _controller;

        public ReturnsControllerTests()
        {
            _mediatorMock = new Mock<IMediator>();
            _approveValidator = new ApproveBookReturnCommandValidator();
            _bookReturnValidator = new BookReturnCommandValidator();

            _controller = new ReturnsController(
                _mediatorMock.Object,
                _approveValidator,
                _bookReturnValidator
            );
        }

        [Fact]
        public async Task Index_DefaultQuery_ReturnsOkResult()
        {
            // Arrange
            var browseReturnsQuery = new BrowseReturnsQuery();
            var pagination = new PaginationMetadata();
            var returns = new List<BookReturnDto>();

            var httpContext = new DefaultHttpContext();
            _controller.ControllerContext = new ControllerContext()
            {
                HttpContext = httpContext
            };


            _mediatorMock.Setup(m => m.Send(browseReturnsQuery,default))
                .ReturnsAsync((pagination, returns));

            // Act
            var result = await _controller.Index(browseReturnsQuery) as IActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task ReturnBook_ValidLoanId_ReturnsOkResult()
        {
            // Arrange

            _mediatorMock.Setup(m => m.Send(It.IsAny<BookReturnCommand>(), default))
                .ReturnsAsync(new OkResult());

            // Create and set up the HttpContext
            PatronContextSetup();

            // Act
            var result = await _controller.ReturnBook(3) as IActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.IsType<OkResult>(result);
        }

        

        [Fact]
        public async Task ReturnBook_InvalidLoanId_ReturnsBadRequest()
        {
            // Arrange
            var loanId = 0;

            PatronContextSetup();
            // Act
            var result = await _controller.ReturnBook(loanId) as IActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public async Task ReturnBook_InvalidUser_ReturnsBadRequest()
        {
            // Arrange
            var loanId = 123;
            var httpContext = new DefaultHttpContext();

            var authorizationHeader = "Bearer invalid-token";
            httpContext.Request.Headers.Add("Authorization", authorizationHeader);

            _controller.ControllerContext = new ControllerContext()
            {
                HttpContext = httpContext
            };

            // Act
            var result = await _controller.ReturnBook(loanId) as IActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.IsType<UnauthorizedResult>(result);
        }

        [Fact]
        public async Task ApproveReturnBook_ValidBookReturnId_ReturnsOkResult()
        {
            // Arrange
            var bookReturnId = 123;
            _mediatorMock.Setup(m => m.Send(It.IsAny<ApproveBookReturnCommand>(), default))
                .ReturnsAsync(new OkResult());

            // Act
            var result = await _controller.ApproveReturnBook(bookReturnId) as IActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async Task ApproveReturnBook_InvalidBookReturnId_ReturnsBadRequest()
        {
            // Arrange
            var bookReturnId = 0;

            // Act
            var result = await _controller.ApproveReturnBook(bookReturnId) as IActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.IsType<BadRequestResult>(result);
        }

        private void PatronContextSetup()
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
