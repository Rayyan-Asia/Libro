using System.Text.Json;
using Application;
using Application.Entities.Returns.Commands;
using Application.Entities.Returns.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ReturnsController : Controller
    {
        private readonly IMediator _mediator;

        public ReturnsController(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
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
                    var token = authorizationHeader[0]?.Split(" ")[1]; // Extract the JWT token
                    var user = JwtService.GetUserFromPayload(token);
                    if (user == null) return BadRequest();
                    if (loanId == 0)
                        return BadRequest();

                    var bookReturnCommand = new BookReturnCommand() { LoanId = loanId, UserId = user.Id };

                    var bookReturnDto = await _mediator.Send(bookReturnCommand);
                    if (bookReturnDto == null)
                        return BadRequest();
                    return Ok(bookReturnDto);
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

            var bookReturnCommand = new ApproveBookReturnCommand() { BookReturnId = bookReturnId };
            var bookReturnDto = await _mediator.Send(bookReturnCommand);
            if (bookReturnDto == null)
                return BadRequest();
            return Ok(bookReturnDto);
        }
    }
}
