using Application;
using Application.Entities.Feedbacks.Commands;
using Application.Entities.Profiles.Queries;
using Application.Entities.Users.Commands;
using Application.Entities.Users.Queries;
using Domain;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Presentation.Validators.Profiles;
using Presentation.Validators.Users;

namespace Presentation.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : Controller
    {
        private readonly IMediator _mediator;
        private readonly LoginQueryValidator _loginValidator;
        private readonly ModifyRoleCommandValidator _modifyRoleValidator;
        private readonly RegisterCommandValidator _registerValidator;
        private readonly ViewProfileQueryValidator _viewProfileValidator;

        public UsersController(IMediator mediator, LoginQueryValidator loginValidator,
            ModifyRoleCommandValidator modifyRoleValidator, RegisterCommandValidator registerValidator, 
            ViewProfileQueryValidator viewPorfileValidator)
        {
            _mediator = mediator;
            _loginValidator = loginValidator;
            _modifyRoleValidator = modifyRoleValidator;
            _registerValidator = registerValidator;
            _viewProfileValidator = viewPorfileValidator;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterCommand userForRegistry)
        {
            if (userForRegistry == null || !ModelState.IsValid)
                return BadRequest();
            ValidationResult validationResult = _registerValidator.Validate(userForRegistry);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }
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

            ValidationResult validationResult = _loginValidator.Validate(loginQuery);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

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
                        ValidationResult validationResult = _modifyRoleValidator.Validate(modifyRoleCommand);
                        if (!validationResult.IsValid)
                        {
                            return BadRequest(validationResult.Errors);
                        }
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
            if (userId <= 0) return BadRequest();
            var query = new ViewProfileQuery() { PatronId = userId };
            ValidationResult validationResult = _viewProfileValidator.Validate(query);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }
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
                    ValidationResult validationResult = _viewProfileValidator.Validate(query);
                    if (!validationResult.IsValid)
                    {
                        return BadRequest(validationResult.Errors);
                    }
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
