using System.Text.Json;
using Application.Entities.Feedbacks.Commands;
using Application.Entities.Returns.Commands;
using Application.Entities.Returns.Queries;
using Application.Services;
using Domain;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Presentation.Validators.Returns;

namespace Presentation.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ReturnsController : Controller
    {
        private readonly IMediator _mediator;
        private readonly ApproveBookReturnCommandValidator _approveValidator;
        private readonly BookReturnCommandValidator _bookReturnValidator;
        private readonly IJwtService _jwtService;

        public ReturnsController(IMediator mediator, ApproveBookReturnCommandValidator approveValidator,
            BookReturnCommandValidator bookReturnValidator, IJwtService jwtService)
        {
            _mediator = mediator;
            _approveValidator = approveValidator;
            _bookReturnValidator = bookReturnValidator;
            _jwtService = jwtService;
        }

        [HttpGet]
        [Authorize(Policy = "LibrarianRequired")]
        public async Task<IActionResult> Index([FromBody] BrowseReturnsQuery browseReturnsQuery)
        {
            if (browseReturnsQuery == null) browseReturnsQuery = new BrowseReturnsQuery();

            var (pagination, returns) = await _mediator.Send(browseReturnsQuery);
            Response.Headers.Add("X-Pagination",
               JsonSerializer.Serialize(pagination));
            return Ok(returns);
        }



        [HttpPost("return/{loanId}")]
        [Authorize(Policy = "PatronRequired")]
        public async Task<IActionResult> ReturnBook(int loanId)
        {
            if (HttpContext.Request.Headers.TryGetValue("Authorization", out var authorizationHeader))
            {
                if (authorizationHeader.Count > 0)
                {
                    var token = authorizationHeader[0]?.Split(" ")[1];// Extract the JWT token
                    User user;
                    try
                    {
                        user = _jwtService.GetUserFromPayload(token);
                    }catch (Exception ex)
                    {
                        return Unauthorized();
                    }
                    
                    if (user == null) return BadRequest();
                    if (loanId == 0)
                        return BadRequest();

                    var bookReturnCommand = new BookReturnCommand() { LoanId = loanId, UserId = user.Id };
                    ValidationResult validationResult = _bookReturnValidator.Validate(bookReturnCommand);
                    if (!validationResult.IsValid)
                    {
                        return BadRequest(validationResult.Errors);
                    }
                    return await _mediator.Send(bookReturnCommand);
                }
            }
            return Unauthorized();

        }




        [HttpPost("approve/{bookReturnId}")]
        [Authorize(Policy = "LibrarianRequired")]
        public async Task<IActionResult> ApproveReturnBook(int bookReturnId)
        {
            if (bookReturnId == 0)
                return BadRequest();
            var approveBookReturnCommand = new ApproveBookReturnCommand() { BookReturnId = bookReturnId };

            ValidationResult validationResult = _approveValidator.Validate(approveBookReturnCommand);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }
            return await _mediator.Send(approveBookReturnCommand);
        }
    }
}
