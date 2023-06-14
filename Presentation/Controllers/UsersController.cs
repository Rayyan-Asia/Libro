using Application;
using Application.Entities.Profiles.Queries;
using Application.Entities.Users.Commands;
using Application.Entities.Users.Queries;
using Domain;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : Controller
    {
        private readonly IMediator _mediator;

        public UsersController(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterCommand userForRegistry)
        {
            if (userForRegistry == null || !ModelState.IsValid)
                return BadRequest();

            var result = await _mediator.Send(userForRegistry);

            if (result == null)
                return BadRequest("Email already exists");

            return Ok(result);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginQuery loginQuery)
        {
            if (loginQuery == null || !ModelState.IsValid)
                return BadRequest();

            var result = await _mediator.Send(loginQuery);


            return result != null ? Ok(result) : BadRequest("Does not exist");
        }

        [HttpPost("role")]
        [Authorize(Policy = "AdministratorRequired")]
        public async Task<IActionResult> ModifyRole([FromBody] ModifyRoleCommand modifyRoleCommand)
        {
            if (modifyRoleCommand == null || !ModelState.IsValid)
            {
                return BadRequest();
            }
            if (HttpContext.Request.Headers.TryGetValue("Authorization", out var authorizationHeader))
            {
                if (authorizationHeader.Count > 0)
                {
                    var token = authorizationHeader[0]?.Split(" ")[1]; // Extract the JWT token
                    var user = JwtService.GetUserFromPayload(token);
                    if (user?.Role == Role.Administrator && user.Id != modifyRoleCommand.UserId)
                    {
                        var result = await _mediator.Send(modifyRoleCommand);
                        if (result == null) return BadRequest();
                        return Ok(result);
                    }
                }
            }
            return Unauthorized();
        }

        [HttpGet("profile/{userid}")]
        [Authorize]
        public async Task<IActionResult> ViewProfile(int userId)
        { 
            var query = new ViewProfileQuery() { PatronId = userId };
            var result = await _mediator.Send(query);
            if (result == null)
                return BadRequest(); 
            return Ok(result);

        }

        [HttpGet("profile")]
        [Authorize(Policy = "PatronRequired")]
        public async Task<IActionResult> ViewProfile()
        {
            if (HttpContext.Request.Headers.TryGetValue("Authorization", out var authorizationHeader))
            {
                if (authorizationHeader.Count > 0)
                {
                    var token = authorizationHeader[0]?.Split(" ")[1]; // Extract the JWT token
                    var user = JwtService.GetUserFromPayload(token);
                    if (user == null) return BadRequest();
                    var query = new ViewProfileQuery() { PatronId = user.Id };
                    var result = await _mediator.Send(query);
                    if (result == null)
                        return BadRequest();
                    return Ok(result);

                }
            }
            return Unauthorized();
        }
    }

}
