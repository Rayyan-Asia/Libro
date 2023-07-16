using System.Threading;
using System.Threading.Tasks;
using Application;
using Application.DTOs;
using Application.Entities.Loans.Handlers;
using Application.Entities.Loans.Queries;
using Application.Interfaces;
using AutoMapper;
using Domain;
using MediatR;
using Moq;
using Xunit;

namespace Tests.Handlers.Loans
{
    public class GetAllActiveLoansQueryHandlerTests
    {
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<ILoanRepository> _loanRepositoryMock;

        private readonly GetAllActiveLoansQueryHandler _handler;

        public GetAllActiveLoansQueryHandlerTests()
        {
            _mapperMock = new Mock<IMapper>();
            _loanRepositoryMock = new Mock<ILoanRepository>();

            _handler = new GetAllActiveLoansQueryHandler(
                _mapperMock.Object,
                _loanRepositoryMock.Object
            );
        }

        [Fact]
        public async Task Handle_WithValidRequest_ReturnsLoans()
        {
            // Arrange
            var query = new GetAllActiveLoansQuery
            {
                PageNumber = 1,
                PageSize = 10
            };

            var loans = new List<Loan>
            {
                new Loan { Id = 1, ReturnDate = null},
                new Loan { Id = 2, ReturnDate = null }
            };

            var loanDtos = new List<LoanDto>
            {
                new LoanDto { Id = 1 },
                new LoanDto { Id = 2 }
            };

            var paginationMetadata = new PaginationMetadata
            {
                PageCount = 1,
                PageSize = 10,
                ItemCount = 2
            };

            _loanRepositoryMock.Setup(r => r.GetAllLoansAsync(query.PageNumber, query.PageSize))
                .ReturnsAsync((paginationMetadata, loans));

            _mapperMock.Setup(m => m.Map<List<LoanDto>>(loans)).Returns(loanDtos);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.Equal(paginationMetadata, result.Item1);
            Assert.Equal(loanDtos, result.Item2);
        }
    }
}
