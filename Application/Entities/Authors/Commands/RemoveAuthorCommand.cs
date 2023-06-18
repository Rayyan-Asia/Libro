using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace Application.Entities.Authors.Commands
{
    public class RemoveAuthorCommand : IRequest<bool>
    {
        [Required]
        public int Id { get; set; }
    }
}
