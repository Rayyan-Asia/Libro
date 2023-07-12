using System.Threading.Tasks;
using Application;
using Application.DTOs;
using Application.Entities.Feedbacks.Queries;
using Application.Entities.Loans.Queries;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Presentation.Controllers;
using Presentation.Validators.Loans;
using Xunit;

namespace Tests.Controllers
{
    public class LoansControllerTests
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly GetAllActiveLoansQueryValidator _activeLoansValidator;
        private readonly GetOverdueLoanQueryValidator _overdueLoanValidator;
        private readonly ListOverdueLoansQueryValidator _overdueLoansValidator;
        private readonly GetUserOverdueLoansQueryValidator _userOverdueLoansValidator;
        private readonly LoansController _controller;

        public LoansControllerTests()
        {
            _mediatorMock = new Mock<IMediator>();
            _activeLoansValidator = new GetAllActiveLoansQueryValidator();
            _overdueLoanValidator = new GetOverdueLoanQueryValidator();
            _overdueLoansValidator = new ListOverdueLoansQueryValidator();
            _userOverdueLoansValidator = new GetUserOverdueLoansQueryValidator();

            _controller = new LoansController(
                _activeLoansValidator,
                _overdueLoanValidator,
                _overdueLoansValidator,
                _userOverdueLoansValidator,
                _mediatorMock.Object
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
            var query = new GetAllActiveLoansQuery();
            _mediatorMock.Setup(m => m.Send(query, default))
                .ReturnsAsync((new PaginationMetadata(), new List<LoanDto>()));

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
            var query = new GetAllActiveLoansQuery() { PageSize = 200};

            // Act
            var result = await _controller.Index(query) as IActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task GetOverdue_ValidQuery_ReturnsOkResult()
        {
            // Arrange
            var query = new ListOverdueLoansQuery();
            _mediatorMock.Setup(m => m.Send(query, default))
                .ReturnsAsync((new PaginationMetadata(), new List<LoanDto>()));

            // Act
            var result = await _controller.GetOverdue(query) as IActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task GetOverdue_InvalidQuery_ReturnsBadRequest()
        {
            // Arrange
            var query = new ListOverdueLoansQuery() { PageSize = 200};

            // Act
            var result = await _controller.GetOverdue(query) as IActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task GetOverdueByUser_ValidQuery_ReturnsOkResult()
        {
            // Arrange
            var query = new GetUserOverdueLoansQuery() { UserId = 100};
            _mediatorMock.Setup(m => m.Send(query, default))
                .ReturnsAsync((new PaginationMetadata(), new List<OverdueLoanDto>()));

            // Act
            var result = await _controller.GetOverdueByUser(query) as IActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task GetOverdueByUser_InvalidQuery_ReturnsBadRequest()
        {
            // Arrange
            var query = new GetUserOverdueLoansQuery();

            // Act
            var result = await _controller.GetOverdueByUser(query) as IActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task GetOverdueLoan_ValidQuery_ReturnsOkResult()
        {
            // Arrange
            var query = new GetOverdueLoanQuery() { Id =1 };
            _mediatorMock.Setup(m => m.Send(query, default))
                .ReturnsAsync(new OkResult());

            // Act
            var result = await _controller.GetOverdueLoan(query) as IActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async Task GetOverdueLoan_InvalidQuery_ReturnsBadRequest()
        {
            // Arrange
            var query = new GetOverdueLoanQuery();

            // Act
            var result = await _controller.GetOverdueLoan(query) as IActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.IsType<BadRequestObjectResult>(result);
        }
    }
}
