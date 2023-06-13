using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTOs;
using MediatR;

namespace Application.Entities.Returns.Commands
{
    public class BookReturnCommand : IRequest<BookReturnDto>
    {
        public int LoanId { get; set; }
        public int UserId { get; set; }
    }
}
