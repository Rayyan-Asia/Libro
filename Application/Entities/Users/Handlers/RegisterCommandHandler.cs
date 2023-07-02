using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTOs;
using Application.Entities.Users.Commands;
using Application.Interfaces;
using AutoMapper;
using Domain;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
namespace Application.Entities.Users.Handlers
{
    public class RegisterCommandHandler : IRequestHandler<RegisterCommand, IActionResult>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly ILogger<RegisterCommandHandler> _logger;

        public RegisterCommandHandler(IUserRepository userRepository, IMapper mapper, IConfiguration configuration, ILogger<RegisterCommandHandler> logger)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _configuration = configuration;
            _logger = logger;
        }

        public async Task<IActionResult> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Checking if other user with the same email exists with email: {request.Email}");
            var user = await _userRepository.GetUserByEmailAsync(request.Email);
            if (user != null)
            {
                _logger.LogError($"User FOUND with email {request.Email}");
                return new BadRequestObjectResult("User with the same email already exists.");
            }

            var newUser = new User { Email = request.Email, PhoneNumber = request.PhoneNumber, Name = request.Name };
            newUser.Salt = PasswordHasher.GenerateSalt();
            newUser.HashedPassword = PasswordHasher.ComputeHash(request.Password, newUser.Salt);

            _logger.LogInformation($"Adding User");
            var registeredUser = await _userRepository.AddUserAsync(newUser);

            var registeredUserDto = _mapper.Map<UserDto>(registeredUser);
            var jwt = JwtService.GenerateJwt(registeredUser, _configuration);

            return new OkObjectResult(new AuthenticationResponse() { Jwt = jwt, UserDto = registeredUserDto });
        }
    }
}
