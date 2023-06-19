using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace Application.Entities.ReadingLists.Commands
{
    public class RemoveReadingListCommand : IRequest<bool>
    {
        public int ReadingListId { get; set; }
        public int UserId { get; set; }
    }
}
