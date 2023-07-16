using AutoMapper;
using MediatR;
using Application.DTOs;
using Application.Entities.Users.Queries;
using Application.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;
using Application.Services;
using Moq;
using Xunit;
using Domain;
using Application.Entities.Users.Handlers;
using Application.Entities.Users;

namespace Tests.Handlers.Users
{
    public class LoginQueryHandlerTests
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<IConfiguration> _configurationMock;
        private readonly Mock<ILogger<LoginQueryHandler>> _loggerMock;
        private readonly Mock<IPasswordHasher> _passwordHasherMock;
        private readonly Mock<IJwtService> _jwtServiceMock;
        private readonly LoginQueryHandler _loginQueryHandler;

        public LoginQueryHandlerTests()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _mapperMock = new Mock<IMapper>();
            _configurationMock = new Mock<IConfiguration>();
            _loggerMock = new Mock<ILogger<LoginQueryHandler>>();
            _passwordHasherMock = new Mock<IPasswordHasher>();
            _jwtServiceMock = new Mock<IJwtService>();

            _loginQueryHandler = new LoginQueryHandler(
                _userRepositoryMock.Object,
                _mapperMock.Object,
                _configurationMock.Object,
                _loggerMock.Object,
                _passwordHasherMock.Object,
                _jwtServiceMock.Object
            );
        }

        [Fact]
        public async Task Handle_ExistingUserAndPasswordMatches_ShouldReturnOkObjectResultWithAuthenticationResponse()
        {
            // Arrange
            var query = new LoginQuery { Email = "test@example.com", Password = "password" };
            var user = new User { Email = "test@example.com", Salt = "salt", HashedPassword = "hashed_password" };
            var jwt = "generated_jwt";
            var userDto = new UserDto { Email = "test@example.com" };
            var authenticationResponse = new AuthenticationResponse { UserDto = userDto, Jwt = jwt };

            _userRepositoryMock.Setup(repo => repo.GetUserByEmailAsync(query.Email)).ReturnsAsync(user);
            _passwordHasherMock.Setup(hasher => hasher.VerifyPassword(query.Password, user.Salt, user.HashedPassword,4)).Returns(true);
            _jwtServiceMock.Setup(service => service.GenerateJwt(user, _configurationMock.Object)).Returns(jwt);
            _mapperMock.Setup(mapper => mapper.Map<UserDto>(user)).Returns(userDto);

            // Act
            var result = await _loginQueryHandler.Handle(query, CancellationToken.None);

            // Assert
            Assert.IsType<OkObjectResult>(result);
            var okResult = (OkObjectResult)result;
        }

        [Fact]
        public async Task Handle_ExistingUserAndPasswordDoesNotMatch_ShouldReturnBadRequestObjectResult()
        {
            // Arrange
            var query = new LoginQuery { Email = "test@example.com", Password = "password" };
            var user = new User { Email = "test@example.com", Salt = "salt", HashedPassword = "hashed_password" };

            _userRepositoryMock.Setup(repo => repo.GetUserByEmailAsync(query.Email)).ReturnsAsync(user);
            _passwordHasherMock.Setup(hasher => hasher.VerifyPassword(query.Password, user.Salt, user.HashedPassword, 4)).Returns(false);

            // Act
            var result = await _loginQueryHandler.Handle(query, CancellationToken.None);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task Handle_NonExistingUser_ShouldReturnNotFoundObjectResult()
        {
            // Arrange
            var query = new LoginQuery { Email = "nonexistent@example.com", Password = "password" };

            _userRepositoryMock.Setup(repo => repo.GetUserByEmailAsync(query.Email)).ReturnsAsync((User)null);

            // Act
            var result = await _loginQueryHandler.Handle(query, CancellationToken.None);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }
    }
}
