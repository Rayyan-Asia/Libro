using AutoMapper;
using MediatR;
using Application.DTOs;
using Application.Entities.Users.Queries;
using Application.Interfaces;
using Microsoft.Extensions.Configuration;

namespace Application.Entities.Users.Handlers
{
    public class LoginQueryHandler : IRequestHandler<LoginQuery, AuthenticationResponse>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;

        public LoginQueryHandler(IUserRepository userRepository, IMapper mapper, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _configuration = configuration;
        }
        public async Task<AuthenticationResponse> Handle(LoginQuery request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetUserByEmailAsync(request.Email);
            if (user == null)
            {
                return null;
            }
            var isVerified = PasswordHasher.VerifyPassword(request.Password, user.Salt, user.HashedPassword);
            var jwt = JwtService.GenerateJwt(user, _configuration);

            if (isVerified)
            {
                var userDto = _mapper.Map<UserDto>(user);
                return new AuthenticationResponse() { UserDto = userDto, Jwt = jwt };
            }
            return null;
        }


    }
}
