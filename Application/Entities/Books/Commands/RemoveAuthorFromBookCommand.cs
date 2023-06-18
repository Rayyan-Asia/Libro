using System.ComponentModel.DataAnnotations;
using Application.DTOs;
using MediatR;

namespace Application.Entities.Books.Commands
{
    public class RemoveAuthorFromBookCommand : IRequest<BookDto>
    {
        [Required]
        public int BookId { get; set; }

        [Required]
        public int AuthorId { get; set; }
    }
}
