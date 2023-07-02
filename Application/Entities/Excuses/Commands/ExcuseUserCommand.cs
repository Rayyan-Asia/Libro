using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Application.Entities.Excuses.Commands
{
    public class ExcuseUserCommand : IRequest<IActionResult>
    {
        public int UserId { get; set; }
    }
}
