using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace Application.Entities.Books.Commands
{
    public class RemoveBookCommand : IRequest<bool>
    {
        [Required]
        public int BookId { get; set; }
    }
}
