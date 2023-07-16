using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application;
using Application.DTOs;
using Application.Entities.Authors.Handlers;
using Application.Entities.Authors.Queries;
using Application.Interfaces;
using AutoMapper;
using Domain;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Tests.Handlers.Authors
{
    public class BrowseAuthorsQueryHandlerTests
    {
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<IAuthorRepository> _repositoryMock;
        private readonly Mock<ILogger<BrowseAuthorsQueryHandler>> _loggerMock;
        private readonly BrowseAuthorsQueryHandler _handler;

        public BrowseAuthorsQueryHandlerTests()
        {
            _mapperMock = new Mock<IMapper>();
            _repositoryMock = new Mock<IAuthorRepository>();
            _loggerMock = new Mock<ILogger<BrowseAuthorsQueryHandler>>();
            _handler = new BrowseAuthorsQueryHandler(
                _mapperMock.Object,
                _repositoryMock.Object,
                _loggerMock.Object
            );
        }

        [Fact]
        public async Task Handle_ValidQuery_ReturnsPaginationMetadataAndAuthorDtoList()
        {
            // Arrange
            var pageNumber = 1;
            var pageSize = 10;
            var query = new BrowseAuthorsQuery { pageNumber = pageNumber, pageSize = pageSize };
            var cancellationToken = new CancellationToken();
            var paginationMetadata = new PaginationMetadata { ItemCount = 2, PageSize = pageSize, PageCount = pageNumber};
            var authors = new List<Author> { new Author { Id = 1, Name = "John Doe" }, new Author { Id = 2, Name = "Jane Smith" } };
            var authorDtos = new List<AuthorDto> { new AuthorDto { Id = 1, Name = "John Doe" }, new AuthorDto { Id = 2, Name = "Jane Smith" } };

            _repositoryMock.Setup(m => m.GetAuthorsAsync(pageNumber, pageSize)).ReturnsAsync((paginationMetadata, authors));
            _mapperMock.Setup(m => m.Map<List<AuthorDto>>(authors)).Returns(authorDtos);

            // Act
            var result = await _handler.Handle(query, cancellationToken);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(paginationMetadata, result.Item1);
            Assert.Equal(authorDtos, result.Item2);
        }
    }
}
