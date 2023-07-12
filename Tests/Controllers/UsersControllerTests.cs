using Application.Entities.Profiles.Queries;
using Application.Entities.Users.Commands;
using Application.Entities.Users.Queries;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Presentation.Controllers;
using Presentation.Validators.Profiles;
using Presentation.Validators.Users;

namespace Tests.Controllers
{
    public class UsersControllerTests
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly LoginQueryValidator _loginValidator;
        private readonly ModifyRoleCommandValidator _modifyRoleValidator;
        private readonly RegisterCommandValidator _registerValidator;
        private readonly ViewProfileQueryValidator _viewProfileValidator;

        private readonly UsersController _controller;

        public UsersControllerTests()
        {
            _mediatorMock = new Mock<IMediator>();
            _loginValidator = new LoginQueryValidator();
            _modifyRoleValidator = new ModifyRoleCommandValidator();
            _registerValidator = new RegisterCommandValidator();
            _viewProfileValidator = new ViewProfileQueryValidator();

            _controller = new UsersController(
                _mediatorMock.Object,
                _loginValidator,
                _modifyRoleValidator,
                _registerValidator,
                _viewProfileValidator
            );
        }

        //alter the rest to use the actual validator

        [Fact]
        public async Task Register_ValidCommand_ReturnsOkResult()
        {
            // Arrange
            var userForRegistry = new RegisterCommand()
            {
                Email = "Rayyan@gmail.com",
                Name = "Rayyan",
                PhoneNumber = "1234567890",
                Password = "RayyanIsTheGoat"
            };
            _mediatorMock.Setup(m => m.Send(userForRegistry, default))
                .ReturnsAsync(new OkResult());

            // Act
            var result = await _controller.Register(userForRegistry) as IActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async Task Register_InvalidCommand_ReturnsBadRequest()
        {
            // Arrange
            var userForRegistry = new RegisterCommand()
            {
                Name = "Rayyyyyyyyyyyyyyoooooooooooooooooooonnnnnnnnnnnnnnnnnnnnnn",
                Email = "rayyyyyyyyyyyooooooooooooooooooooooooooooooooooooonnn",
                PhoneNumber = "rayyyyyyyyyyyyyyyyyyyyyyyooooooooooooooooooon",
                Password = "Rayyyyooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooon"
            };

            // Act
            var result = await _controller.Register(userForRegistry) as IActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.IsType<BadRequestObjectResult>(result);
        }


        [Fact]
        public async Task Login_ValidQuery_ReturnsOkResult()
        {
            // Arrange
            var loginQuery = new LoginQuery()
            {
                Email = "Ray23fast@gmail.com",
                Password = "Rayyan2001!"
            };


            _mediatorMock.Setup(m => m.Send(loginQuery, default))
                .ReturnsAsync(new OkResult());

            // Act
            var result = await _controller.Login(loginQuery) as IActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async Task Login_InvalidQuery_ReturnsBadRequest()
        {
            // Arrange
            var loginQuery = new LoginQuery()
            {
                Email = "Rayyyyyyyyyyyyyyyyyyyyooooooooooooooonnnnnnnnnnnnn",
                Password = "hello"
            };
            // Act
            var result = await _controller.Login(loginQuery) as IActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task ModifyRole_ValidCommand_ReturnsOkResult()
        {
            // Arrange
            var modifyRoleCommand = new ModifyRoleCommand()
            {
                UserId = 2,
                Role = Domain.Role.Administrator
            };
            _mediatorMock.Setup(m => m.Send(modifyRoleCommand, default))
                .ReturnsAsync(new OkResult());

            // Create and set up the HttpContext
            var httpContext = new DefaultHttpContext();
            var authorizationHeader = "Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1laWRlbnRpZmllciI6IjEiLCJodHRwOi8vc2NoZW1hcy5taWNyb3NvZnQuY29tL3dzLzIwMDgvMDYvaWRlbnRpdHkvY2xhaW1zL3JvbGUiOiJBZG1pbmlzdHJhdG9yIiwibmJmIjoxNjg5MTU4MTcwLCJleHAiOjE2ODkxNjE3NzAsImlzcyI6Imh0dHBzOi8vbG9jYWxob3N0OjcyNzkiLCJhdWQiOiJQcmVzZW50YXRpb24ifQ.T0WmsT30B3yUvSDB31yXLMTHCPqnoyClFQ_6aqQHTB0";
            httpContext.Request.Headers.Add("Authorization", authorizationHeader);

            _controller.ControllerContext = new ControllerContext()
            {
                HttpContext = httpContext
            };
            // Act
            var result = await _controller.ModifyRole(modifyRoleCommand) as IActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async Task ModifyRole_InvalidCommand_ReturnsBadRequest()
        {
            // Arrange
            var modifyRoleCommand = new ModifyRoleCommand()
            {
                UserId = 1,
                Role = Domain.Role.Administrator
            };
            var httpContext = new DefaultHttpContext();

            var authorizationHeader = "Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1laWRlbnRpZmllciI6IjEiLCJodHRwOi8vc2NoZW1hcy5taWNyb3NvZnQuY29tL3dzLzIwMDgvMDYvaWRlbnRpdHkvY2xhaW1zL3JvbGUiOiJBZG1pbmlzdHJhdG9yIiwibmJmIjoxNjg5MTU4MTcwLCJleHAiOjE2ODkxNjE3NzAsImlzcyI6Imh0dHBzOi8vbG9jYWxob3N0OjcyNzkiLCJhdWQiOiJQcmVzZW50YXRpb24ifQ.T0WmsT30B3yUvSDB31yXLMTHCPqnoyClFQ_6aqQHTB0";
            httpContext.Request.Headers.Add("Authorization", authorizationHeader);

            _controller.ControllerContext = new ControllerContext()
            {
                HttpContext = httpContext
            };

            // Act
            var result = await _controller.ModifyRole(modifyRoleCommand) as IActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.IsType<UnauthorizedResult>(result);
        }


        [Fact]
        public async Task ViewProfile_ValidQuery_ReturnsOkResult()
        {
            // Arrange
            var userId = 123;
            var viewProfileQuery = new ViewProfileQuery() { PatronId = userId };

            _mediatorMock.Setup(m => m.Send(It.IsAny<ViewProfileQuery>(), default))
                .ReturnsAsync(new OkResult());

            // Act
            var result = await _controller.ViewProfile(userId) as IActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async Task ViewProfile_InvalidQuery_ReturnsBadRequest()
        {
            // Arrange
            var userId = -1; // Invalid user ID
            var viewProfileQuery = new ViewProfileQuery() { PatronId = userId };

            // Act
            var result = await _controller.ViewProfile(userId) as IActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.IsType<BadRequestResult>(result);
        }



    }
}


