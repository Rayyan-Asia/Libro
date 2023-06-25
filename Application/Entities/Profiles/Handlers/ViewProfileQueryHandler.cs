using Application.DTOs;
using Application.Entities.Profiles.Queries;
using AutoMapper;
using Domain;
using Application.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Entities.Profiles.Handlers
{
    public class ViewProfileQueryHandler : IRequestHandler<ViewProfileQuery, ProfileDto>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<ViewProfileQueryHandler> _logger;

        public ViewProfileQueryHandler(IUserRepository userRepository, IMapper mapper, ILogger<ViewProfileQueryHandler> logger)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<ProfileDto> Handle(ViewProfileQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Retrieving user with ID {request.PatronId}");
            var user = await _userRepository.GetUserProfileByIdAsync(request.PatronId);
            if (user == null)
            {
                _logger.LogError($"User NOT FOUND with ID {request.PatronId}");
                return null;
            }
            if (user.Role != Role.Patron)
            {
                _logger.LogError($"User IS NOT A PATRON with ID {request.PatronId}");
                return null;
            }
            var dto = _mapper.Map<ProfileDto>(user);
            return dto;
        }
    }
}
