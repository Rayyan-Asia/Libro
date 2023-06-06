using System.Reflection.Metadata.Ecma335;
using Application.DTOs;
using Application.Users.Commands;
using Application.Users.Queries;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Presentation.Validators;

namespace Presentation.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        private readonly IMediator _mediator;

        public UserController(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterCommand userForRegistry)
        {
            if (userForRegistry == null || !ModelState.IsValid)
                return BadRequest();

            var result = await _mediator.Send(userForRegistry);

            if(result.Item1 == null)
                return BadRequest("Email already exists");

            var response = new AuthenticationResponse() { User = result.Item1, Jwt = result.Item2 };

            return Ok(response);

        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginQuery loginQuery)
        {
            if(loginQuery == null || !ModelState.IsValid)
                return BadRequest();

            var result = await _mediator.Send(loginQuery);

            var response = new AuthenticationResponse() { User = result.Item1, Jwt = result.Item2 };

            return result.Item1 != null ? Ok(response) : BadRequest("Does not exist");
        }

        public class AuthenticationResponse
        {
            public UserDto User { get; set; }
            public string Jwt { get; set; }
        }


    }

}
