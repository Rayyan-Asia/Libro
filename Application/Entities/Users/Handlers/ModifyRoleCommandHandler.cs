﻿using Application.DTOs;
using Application.Entities.Users.Commands;
using AutoMapper;
using Application.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Entities.Users.Handlers
{
    public class ModifyRoleCommandHandler : IRequestHandler<ModifyRoleCommand, UserDto>
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

        public async Task<UserDto> Handle(ModifyRoleCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Retrieving user with ID {request.UserId}");
            var user = await _userRepository.GetUserByIdAsync(request.UserId);
            if (user == null) {
                _logger.LogError($"User NOT FOUND with ID {request.UserId}");
                return null; 
            }
            user.Role = request.Role;
            _logger.LogInformation($"Updating user with ID {request.UserId} to role {user.Role}");
            user = await _userRepository.UpdateUserAsync(user);
            var result = _mapper.Map<UserDto>(user);
            return result;
        }
    }
}
