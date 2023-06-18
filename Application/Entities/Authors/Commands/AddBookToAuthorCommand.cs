using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTOs;
using MediatR;

namespace Application.Entities.Authors.Commands
{
    public class AddBookToAuthorCommand : IRequest<AuthorDto>
    {
        [Required]
        public int AuthorId { get; set; }

        [Required]
        public int BookId { get; set; }
    }
}
