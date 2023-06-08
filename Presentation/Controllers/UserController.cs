using System.Reflection.Metadata.Ecma335;
using Application.DTOs;
using Application.Users;
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

            if(result == null)
                return BadRequest("Email already exists");

            return Ok(result);

        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginQuery loginQuery)
        {
            if(loginQuery == null || !ModelState.IsValid)
                return BadRequest();

            var result = await _mediator.Send(loginQuery);


            return result != null ? Ok(result) : BadRequest("Does not exist");
        }

        

    }

}
