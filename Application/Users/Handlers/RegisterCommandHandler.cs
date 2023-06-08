using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTOs;
using Application.Users.Commands;
using AutoMapper;
using Domain;
using Infrastructure;
using Infrastructure.Interfaces;
using MediatR;
using Microsoft.Extensions.Configuration;

namespace Application.Users.Handlers
{
    public class RegisterCommandHandler : IRequestHandler<RegisterCommand, AuthenticationResponse>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;

        public RegisterCommandHandler(IUserRepository userRepository, IMapper mapper, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _configuration = configuration;
        }
        public async Task<AuthenticationResponse> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetUserByEmailAsync(request.Email);
            if (user != null) {
                return null;
            }
            var newUser = new User {  Email = request.Email , PhoneNumber = request.PhoneNumber, Name = request.Name};
            newUser.Salt = PasswordHasher.GenerateSalt();
            newUser.HashedPassword = PasswordHasher.ComputeHash(request.Password, newUser.Salt);

            var registeredUser = await _userRepository.AddUserAsync(newUser);

            var registeredUserDto = _mapper.Map<UserDto>(registeredUser);
            var jwt = JwtService.GenerateJwt(registeredUser, _configuration);

            return new AuthenticationResponse() { Jwt = jwt, UserDto = registeredUserDto};
        }
    }
}
