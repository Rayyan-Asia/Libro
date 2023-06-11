using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTOs;

namespace Application.Entities.Users
{
    public class AuthenticationResponse
    {
        public UserDto UserDto { get; set; }
        public string Jwt { get; set; }
    }

}
