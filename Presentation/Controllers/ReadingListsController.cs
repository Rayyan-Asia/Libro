using System.Runtime.CompilerServices;
using System.Text.Json;
using Application;
using Application.Entities.ReadingLists.Commands;
using Application.Entities.ReadingLists.Queries;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Presentation.Validators.ReadingLists;

namespace Presentation.Controllers
{
    [Authorize(Policy = "PatronRequired")]
    [ApiController]
    [Route("api/[controller]")]
    public class ReadingListsController : Controller
    {
        private readonly IMediator _mediator;
        private readonly AddBookToReadingListCommandValidator _addBookToReadingListValidator;
        private readonly AddReadingListCommandValidator _addReadingListValidator;
        private readonly BrowseReadingListsQueryValidator _browseReadingListQueryValidator;
        private readonly EditReadingListCommandValidator _editReadingListValidator;
        private readonly RemoveBookFromReadingListCommandValidator _removeBookFromReadingListValidator;
        private readonly RemoveReadingListCommandValidator _removeReadingListValidator;

        public ReadingListsController(IMediator mediator, AddBookToReadingListCommandValidator addBookToReadingListValidator,
            AddReadingListCommandValidator addReadingListValidator, BrowseReadingListsQueryValidator browseReadingListQueryValidator,
            EditReadingListCommandValidator editReadingListValidator, RemoveBookFromReadingListCommandValidator removeBookFromReadingListValidator,
            RemoveReadingListCommandValidator removeReadingListValidator)
        {
            _mediator = mediator;
            _addBookToReadingListValidator = addBookToReadingListValidator;
            _addReadingListValidator = addReadingListValidator;
            _browseReadingListQueryValidator = browseReadingListQueryValidator;
            _editReadingListValidator = editReadingListValidator;
            _removeBookFromReadingListValidator = removeBookFromReadingListValidator;
            _removeReadingListValidator = removeReadingListValidator;
        }

        [HttpGet]
        public async Task<IActionResult> Index([FromBody] BrowseReadingListsQuery browseReadingListsQuery)
        {
            if (HttpContext.Request.Headers.TryGetValue("Authorization", out var authorizationHeader))
            {
                if (authorizationHeader.Count > 0)
                {
                    var token = authorizationHeader[0]?.Split(" ")[1]; // Extract the JWT token
                    var user = JwtService.GetUserFromPayload(token);

                    if (browseReadingListsQuery == null) browseReadingListsQuery = new() { UserId = user.Id};
                    ValidationResult validationResult = _browseReadingListQueryValidator.Validate(browseReadingListsQuery);
                    if (!validationResult.IsValid)
                    {
                        return BadRequest(validationResult.Errors);
                    }
                    var (pagination, list) = await _mediator.Send(browseReadingListsQuery);
                        Response.Headers.Add("X-Pagination",
                           JsonSerializer.Serialize(pagination));
                    return Ok(list);
                }
            }
            return Unauthorized();
            
        }


        [HttpPost("add")]
        public async Task<IActionResult> AddReadingList([FromBody] AddReadingListCommand addReadingListCommand)
        {
            if (HttpContext.Request.Headers.TryGetValue("Authorization", out var authorizationHeader))
            {
                if (authorizationHeader.Count > 0)
                {
                    var token = authorizationHeader[0]?.Split(" ")[1]; // Extract the JWT token
                    var user = JwtService.GetUserFromPayload(token);
                    if (addReadingListCommand == null) return BadRequest();
                    addReadingListCommand.UserId = user.Id;
                    ValidationResult validationResult = _addReadingListValidator.Validate(addReadingListCommand);
                    if (!validationResult.IsValid)
                    {
                        return BadRequest(validationResult.Errors);
                    }
                    var result = await _mediator.Send(addReadingListCommand);
                    if (result == null) return BadRequest();
                    return Ok(result);
                }
            }
            return Unauthorized();

        }


        [HttpPost("edit")]
        public async Task<IActionResult> EditReadingList([FromBody] EditReadingListCommand editReadingListCommand)
        {
            if (HttpContext.Request.Headers.TryGetValue("Authorization", out var authorizationHeader))
            {
                if (authorizationHeader.Count > 0)
                {
                    var token = authorizationHeader[0]?.Split(" ")[1]; // Extract the JWT token
                    var user = JwtService.GetUserFromPayload(token);
                    if (editReadingListCommand == null) return BadRequest();
                    editReadingListCommand.UserId = user.Id;
                    ValidationResult validationResult = _editReadingListValidator.Validate(editReadingListCommand);
                    if (!validationResult.IsValid)
                    {
                        return BadRequest(validationResult.Errors);
                    }
                    var result = await _mediator.Send(editReadingListCommand);
                    if (result == null) return BadRequest();
                    return Ok(result);
                }
            }
            return Unauthorized();
        }


        [HttpPost("remove/{id}")]
        public async Task<IActionResult> DeleteReadingList([FromRoute] int id)
        {
            if (HttpContext.Request.Headers.TryGetValue("Authorization", out var authorizationHeader))
            {
                if (authorizationHeader.Count > 0)
                {
                    if (id == 0) return BadRequest();
                    var token = authorizationHeader[0]?.Split(" ")[1]; // Extract the JWT token
                    var user = JwtService.GetUserFromPayload(token);
                    var removeReadingListCommand = new RemoveReadingListCommand() { UserId = user.Id , ReadingListId = id};
                    ValidationResult validationResult = _removeReadingListValidator.Validate(removeReadingListCommand);
                    if (!validationResult.IsValid)
                    {
                        return BadRequest(validationResult.Errors);
                    }
                    var result = await _mediator.Send(removeReadingListCommand);
                    if (!result) return BadRequest();
                    return Ok(result);
                }
            }
            return Unauthorized();

        }

        [HttpPost("add/book")]
        public async Task<IActionResult> AddBookToReadingList([FromBody] AddBookToReadingListCommand addBookToReadingListCommand)
        {
            if (HttpContext.Request.Headers.TryGetValue("Authorization", out var authorizationHeader))
            {
                if (authorizationHeader.Count > 0)
                {
                    var token = authorizationHeader[0]?.Split(" ")[1]; // Extract the JWT token
                    var user = JwtService.GetUserFromPayload(token);
                    addBookToReadingListCommand.UserId = user.Id;
                    ValidationResult validationResult = _addBookToReadingListValidator.Validate(addBookToReadingListCommand);
                    if (!validationResult.IsValid)
                    {
                        return BadRequest(validationResult.Errors);
                    }
                    var result = await _mediator.Send(addBookToReadingListCommand);
                    if (result == null) return BadRequest();
                    return Ok(result);
                }
            }
            return Unauthorized();
        }

        [HttpPost("remove/book")]
        public async Task<IActionResult> RemoveBookFromReadingList([FromBody] RemoveBookFromReadingListCommand removeBookFromReadingList)
        {
            if (HttpContext.Request.Headers.TryGetValue("Authorization", out var authorizationHeader))
            {
                if (authorizationHeader.Count > 0)
                {
                    var token = authorizationHeader[0]?.Split(" ")[1]; // Extract the JWT token
                    var user = JwtService.GetUserFromPayload(token);
                    removeBookFromReadingList.UserId = user.Id;
                    ValidationResult validationResult = _removeBookFromReadingListValidator.Validate(removeBookFromReadingList);
                    if (!validationResult.IsValid)
                    {
                        return BadRequest(validationResult.Errors);
                    }
                    var result = await _mediator.Send(removeBookFromReadingList);
                    if (result == null) return BadRequest();
                    return Ok(result);
                }
            }
            return Unauthorized();
        }





    }
}
