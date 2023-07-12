using Application.Entities.Feedbacks.Commands;
using Application.Entities.Profiles.Queries;
using Application.Entities.Users.Commands;
using Application.Entities.Users.Queries;
using Application.Services;
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
            return await _mediator.Send(userForRegistry);
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

            return await _mediator.Send(loginQuery);
        }

        [HttpPost("role")]
        [Authorize(Policy = "AdministratorRequired")]
        public async Task<IActionResult> ModifyRole([FromBody] ModifyRoleCommand modifyRoleCommand)
        {
            if (modifyRoleCommand == null)
            {
                return BadRequest();
            }
            if (HttpContext.Request.Headers.TryGetValue("Authorization", out var authorizationHeader))
            {
                if (authorizationHeader.Count > 0)
                {
                    var token = authorizationHeader[0]?.Split(" ")[1]; // Extract the JWT token
                    User user;
                    try
                    {
                        user = JwtService.GetUserFromPayload(token);
                    }
                    catch (Exception ex)
                    {
                        return Unauthorized();
                    }
                    if (user?.Role == Role.Administrator && user.Id != modifyRoleCommand.UserId)
                    {
                        ValidationResult validationResult = _modifyRoleValidator.Validate(modifyRoleCommand);
                        if (!validationResult.IsValid)
                        {
                            return BadRequest(validationResult.Errors);
                        }
                        return await _mediator.Send(modifyRoleCommand);
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
            return await _mediator.Send(query);
            

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
                    User user;
                    try
                    {
                        user = JwtService.GetUserFromPayload(token);
                    }
                    catch (Exception ex)
                    {
                        return Unauthorized();
                    }
                    if (user == null) return BadRequest();
                    var query = new ViewProfileQuery() { PatronId = user.Id };
                    ValidationResult validationResult = _viewProfileValidator.Validate(query);
                    if (!validationResult.IsValid)
                    {
                        return BadRequest(validationResult.Errors);
                    }
                    return await _mediator.Send(query);

                }
            }
            return Unauthorized();
        }
    }

}
