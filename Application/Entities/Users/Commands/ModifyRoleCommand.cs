using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTOs;
using Domain;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Application.Entities.Users.Commands
{
    public class ModifyRoleCommand : IRequest<IActionResult>
    {
        public int UserId { get; set; }
        public Role Role { get; set; }
    }
}
