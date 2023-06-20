using System.Text.Json;
using Application;
using Application.Entities.Feedbacks.Commands;
using Application.Entities.Feedbacks.Queries;
using Application.Entities.ReadingLists.Commands;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{
    [Authorize(Policy = "PatronRequired")]
    [ApiController]
    [Route("api/[controller]")]
    public class FeedbacksController : Controller
    {
        private readonly IMediator _mediator;

        public FeedbacksController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("book")]
        public async Task<IActionResult> Index([FromBody] BrowseBookFeedbackQuery browseBookFeedbackQuery)
        {
            if(browseBookFeedbackQuery.BookId == 0) return BadRequest();
            var (pagination, list) = await _mediator.Send(browseBookFeedbackQuery);
            if(pagination == null)
                return NotFound();
            Response.Headers.Add("X-Pagination",
               JsonSerializer.Serialize(pagination));
            return Ok(list);
        }

        [HttpGet("user")]
        public async Task<IActionResult> ListFeedbacksByUser([FromBody] BrowseUserFeedbackQuery browseUserFeedbackQuery)
        {
            if (browseUserFeedbackQuery.UserId == 0) return BadRequest();
            var (pagination, list) = await _mediator.Send(browseUserFeedbackQuery);
            if (pagination == null)
                return NotFound();
            Response.Headers.Add("X-Pagination",
               JsonSerializer.Serialize(pagination));
            return Ok(list);
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddFeedbackAsync([FromBody] AddFeedbackCommand addFeedbackCommand)
        {
            if (HttpContext.Request.Headers.TryGetValue("Authorization", out var authorizationHeader))
            {
                if (authorizationHeader.Count > 0)
                {
                    var token = authorizationHeader[0]?.Split(" ")[1]; // Extract the JWT token
                    var user = JwtService.GetUserFromPayload(token);
                    if (addFeedbackCommand == null) return BadRequest();
                    addFeedbackCommand.UserId = user.Id;
                    var result = await _mediator.Send(addFeedbackCommand);
                    if (result == null)
                        return BadRequest();
                    return Ok(result);
                }
            }
            return Unauthorized();
        }

        [HttpPost("edit")]
        public async Task<IActionResult> EditFeedbackAsync([FromBody] EditFeedbackCommand editFeedbackCommand)
        {
            if (HttpContext.Request.Headers.TryGetValue("Authorization", out var authorizationHeader))
            {
                if (authorizationHeader.Count > 0)
                {
                    var token = authorizationHeader[0]?.Split(" ")[1]; // Extract the JWT token
                    var user = JwtService.GetUserFromPayload(token);
                    if (editFeedbackCommand == null) return BadRequest();
                    editFeedbackCommand.UserId = user.Id;
                    var result = await _mediator.Send(editFeedbackCommand);
                    if (result == null)
                        return BadRequest();
                    return Ok(result);
                }
            }
            return Unauthorized();
        }

        [HttpPost("remove/{id}")]
        public async Task<IActionResult> RemoveFeedbackAsync([FromRoute] int id)
        {
            if (HttpContext.Request.Headers.TryGetValue("Authorization", out var authorizationHeader))
            {
                if (authorizationHeader.Count > 0)
                {
                    var token = authorizationHeader[0]?.Split(" ")[1]; // Extract the JWT token
                    var user = JwtService.GetUserFromPayload(token);
                    if (id == 0) return BadRequest();
                    var removeFeedbackCommand = new RemoveFeedbackCommand() { UserId = user.Id , FeedbackId = id};
                    var result = await _mediator.Send(removeFeedbackCommand);
                    if (!result)
                        return BadRequest();
                    return Ok();
                }
            }
            return Unauthorized();
        }
    }
}
