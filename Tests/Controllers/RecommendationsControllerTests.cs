using System;
using System.Text.Json;
using System.Threading.Tasks;
using Application.Entities.Recommendations.Query;
using Application.Services;
using Domain;
using FluentValidation.Results;
using Infrastructure;
using Infrastructure.Services;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Presentation.Controllers;
using Presentation.Validators.Recommendations;
using Xunit;

namespace Tests.Controllers
{
    public class RecommendationsControllerTests
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly GetRecommendationQueryValidator _validator;
        private readonly RecommendationsController _controller;
        private readonly IJwtService _jwtService;

        public RecommendationsControllerTests()
        {
            _mediatorMock = new Mock<IMediator>();
            _validator = new GetRecommendationQueryValidator();
            _jwtService = new JwtService();
            _controller = new RecommendationsController(_mediatorMock.Object,_validator, _jwtService);
            var httpContext = new DefaultHttpContext();
            _controller.ControllerContext = new ControllerContext()
            {
                HttpContext = httpContext
            };
        }

        [Fact]
        public async Task Index_ValidRequest_ReturnsOkResult()
        {
            //Arrange
            SetupPatronContext();

            _mediatorMock.Setup(m => m.Send(It.IsAny<GetRecommendationQuery>(), default))
                .ReturnsAsync(new OkResult());
            // Act
            var result = await _controller.Index() as IActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.IsType<OkResult>(result);
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
    }
}
