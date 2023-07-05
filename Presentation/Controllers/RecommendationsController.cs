using Application.Entities.Feedbacks.Commands;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using Application.Entities.Recommendations.Query;
using Microsoft.EntityFrameworkCore.Metadata;
using Presentation.Validators.Recommendations;
using FluentValidation.Results;
using Application.Services;

namespace Presentation.Controllers
{
    [Authorize(Policy = "PatronRequired")]
    [ApiController]
    [Route("api/[controller]")]
    public class RecommendationsController : Controller
    {
        private readonly IMediator _mediator;
        private readonly GetRecommendationQueryValidator _validator;
        public RecommendationsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            if (HttpContext.Request.Headers.TryGetValue("Authorization", out var authorizationHeader))
            {
                if (authorizationHeader.Count > 0)
                {
                    var token = authorizationHeader[0]?.Split(" ")[1]; // Extract the JWT token
                    var user = JwtService.GetUserFromPayload(token);
                    var getRecommendationQuery = new GetRecommendationQuery() { UserId = user.Id};
                    ValidationResult validationResult = _validator.Validate(getRecommendationQuery);
                    if (!validationResult.IsValid)
                    {
                        return BadRequest(validationResult.Errors);
                    }
                    return await _mediator.Send(getRecommendationQuery);
                }
            }
            return Unauthorized();
        }
    }
}
