using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTOs;
using Domain;
using MediatR;

namespace Application.Entities.Authors.Commands
{
    public class AddAuthorCommand : IRequest<AuthorDto>
    {
        
        [Required]
        [MaxLength(32)]
        public string Name { get; set; }

        [MaxLength(500)]
        public string Description { get; set; }

        public List<IdDto> Books;

        public AddAuthorCommand() { 
            Books = new List<IdDto>();
        }
    }
}
