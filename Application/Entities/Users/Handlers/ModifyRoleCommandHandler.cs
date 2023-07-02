using Application.DTOs;
using Application.Entities.Users.Commands;
using AutoMapper;
using Application.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;

namespace Application.Entities.Users.Handlers
{
    public class ModifyRoleCommandHandler : IRequestHandler<ModifyRoleCommand, IActionResult>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<ModifyRoleCommandHandler> _logger;
        public ModifyRoleCommandHandler(IUserRepository userRepository, IMapper mapper, ILogger<ModifyRoleCommandHandler> logger)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IActionResult> Handle(ModifyRoleCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Retrieving user with ID {request.UserId}");
            var user = await _userRepository.GetUserByIdAsync(request.UserId);
            if (user == null)
            {
                _logger.LogError($"User NOT FOUND with ID {request.UserId}");
                return new NotFoundObjectResult("User not found with ID " + request.UserId);
            }

            user.Role = request.Role;
            _logger.LogInformation($"Updating user with ID {request.UserId} to role {user.Role}");
            user = await _userRepository.UpdateUserAsync(user);
            return new OkObjectResult(_mapper.Map<UserDto>(user));
        }
    }
}
