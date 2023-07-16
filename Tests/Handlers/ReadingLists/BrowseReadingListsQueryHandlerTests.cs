using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application;
using Application.DTOs;
using Application.Entities.ReadingLists.Handlers;
using Application.Entities.ReadingLists.Queries;
using Application.Interfaces;
using AutoMapper;
using Domain;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Tests.Handlers.ReadingLists
{
    public class BrowseReadingListsQueryHandlerTests
    {
        private readonly Mock<IReadingListRepository> _readingListRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<ILogger<BrowseReadingListsQueryHandler>> _loggerMock;

        private readonly BrowseReadingListsQueryHandler _handler;

        public BrowseReadingListsQueryHandlerTests()
        {
            _readingListRepositoryMock = new Mock<IReadingListRepository>();
            _mapperMock = new Mock<IMapper>();
            _loggerMock = new Mock<ILogger<BrowseReadingListsQueryHandler>>();

            _handler = new BrowseReadingListsQueryHandler(
                _readingListRepositoryMock.Object,
                _mapperMock.Object,
                _loggerMock.Object);
        }

        [Fact]
        public async Task Handle_ValidRequest_ReturnsReadingLists()
        {
            // Arrange
            var query = new BrowseReadingListsQuery
            {
                UserId = 1,
                pageNumber = 1,
                pageSize = 10
            };

            var expectedReadingLists = new List<ReadingList>
            {
                new ReadingList { Id = 1, Name = "Reading List 1" },
                new ReadingList { Id = 2, Name = "Reading List 2" }
            };


            var expectedReadingDtoLists = new List<ReadingListDto>
            {
                new ReadingListDto { Id = 1, Name = "Reading List 1" },
                new ReadingListDto { Id = 2, Name = "Reading List 2" }
            };

            var paginationMetadata = new PaginationMetadata
            {
                PageCount = 1,
                PageSize = 10,
                ItemCount = 2,
            };

            _readingListRepositoryMock.Setup(r => r.GetReadingListsAsync(query.pageNumber, query.pageSize, query.UserId))
                .ReturnsAsync((paginationMetadata, expectedReadingLists));

            _mapperMock.Setup(m => m.Map<List<ReadingListDto>>(expectedReadingLists))
                .Returns(expectedReadingDtoLists);

            // Act
            var (actualPaginationMetadata, actualReadingLists) = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.Equal(paginationMetadata, actualPaginationMetadata);
            Assert.Equal(expectedReadingDtoLists, actualReadingLists);
        }
    }
}
