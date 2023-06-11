using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTOs;
using Domain;
using MediatR;

namespace Application.Entities.Users.Commands
{
    public class ModifyRoleCommand : IRequest<UserDto>
    {
        public int UserId { get; set; }
        public Role Role { get; set; }
    }
}
