using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTOs;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Application.Entities.Loans.Queries
{
    public class GetOverdueLoanQuery : IRequest<IActionResult> 
    {
        public int Id { get; set; }
        public double Rate { get; set; }
    }
}
