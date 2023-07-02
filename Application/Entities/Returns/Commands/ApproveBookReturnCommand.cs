using Application.DTOs;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Application.Entities.Returns.Commands
{
    public class ApproveBookReturnCommand : IRequest<IActionResult>
    {
        public int BookReturnId { get; set; }
    }
}
