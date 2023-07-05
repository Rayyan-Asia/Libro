using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Application.Entities.Emails.Commands
{
    public class SendUserEmailCommand : IRequest<IActionResult>
    {
        public int UserId { get; set; }

        public string Subject { get; set; }

        public string Body { get; set; }

    }
}
