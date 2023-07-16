using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application;
using Application.DTOs;
using Application.Entities.Books.Handlers;
using Application.Entities.Books.Queries;
using Application.Interfaces;
using AutoMapper;
using Domain;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Tests.Handlers.Books
{
    public class BrowseBooksQueryHandlerTests
    {
        private readonly Mock<IBookRepository> _repositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<ILogger<BrowseBooksQueryHandler>> _loggerMock;

        private readonly BrowseBooksQueryHandler _handler;

        public BrowseBooksQueryHandlerTests()
        {
            _repositoryMock = new Mock<IBookRepository>();
            _mapperMock = new Mock<IMapper>();
            _loggerMock = new Mock<ILogger<BrowseBooksQueryHandler>>();

            _handler = new BrowseBooksQueryHandler(
                _repositoryMock.Object,
                _mapperMock.Object,
                _loggerMock.Object
            );
        }

        [Fact]
        public async Task Handle_WithValidQuery_ReturnsPaginationMetadataAndBookList()
        {
            // Arrange
            var query = new BrowseBooksQuery { pageNumber = 1, pageSize = 10 };
            var paginationMetadata = new PaginationMetadata();
            var books = new List<Book>(); // create a list of book instances
            var bookDtoList = new List<BookDto>(); // create a list of book DTO instances

            _repositoryMock.Setup(r => r.GetBooksAsync(query.pageNumber, query.pageSize)).ReturnsAsync((paginationMetadata, books));
            _mapperMock.Setup(m => m.Map<List<BookDto>>(books)).Returns(bookDtoList);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.Equal((paginationMetadata, bookDtoList), result);
        }
    }
}
