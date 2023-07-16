using System;
using System.Collections.Generic;
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
    public class ListOverdueLoansQueryHandlerTests
    {
        private readonly Mock<ILoanRepository> _loanRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly ListOverdueLoansQueryHandler _handler;

        public ListOverdueLoansQueryHandlerTests()
        {
            _loanRepositoryMock = new Mock<ILoanRepository>();
            _mapperMock = new Mock<IMapper>();

            _handler = new ListOverdueLoansQueryHandler(
                _loanRepositoryMock.Object,
                _mapperMock.Object
            );
        }

        [Fact]
        public async Task Handle_WithValidRequest_ReturnsOverdueLoans()
        {
            // Arrange
            var query = new ListOverdueLoansQuery
            {
                PageNumber = 1,
                PageSize = 10
            };

            var loans = new List<Loan>
            {
                new Loan
                {
                    Id = 1,
                    DueDate = DateTime.UtcNow.AddDays(-5),
                    ReturnDate = null,
                    isExcused = false
                },
                new Loan
                {
                    Id = 2,
                    DueDate = DateTime.UtcNow.AddDays(-3),
                    ReturnDate = null,
                    isExcused = false
                }
            };

            var overdueLoanDtos = new List<LoanDto>
            {
                new LoanDto
                {
                    Id = 1,
                    DueDate = loans[0].DueDate
                },
                new LoanDto
                {
                    Id = 2,
                    DueDate = loans[1].DueDate
                }
            };

            _loanRepositoryMock.Setup(r => r.GetAllOverdueLoansAsync(query.PageNumber, query.PageSize))
                .ReturnsAsync((null, loans));

            _mapperMock.Setup(m => m.Map<List<LoanDto>>(loans))
                .Returns(overdueLoanDtos);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            (PaginationMetadata, List<LoanDto>) expectedMetadata = (null, overdueLoanDtos); 
            Assert.Equal(expectedMetadata, result);
        }
    }
}
