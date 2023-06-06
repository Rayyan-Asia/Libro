using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain;
using MediatR;

namespace Application.Users.Commands
{
    public class ChangeUserRoleCommand : IRequest<User>
    {
        public int UserId { get; set; }
        public int Role { get; set; }
    }
}
