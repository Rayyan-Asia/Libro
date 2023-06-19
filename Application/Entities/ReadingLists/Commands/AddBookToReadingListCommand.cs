using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTOs;
using MediatR;

namespace Application.Entities.ReadingLists.Commands
{
    public class AddBookToReadingListCommand : IRequest<ReadingListDto>
    {
        public int UserId { get; set; } 
        public int BookId { get; set; } 
        public int ReadingListId { get; set; }
    }
}
