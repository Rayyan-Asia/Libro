using Application.DTOs;
using Application.Entities.Users.Commands;
using AutoMapper;
using Application.Interfaces;
using MediatR;

namespace Application.Entities.Users.Handlers
{
    public class ModifyRoleCommandHandler : IRequestHandler<ModifyRoleCommand, UserDto>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        public ModifyRoleCommandHandler(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<UserDto> Handle(ModifyRoleCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetUserByIdAsync(request.UserId);
            if (user == null)
                return null;
            user.Role = request.Role;
            user = await _userRepository.UpdateUserAsync(user);
            var result = _mapper.Map<UserDto>(user);
            return result;
        }
    }
}
