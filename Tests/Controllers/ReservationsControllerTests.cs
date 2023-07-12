using System;
using System.Text.Json;
using System.Threading.Tasks;
using Application;
using Application.DTOs;
using Application.Entities.Reservations.Commnads;
using Application.Entities.Reservations.Queries;
using Domain;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Presentation.Controllers;
using Presentation.Validators.Reservations;
using Xunit;

namespace Tests.Controllers
{
    public class ReservationsControllerTests
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly ReserveBookCommandValidator _reserveBookCommandValidator;
        private readonly ApproveReservationCommandValidator _approveReservationCommandValidator;
        private readonly ReservationsController _controller;

        public ReservationsControllerTests()
        {
            _mediatorMock = new Mock<IMediator>();
            _reserveBookCommandValidator = new ReserveBookCommandValidator();
            _approveReservationCommandValidator = new ApproveReservationCommandValidator();
            _controller = new ReservationsController(
                _mediatorMock.Object,
                _reserveBookCommandValidator,
                _approveReservationCommandValidator
            );
        }

        [Fact]
        public async Task Reserve_ValidRequest_ReturnsOkResult()
        {
            // Arrange

            _mediatorMock.Setup(m => m.Send(It.IsAny<ReserveBookCommand>(), default))
                .ReturnsAsync(new OkResult());
            SetupPatronContext();

            // Act
            var result = await _controller.Reserve(1) as IActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async Task Reserve_InvalidRequest_ReturnsBadRequest()
        {
          

            _mediatorMock.Setup(m => m.Send(It.IsAny<ReserveBookCommand>(), default))
                .ReturnsAsync(new OkResult());
            SetupPatronContext();
            // Act
            var result = await _controller.Reserve(0) as IActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task Reservations_ValidRequest_ReturnsOkResult()
        {
            var browseReservationsQuery = new BrowseReservationsQuery();
            var paginationMetadata = new PaginationMetadata();
            var reservations = new List<ReservationDto>() { new ReservationDto(),};

            _mediatorMock.Setup(m => m.Send(browseReservationsQuery, default))
                .ReturnsAsync((paginationMetadata, reservations));
            SetupLibrarianContext();

            // Act
            var result = await _controller.Reservations(browseReservationsQuery) as IActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.IsType<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.Equal(JsonSerializer.Serialize(reservations), JsonSerializer.Serialize(okResult.Value));
        }

        [Fact]
        public async Task ApproveReservation_ValidRequest_ReturnsOkResult()
        {
            // Arrange
            var reservationId = 1;

            _mediatorMock.Setup(m => m.Send(It.IsAny<ApproveReservationCommand>(), default))
                .ReturnsAsync(new OkResult());
            SetupLibrarianContext();
            // Act
            var result = await _controller.ApproveReservation(reservationId) as IActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async Task ApproveReservation_InvalidRequest_ReturnsBadRequest()
        {
            // Arrange
            var command = new ApproveReservationCommand {};
            SetupLibrarianContext();
            _mediatorMock.Setup(m => m.Send(command, default))
                .ReturnsAsync(new OkResult());

            // Act
            var result = await _controller.ApproveReservation(0) as IActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.IsType<BadRequestObjectResult>(result);
        }

        private void SetupPatronContext()
        {
            var httpContext = new DefaultHttpContext();
            var authorizationHeader = "Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hc" +
                "y54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1laWRlbnRpZmllciI6IjMiLCJodHRwOi8vc2N" +
                "oZW1hcy5taWNyb3NvZnQuY29tL3dzLzIwMDgvMDYvaWRlbnRpdHkvY2xhaW1zL3JvbGUiOiJQYXRyb24iLCJuYmYiOjE2OD" +
                "kxNzExMDUsImV4cCI6MTY4OTE3NDcwNSwiaXNzIjoiaHR0cHM6Ly9sb2NhbGhvc3Q6NzI3OSIsImF1ZCI6IlByZXNlbnRhdGlv" +
                "biJ9.Bc_UUu787Z9DoXGRY0iuHyGUqDwqZmBPjeCMY9-ZNyE";
            httpContext.Request.Headers.Add("Authorization", authorizationHeader);

            _controller.ControllerContext = new ControllerContext()
            {
                HttpContext = httpContext
            };
        }

        private void SetupLibrarianContext()
        {
            var httpContext = new DefaultHttpContext();
            var authorizationHeader = "Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9." +
                "eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2N" +
                "sYWltcy9uYW1laWRlbnRpZmllciI6IjIiLCJodHRwOi8vc2NoZW1hcy5taWNyb3NvZnQ" +
                "uY29tL3dzLzIwMDgvMDYvaWRlbnRpdHkvY2xhaW1zL3JvbGUiOiJMaWJyYXJpYW4iLCJu" +
                "YmYiOjE2ODkxNTgxNzEsImV4cCI6MTY4OTE2MTc3MSwiaXNzIjoiaHR0cHM6Ly9sb2NhbGh" +
                "vc3Q6NzI3OSIsImF1ZCI6IlByZXNlbnRhdGlvbiJ9.D21Q7qrU40nHht3xJew6qN_Vmq4Jq4XKzOdlB--gxN0";
            httpContext.Request.Headers.Add("Authorization", authorizationHeader);

            _controller.ControllerContext = new ControllerContext()
            {
                HttpContext = httpContext
            };
        }
    }
}
