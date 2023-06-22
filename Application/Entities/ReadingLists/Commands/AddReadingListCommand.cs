using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTOs;
using Domain;
using MediatR;

namespace Application.Entities.ReadingLists.Commands
{
    public class AddReadingListCommand : IRequest<ReadingListDto>
    {
        public List<IdDto> Books { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        public int UserId { get; set; }
    }
}
