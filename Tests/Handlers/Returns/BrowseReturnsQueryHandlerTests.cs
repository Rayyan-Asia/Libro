using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.DTOs;
using Application.Entities.Returns.Handlers;
using Application.Entities.Returns.Queries;
using Application.Interfaces;
using AutoMapper;
using Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Application.UnitTests.Entities.Returns.Handlers
{
    public class BrowseReturnsQueryHandlerTests
    {
        private readonly Mock<IBookReturnRepository> _repositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<ILogger<BrowseReturnsQueryHandler>> _loggerMock;
        private readonly BrowseReturnsQueryHandler _handler;

        public BrowseReturnsQueryHandlerTests()
        {
            _repositoryMock = new Mock<IBookReturnRepository>();
            _mapperMock = new Mock<IMapper>();
            _loggerMock = new Mock<ILogger<BrowseReturnsQueryHandler>>();
            _handler = new BrowseReturnsQueryHandler(
                _repositoryMock.Object,
                _mapperMock.Object,
                _loggerMock.Object);
        }

        [Fact]
        public async Task Handle_ValidRequest_ReturnsPaginationMetadataAndList()
        {
            // Arrange
            var query = new BrowseReturnsQuery
            {
                pageNumber = 1,
                pageSize = 10
            };

            var expectedPaginationMetadata = new PaginationMetadata
            {
                ItemCount = 100,
                PageSize = 10,
                PageCount = 1,
            };

            var expectedReturnsDtos = new List<BookReturnDto>
            {
                new BookReturnDto { Id = 1, LoanId = 1 },
                new BookReturnDto { Id = 2, LoanId = 2 }
            };
            var expectedReturns = new List<BookReturn>
            {
                new BookReturn { Id = 1, LoanId = 1 },
                new BookReturn { Id = 2, LoanId = 2 }
            };

            _repositoryMock.Setup(r => r.GetAllReturnsAsync(query.pageNumber, query.pageSize))
                .ReturnsAsync((expectedPaginationMetadata, expectedReturns));
            _mapperMock.Setup(r => r.Map<List<BookReturnDto>>(expectedReturns)).Returns(expectedReturnsDtos);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.IsType<(PaginationMetadata, List<BookReturnDto>)>(result);
            Assert.Equal(expectedPaginationMetadata, result.Item1);
            Assert.Equal(expectedReturnsDtos, result.Item2);
        }
    }
}
