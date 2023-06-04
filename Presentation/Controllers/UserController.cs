using Application.Interfaces;
using Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.JsonWebTokens;
using Presentation.DTOs;
using Presentation.Validators;

namespace Presentation.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        private readonly UserValidator _validator;

        public UserController(IUserService userService, UserValidator userValidator)
        {
            _userService = userService;
            _validator = userValidator;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto registerDto)
        {
            // use automapper
            // use fluent validations
            var user = new User
            {
                Email = registerDto.Email,
                Name = registerDto.Name,
                PhoneNumber = registerDto.PhoneNumber,
                Role = registerDto.Role,
            };

            var validationResult = _validator.Validate(user);
            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors);

            // Register the user
            var registeredUser = await _userService.RegisterUser(user, registerDto.Password);
            if (registeredUser == null)
            {
                return BadRequest();
            }

           // return jwt token
            return Ok();
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            if(! await _userService.UserExists(loginDto.Email))
                return BadRequest("User does not exist");
            // Perform user login
            var user = await _userService.Login(loginDto.Email, loginDto.Password);

            if (user == null)
            {
                return BadRequest("Bad password");
            }

            // return jwt token \
            return Ok();
        }
    }

}
