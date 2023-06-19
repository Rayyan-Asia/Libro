using Application.DTOs;
using MediatR;

namespace Application.Entities.ReadingLists.Commands
{
    public class RemoveBookFromReadingListCommand : IRequest<ReadingListDto>
    {
        public int UserId { get; set; }
        public int BookId { get; set; }
        public int ReadingListId { get; set; }
    }
}
