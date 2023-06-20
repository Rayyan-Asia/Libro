using Application.Entities.Feedbacks.Commands;
using Application;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using Application.Entities.Recommendations.Query;

namespace Presentation.Controllers
{
    [Authorize(Policy = "PatronRequired")]
    [ApiController]
    [Route("api/[controller]")]
    public class RecommendationsController : Controller
    {
        private readonly IMediator _mediator;

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
                    var result = await _mediator.Send(getRecommendationQuery);
                    if (result == null)
                        return BadRequest();
                    return Ok(result);
                }
            }
            return Unauthorized();
        }
    }
}
