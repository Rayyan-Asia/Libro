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
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Tests.Handler.Loans
{
    public class GetUserOverdueLoansQueryHandlerTests
    {
        private readonly Mock<ILoanRepository> _loanRepositoryMock;
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly GetUserOverdueLoansQueryHandler _handler;

        public GetUserOverdueLoansQueryHandlerTests()
        {
            _loanRepositoryMock = new Mock<ILoanRepository>();
            _userRepositoryMock = new Mock<IUserRepository>();
            _mapperMock = new Mock<IMapper>();

            _handler = new GetUserOverdueLoansQueryHandler(
                _loanRepositoryMock.Object,
                _userRepositoryMock.Object,
                _mapperMock.Object
            );
        }

        [Fact]
        public async Task Handle_WithValidUserId_ReturnsUserOverdueLoans()
        {
            // Arrange
            var query = new GetUserOverdueLoansQuery
            {
                UserId = 1,
                PageNumber = 1,
                PageSize = 10,
                Rate = 0.1 // 10% rate
            };

            var user = new User
            {
                Id = 1,
                Name = "John Doe"
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

            var overdueLoanDtos = new List<OverdueLoanDto>
            {
                new OverdueLoanDto
                {
                    Id = 1,
                    DueDate = loans[0].DueDate,
                    Fee = Math.Round((DateTime.Now - loans[0].DueDate).TotalDays * (query.Rate / 100.0), 2)
                },
                new OverdueLoanDto
                {
                    Id = 2,
                    DueDate = loans[1].DueDate,
                    Fee = Math.Round((DateTime.Now - loans[1].DueDate).TotalDays * (query.Rate / 100.0), 2)
                }
            };

            _userRepositoryMock.Setup(r => r.GetUserByIdAsync(query.UserId))
                .ReturnsAsync(user);

            _loanRepositoryMock.Setup(r => r.GetAllOverdueLoansByUserIdAsync(query.PageNumber, query.PageSize, query.UserId))
                .ReturnsAsync((null, loans));

            _mapperMock.Setup(m => m.Map<List<OverdueLoanDto>>(loans))
                .Returns(overdueLoanDtos);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            (PaginationMetadata,List<OverdueLoanDto>) expectedMetadata = (null, overdueLoanDtos);
            Assert.Equal(expectedMetadata, result);
        }

        [Fact]
        public async Task Handle_WithNonexistentUser_ReturnsNullResult()
        {
            // Arrange
            var query = new GetUserOverdueLoansQuery
            {
                UserId = 1,
                PageNumber = 1,
                PageSize = 10,
                Rate = 0.1
            };

            _userRepositoryMock.Setup(r => r.GetUserByIdAsync(query.UserId))
                .ReturnsAsync((User)null);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            (PaginationMetadata, List<OverdueLoanDto>) expectedMetadata = (null, null);
            Assert.Equal(expectedMetadata, result);
        }
    }
}
