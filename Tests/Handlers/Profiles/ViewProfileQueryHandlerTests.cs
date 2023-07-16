using System.Threading;
using System.Threading.Tasks;
using Application.DTOs;
using Application.Entities.Profiles.Handlers;
using Application.Entities.Profiles.Queries;
using Application.Interfaces;
using AutoMapper;
using Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Tests.Handlers.Profiles
{
    public class ViewProfileQueryHandlerTests
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<ILogger<ViewProfileQueryHandler>> _loggerMock;
        private readonly ViewProfileQueryHandler _handler;

        public ViewProfileQueryHandlerTests()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _mapperMock = new Mock<IMapper>();
            _loggerMock = new Mock<ILogger<ViewProfileQueryHandler>>();

            _handler = new ViewProfileQueryHandler(
                _userRepositoryMock.Object,
                _mapperMock.Object,
                _loggerMock.Object
            );
        }

        [Fact]
        public async Task Handle_WithValidRequest_ReturnsUserProfile()
        {
            // Arrange
            var query = new ViewProfileQuery
            {
                PatronId = 1
            };

            var user = new User
            {
                Id = 1,
                Role = Role.Patron
            };

            var profileDto = new ProfileDto
            {
                Id = user.Id,
                Role = user.Role
            };

            _userRepositoryMock.Setup(r => r.GetUserProfileByIdAsync(query.PatronId))
                .ReturnsAsync(user);

            _mapperMock.Setup(m => m.Map<ProfileDto>(user))
                .Returns(profileDto);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            var expectedResponse = new OkObjectResult(profileDto);
            Assert.Equal(expectedResponse.StatusCode, (result as OkObjectResult)?.StatusCode);
            Assert.Equal(expectedResponse.Value, (result as OkObjectResult)?.Value);
        }

        [Fact]
        public async Task Handle_WithNonExistentUser_ReturnsNotFound()
        {
            // Arrange
            var query = new ViewProfileQuery
            {
                PatronId = 1
            };

            _userRepositoryMock.Setup(r => r.GetUserProfileByIdAsync(query.PatronId))
                .ReturnsAsync((User)null);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            var expectedResponse = new NotFoundObjectResult($"User NOT FOUND with ID {query.PatronId}");
            Assert.Equal(expectedResponse.StatusCode, (result as NotFoundObjectResult)?.StatusCode);
            Assert.Equal(expectedResponse.Value, (result as NotFoundObjectResult)?.Value);
        }

        [Fact]
        public async Task Handle_WithNonPatronUser_ReturnsUnauthorized()
        {
            // Arrange
            var query = new ViewProfileQuery
            {
                PatronId = 1
            };

            var user = new User
            {
                Id = 1,
                Role = Role.Administrator
            };

            _userRepositoryMock.Setup(r => r.GetUserProfileByIdAsync(query.PatronId))
                .ReturnsAsync(user);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            var expectedResponse = new UnauthorizedObjectResult($"User IS NOT A PATRON with ID {query.PatronId}");
            Assert.Equal(expectedResponse.StatusCode, (result as UnauthorizedObjectResult)?.StatusCode);
            Assert.Equal(expectedResponse.Value, (result as UnauthorizedObjectResult)?.Value);
        }
    }
}
