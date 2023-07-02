using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTOs;
using Domain;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Application.Entities.Authors.Commands
{
    public class AddAuthorCommand : IRequest<IActionResult>
    {
        
        [Required]
        [MaxLength(32)]
        public string Name { get; set; }

        [MaxLength(500)]
        public string Description { get; set; }

        public List<IdDto> Books {get; set; }

        public AddAuthorCommand() { 
            Books = new List<IdDto>();
        }
    }
}
