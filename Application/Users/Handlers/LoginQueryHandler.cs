using Application.Users.Queries;
using AutoMapper;
using Infrastructure;
using Infrastructure.Interfaces;
using MediatR;
using Application.DTOs;
using Microsoft.Extensions.Configuration;
namespace Application.Users.Handlers
{
    public class LoginQueryHandler : IRequestHandler<LoginQuery, (UserDto,string)>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;

        public LoginQueryHandler(IUserRepository userRepository, IMapper mapper,IConfiguration configuration)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _configuration = configuration;
        }
        public async Task<(UserDto,string)> Handle(LoginQuery request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetUserByEmailAsync(request.Email);
            if (user == null)
            {
                return (null,null);
            }
            var isVerified = PasswordHasher.VerifyPassword(request.Password, user.Salt, user.HashedPassword);
            var jwt = TokenHandler.GenerateJwt(user, _configuration);
            if (isVerified)
                return (_mapper.Map<UserDto>(user), jwt);
            return (null, null);
        }

        
    }
}
