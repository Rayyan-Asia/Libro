using System.ComponentModel.DataAnnotations;
using Application.DTOs;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Application.Entities.Authors.Commands
{
    public class RemoveBookFromAuthorCommand : IRequest<IActionResult>
    {
        [Required]
        public int AuthorId { get; set; }

        [Required]
        public int BookId { get; set; }
    }
}
