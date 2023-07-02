using AutoMapper;
using MediatR;
using Application.DTOs;
using Application.Entities.Users.Queries;
using Application.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;

namespace Application.Entities.Users.Handlers
{
    public class LoginQueryHandler : IRequestHandler<LoginQuery, IActionResult>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly ILogger<LoginQueryHandler> _logger;

        public LoginQueryHandler(IUserRepository userRepository, IMapper mapper, IConfiguration configuration, ILogger<LoginQueryHandler> logger)

        {
            _userRepository = userRepository;
            _mapper = mapper;
            _configuration = configuration;
            _logger = logger;
        }
        public async Task<IActionResult> Handle(LoginQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Retrieving user by email {request.Email}");
            var user = await _userRepository.GetUserByEmailAsync(request.Email);
            if (user == null)
            {
                _logger.LogError($"User NOT FOUND with email {request.Email}");
                return new NotFoundObjectResult("User not found with email " + request.Email);
            }
            var isVerified = PasswordHasher.VerifyPassword(request.Password, user.Salt, user.HashedPassword);
            var jwt = JwtService.GenerateJwt(user, _configuration);

            if (isVerified)
            {
                var userDto = _mapper.Map<UserDto>(user);
                var authenticationResponse = new AuthenticationResponse() { UserDto = userDto, Jwt = jwt };
                return new OkObjectResult(authenticationResponse);
            }
            _logger.LogError($"User entered wrong password");
            return new BadRequestObjectResult("User entered wrong password");
        }


    }
}
