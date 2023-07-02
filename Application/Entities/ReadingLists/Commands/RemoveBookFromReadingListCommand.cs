using Application.DTOs;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Application.Entities.ReadingLists.Commands
{
    public class RemoveBookFromReadingListCommand : IRequest<IActionResult>
    {
        public int UserId { get; set; }
        public int BookId { get; set; }
        public int ReadingListId { get; set; }
    }
}
