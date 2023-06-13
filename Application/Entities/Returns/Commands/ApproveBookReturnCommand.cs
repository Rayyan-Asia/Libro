using Application.DTOs;
using MediatR;

namespace Application.Entities.Returns.Commands
{
    public class ApproveBookReturnCommand : IRequest<BookReturnDto>
    {
        public int BookReturnId { get; set; }
    }
}
