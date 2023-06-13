using Application.DTOs;
using Application.Entities.Profiles.Queries;
using AutoMapper;
using Domain;
using Infrastructure.Interfaces;
using MediatR;

namespace Application.Entities.Profiles.Handlers
{
    public class ViewProfileQueryHandler : IRequestHandler<ViewProfileQuery, ProfileDto>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public ViewProfileQueryHandler(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<ProfileDto> Handle(ViewProfileQuery request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetUserProfileByIdAsync(request.PatronId);
            if (user == null || user.Role != Role.Patron)
                return null;
            var dto = _mapper.Map<ProfileDto>(user);
            return dto;
        }
    }
}
