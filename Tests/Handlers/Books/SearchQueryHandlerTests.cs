using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.DTOs;
using Application.Entities.Books.Queries;
using Application.Entities.Books.Handlers;
using Application.Interfaces;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;
using Domain;
using Application;

namespace Tests.Handlers.Books
{
    public class SearchQueryHandlerTests
    {
        private readonly Mock<IBookRepository> _repositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<ILogger<SearchQueryHandler>> _loggerMock;

        private readonly SearchQueryHandler _handler;

        public SearchQueryHandlerTests()
        {
            _repositoryMock = new Mock<IBookRepository>();
            _mapperMock = new Mock<IMapper>();
            _loggerMock = new Mock<ILogger<SearchQueryHandler>>();

            _handler = new SearchQueryHandler(
                _repositoryMock.Object,
                _mapperMock.Object,
                _loggerMock.Object
            );
        }

        [Fact]
        public async Task Handle_WithValidSearchQuery_ReturnsPaginationMetadataAndBookList()
        {
            // Arrange
            var query = new SearchQuery
            {
                Title = "Test",
                Author = "John Doe",
                Genre = "Fantasy",
                pageNumber = 1,
                pageSize = 10
            };

            var books = new List<Book> { new Book { Id = 1, Title = "Test Book" } };
            var bookDtos = new List<BookDto> { new BookDto { Id = 1, Title = "Test Book" } };
            var paginationMetadata = new PaginationMetadata
            {
                PageCount = 1,
                PageSize = 10,
                ItemCount = 1,
            };

            _repositoryMock.Setup(r => r.SearchBooksAsync(
                query.Title,
                query.Author,
                query.Genre,
                query.pageNumber,
                query.pageSize
            )).ReturnsAsync((paginationMetadata, books));

            _mapperMock.Setup(m => m.Map<List<BookDto>>(books)).Returns(bookDtos);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.Equal(paginationMetadata, result.Item1);
            Assert.Equal(bookDtos, result.Item2);
        }
    }
}
