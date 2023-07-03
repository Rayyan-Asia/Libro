using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTOs;
using Application.Entities.Loans.Queries;
using Application.Interfaces;
using AutoMapper;
using Domain;
using MediatR;

namespace Application.Entities.Loans.Handlers
{
    public class ListOverdueLoansQueryHandler : IRequestHandler<ListOverdueLoansQuery,(PaginationMetadata,List<LoanDto>)>
    {
        private readonly ILoanRepository _loanRepository;
        private readonly IMapper _mapper;

        public ListOverdueLoansQueryHandler(ILoanRepository loanRepository, IMapper mapper)
        {
            _loanRepository = loanRepository;
            _mapper = mapper;
        }

        public async Task<(PaginationMetadata, List<LoanDto>)> Handle(ListOverdueLoansQuery request, CancellationToken cancellationToken)
        {
            var (pagination, loans) = await _loanRepository.GetAllOverdueLoansAsync(request.PageNumber, request.PageSize);
            var loansDtos = _mapper.Map<List<LoanDto>>(loans);
            return (pagination, loansDtos);
        }
    }
}
