using System.Text.Json;
using Application;
using Application.Entities.Authors.Commands;
using Application.Entities.Feedbacks.Commands;
using Application.Entities.Feedbacks.Queries;
using Application.Entities.ReadingLists.Commands;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Presentation.Validators.Feedbacks;

namespace Presentation.Controllers
{
    [Authorize(Policy = "PatronRequired")]
    [ApiController]
    [Route("api/[controller]")]
    public class FeedbacksController : Controller
    {
        private readonly IMediator _mediator;
        private readonly BrowseBookFeedbackQueryValidator _browseBookFeedbackValidator;
        private readonly AddFeedbackCommandValidator _addFeedbackValidator;
        private readonly BrowseUserFeedbackQueryValidator _browseUserFeedbackValidator;
        private readonly EditFeedbackCommandValidator _editFeedbackValidator;
        private readonly RemoveFeedbackCommandValidator _removeFeedbackValidator;

        public FeedbacksController(IMediator mediator, BrowseBookFeedbackQueryValidator browseBookFeedbackValidator, 
            AddFeedbackCommandValidator addFeedbackValidator, BrowseUserFeedbackQueryValidator browseUserFeedbackValidator, 
            EditFeedbackCommandValidator editFeedbackValidator, RemoveFeedbackCommandValidator removeFeedbackValidator)
        {
            _mediator = mediator;
            _browseBookFeedbackValidator = browseBookFeedbackValidator;
            _addFeedbackValidator = addFeedbackValidator;
            _browseUserFeedbackValidator = browseUserFeedbackValidator;
            _editFeedbackValidator = editFeedbackValidator;
            _removeFeedbackValidator = removeFeedbackValidator;
        }

        [HttpGet("book")]
        public async Task<IActionResult> Index([FromBody] BrowseBookFeedbackQuery browseBookFeedbackQuery)
        {
            if(browseBookFeedbackQuery.BookId <= 0) return BadRequest();
            ValidationResult validationResult = _browseBookFeedbackValidator.Validate(browseBookFeedbackQuery);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }
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
            if (browseUserFeedbackQuery.UserId <= 0) return BadRequest();
            ValidationResult validationResult = _browseUserFeedbackValidator.Validate(browseUserFeedbackQuery);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }
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
                    ValidationResult validationResult = _addFeedbackValidator.Validate(addFeedbackCommand);
                    if (!validationResult.IsValid)
                    {
                        return BadRequest(validationResult.Errors);
                    }
                    return await _mediator.Send(addFeedbackCommand);
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
                    ValidationResult validationResult = _editFeedbackValidator.Validate(editFeedbackCommand);
                    if (!validationResult.IsValid)
                    {
                        return BadRequest(validationResult.Errors);
                    }
                    return await _mediator.Send(editFeedbackCommand);
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
                    if (id <= 0) return BadRequest();
                    var removeFeedbackCommand = new RemoveFeedbackCommand() { UserId = user.Id , FeedbackId = id};
                    ValidationResult validationResult = _removeFeedbackValidator.Validate(removeFeedbackCommand);
                    if (!validationResult.IsValid)
                    {
                        return BadRequest(validationResult.Errors);
                    }
                    return await _mediator.Send(removeFeedbackCommand);
                }
            }
            return Unauthorized();
        }
    }
}
