using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTOs;
using Application.Entities.Loans.Queries;
using Application.Interfaces;
using AutoMapper;
using MediatR;

namespace Application.Entities.Loans.Handlers
{
    public class GetAllActiveLoansQueryHandler : IRequestHandler<GetAllActiveLoansQuery,(PaginationMetadata, List<LoanDto>)>
    {
        private readonly IMapper _mapper;
        private readonly ILoanRepository _loanRepository;

        public GetAllActiveLoansQueryHandler(IMapper mapper, ILoanRepository loanRepository)
        {
            _mapper = mapper;
            _loanRepository = loanRepository;
        }

        public async Task<(PaginationMetadata, List<LoanDto>)> Handle(GetAllActiveLoansQuery request, CancellationToken cancellationToken)
        {
            var (pagination, loans) = await _loanRepository.GetAllLoansAsync(request.PageNumber, request.PageSize);
            var loanDtos = _mapper.Map<List<LoanDto>>(loans);
            return (pagination, loanDtos);
        }
    }
}
