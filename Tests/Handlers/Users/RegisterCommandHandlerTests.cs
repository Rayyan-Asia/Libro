using System.Threading;
using System.Threading.Tasks;
using Application.DTOs;
using Application.Entities.Users;
using Application.Entities.Users.Commands;
using Application.Entities.Users.Handlers;
using Application.Interfaces;
using Application.Services;
using AutoMapper;
using Domain;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Tests.Handlers.Users
{
    public class RegisterCommandHandlerTests
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<IConfiguration> _configurationMock;
        private readonly Mock<ILogger<RegisterCommandHandler>> _loggerMock;
        private readonly Mock<IJwtService> _jwtServiceMock;
        private readonly Mock<IPasswordHasher> _passwordHasherMock;
        private readonly RegisterCommandHandler _registerCommandHandler;

        public RegisterCommandHandlerTests()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _mapperMock = new Mock<IMapper>();
            _configurationMock = new Mock<IConfiguration>();
            _loggerMock = new Mock<ILogger<RegisterCommandHandler>>();
            _jwtServiceMock = new Mock<IJwtService>();
            _passwordHasherMock = new Mock<IPasswordHasher>();

            _registerCommandHandler = new RegisterCommandHandler(
                _userRepositoryMock.Object,
                _mapperMock.Object,
                _configurationMock.Object,
                _loggerMock.Object,
                _jwtServiceMock.Object,
                _passwordHasherMock.Object
            );
        }

        [Fact]
        public async Task Handle_NewUser_ShouldReturnOkObjectResultWithAuthenticationResponse()
        {
            // Arrange
            var command = new RegisterCommand
            {
                Email = "test@example.com",
                PhoneNumber = "123456789",
                Name = "Test User",
                Password = "password"
            };
            var newUser = new User
            {
                Email = command.Email,
                PhoneNumber = command.PhoneNumber,
                Name = command.Name
            };
            var registeredUser = new User
            {
                Id = 1,
                Email = command.Email,
                PhoneNumber = command.PhoneNumber,
                Name = command.Name
            };
            var registeredUserDto = new UserDto
            {
                Id = 1,
                Email = command.Email,
                PhoneNumber = command.PhoneNumber,
                Name = command.Name
            };
            var jwt = "generated_jwt";
            var authenticationResponse = new AuthenticationResponse
            {
                Jwt = jwt,
                UserDto = registeredUserDto
            };

            _userRepositoryMock.Setup(repo => repo.GetUserByEmailAsync(command.Email)).ReturnsAsync((User)null);
            _passwordHasherMock.Setup(hasher => hasher.GenerateSalt()).Returns("salt");
            _passwordHasherMock.Setup(hasher => hasher.ComputeHash(command.Password, "salt",4)).Returns("hashed_password");
            _userRepositoryMock.Setup(repo => repo.AddUserAsync(newUser)).ReturnsAsync(registeredUser);
            _mapperMock.Setup(mapper => mapper.Map<UserDto>(registeredUser)).Returns(registeredUserDto);
            _jwtServiceMock.Setup(service => service.GenerateJwt(registeredUser, _configurationMock.Object)).Returns(jwt);

            // Act
            var result = await _registerCommandHandler.Handle(command, CancellationToken.None);

            // Assert
            Assert.IsType<OkObjectResult>(result);
            var okResult = (OkObjectResult)result;
            Assert.IsType<AuthenticationResponse>(okResult.Value);
        }

        [Fact]
        public async Task Handle_ExistingUser_ShouldReturnBadRequestObjectResult()
        {
            // Arrange
            var command = new RegisterCommand
            {
                Email = "test@example.com",
                PhoneNumber = "123456789",
                Name = "Test User",
                Password = "password"
            };
            var existingUser = new User { Email = command.Email };

            _userRepositoryMock.Setup(repo => repo.GetUserByEmailAsync(command.Email)).ReturnsAsync(existingUser);

            // Act
            var result = await _registerCommandHandler.Handle(command, CancellationToken.None);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }
    }
}
