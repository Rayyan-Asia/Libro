using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Application.Entities.ReadingLists.Commands
{
    public class RemoveReadingListCommand : IRequest<IActionResult>
    {
        public int ReadingListId { get; set; }
        public int UserId { get; set; }
    }
}
