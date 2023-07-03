using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTOs;
using MediatR;

namespace Application.Entities.Loans.Queries
{
    public class ListOverdueLoansQuery : IRequest<(PaginationMetadata,List<LoanDto>)>
    {
        [Range(0,5)]
        public int PageSize { get; set; } = 5;
        public int PageNumber { get; set; } = 0;
    }
}
