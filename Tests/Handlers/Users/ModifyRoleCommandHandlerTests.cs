using Application.DTOs;
using Application.Entities.Users.Commands;
using Application.Interfaces;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using Domain;
using Application.Entities.Users.Handlers;

namespace Tests.Handlers.Users
{
    public class ModifyRoleCommandHandlerTests
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<ILogger<ModifyRoleCommandHandler>> _loggerMock;
        private readonly ModifyRoleCommandHandler _modifyRoleCommandHandler;

        public ModifyRoleCommandHandlerTests()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _mapperMock = new Mock<IMapper>();
            _loggerMock = new Mock<ILogger<ModifyRoleCommandHandler>>();

            _modifyRoleCommandHandler = new ModifyRoleCommandHandler(
                _userRepositoryMock.Object,
                _mapperMock.Object,
                _loggerMock.Object
            );
        }

        [Fact]
        public async Task Handle_ExistingUser_ShouldReturnOkObjectResultWithModifiedUserDto()
        {
            // Arrange
            var command = new ModifyRoleCommand { UserId = 1, Role = Role.Administrator };
            var user = new User { Id = 1, Role = Role.Patron };
            var modifiedUser = new User { Id = 1, Role = Role.Administrator };
            var modifiedUserDto = new UserDto { Id = 1};

            _userRepositoryMock.Setup(repo => repo.GetUserByIdAsync(command.UserId)).ReturnsAsync(user);
            _userRepositoryMock.Setup(repo => repo.UpdateUserAsync(It.IsAny<User>())).ReturnsAsync(modifiedUser);
            _mapperMock.Setup(mapper => mapper.Map<UserDto>(It.IsAny<User>())).Returns(modifiedUserDto);

            // Act
            var result = await _modifyRoleCommandHandler.Handle(command, CancellationToken.None);

            // Assert
            Assert.IsType<OkObjectResult>(result);
            var okResult = (OkObjectResult)result;
            Assert.Equal(okResult.Value,modifiedUserDto);
        }

        [Fact]
        public async Task Handle_NonExistingUser_ShouldReturnNotFoundObjectResult()
        {
            // Arrange
            var command = new ModifyRoleCommand { UserId = 1, Role = Role.Administrator };

            _userRepositoryMock.Setup(repo => repo.GetUserByIdAsync(command.UserId)).ReturnsAsync((User)null);

            // Act
            var result = await _modifyRoleCommandHandler.Handle(command, CancellationToken.None);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }
    }
}
