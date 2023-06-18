using System.ComponentModel.DataAnnotations;
using Application.DTOs;
using MediatR;

namespace Application.Entities.Authors.Commands
{
    public class RemoveBookFromAuthorCommand : IRequest<AuthorDto>
    {
        [Required]
        public int AuthorId { get; set; }

        [Required]
        public int BookId { get; set; }
    }
}
