using System;
using System.Threading;
using System.Threading.Tasks;
using Application.DTOs;
using Application.Entities.Loans.Handlers;
using Application.Entities.Loans.Queries;
using Application.Interfaces;
using AutoMapper;
using Domain;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Tests.Handlers.Loans
{
    public class GetOverdueLoanQueryHandlerTests
    {
        private readonly Mock<ILoanRepository> _loanRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly GetOverdueLoanQueryHandler _handler;

        public GetOverdueLoanQueryHandlerTests()
        {
            _loanRepositoryMock = new Mock<ILoanRepository>();
            _mapperMock = new Mock<IMapper>();

            _handler = new GetOverdueLoanQueryHandler(
                _loanRepositoryMock.Object,
                _mapperMock.Object
            );
        }

        [Fact]
        public async Task Handle_WithOverdueLoan_ReturnsOverdueLoanDto()
        {
            // Arrange
            var query = new GetOverdueLoanQuery
            {
                Id = 1,
                Rate = 0.1 // 10% rate
            };

            var overdueLoan = new Loan
            {
                Id = 1,
                DueDate = DateTime.UtcNow.AddDays(-5),
                ReturnDate = null,
                isExcused = false
            };

            var overdueLoanDto = new OverdueLoanDto
            {
                Id = 1,
                DueDate = overdueLoan.DueDate,
                Fee = Math.Round((DateTime.Now - overdueLoan.DueDate).Days * (query.Rate / 100.0), 2)
            };

            _loanRepositoryMock.Setup(r => r.GetLoanByIdAsync(query.Id))
                .ReturnsAsync(overdueLoan);

            _mapperMock.Setup(m => m.Map<OverdueLoanDto>(overdueLoan))
                .Returns(overdueLoanDto);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var resultDto = Assert.IsType<OverdueLoanDto>(okResult.Value);

            Assert.Equal(overdueLoanDto.Id, resultDto.Id);
            Assert.Equal(overdueLoanDto.DueDate, resultDto.DueDate);
            Assert.Equal(overdueLoanDto.Fee, resultDto.Fee);
        }

        [Fact]
        public async Task Handle_WithNonexistentLoan_ReturnsNotFoundResult()
        {
            // Arrange
            var query = new GetOverdueLoanQuery
            {
                Id = 1,
                Rate = 0.1
            };

            _loanRepositoryMock.Setup(r => r.GetLoanByIdAsync(query.Id))
                .ReturnsAsync((Loan)null);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("Loan is NOT FOUND with ID 1", notFoundResult.Value);
        }

        [Fact]
        public async Task Handle_WithNonOverdueLoan_ReturnsBadRequestResult()
        {
            // Arrange
            var query = new GetOverdueLoanQuery
            {
                Id = 1,
                Rate = 0.1
            };

            var loan = new Loan
            {
                Id = 1,
                DueDate = DateTime.UtcNow.AddDays(5),
                ReturnDate = null,
                isExcused = false
            };

            _loanRepositoryMock.Setup(r => r.GetLoanByIdAsync(query.Id))
                .ReturnsAsync(loan);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Loan is NOT OVERDUE", badRequestResult.Value);
        }
    }
}
