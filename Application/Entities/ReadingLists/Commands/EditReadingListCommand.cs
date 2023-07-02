using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTOs;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Application.Entities.ReadingLists.Commands
{
    public class EditReadingListCommand : IRequest<IActionResult>
    {
        public int Id { get; set; }
        public List<IdDto> Books { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        public int UserId { get; set; }
    }
}
