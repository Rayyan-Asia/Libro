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
using Microsoft.AspNetCore.Mvc;

namespace Application.Entities.Loans.Handlers
{
    public class GetUserOverdueLoansQueryHandler : IRequestHandler<GetUserOverdueLoansQuery, (PaginationMetadata, List<OverdueLoanDto>)>
    {
        private readonly ILoanRepository _loanRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public GetUserOverdueLoansQueryHandler(ILoanRepository loanRepository, IUserRepository userRepository, IMapper mapper)
        {
            _loanRepository = loanRepository;
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<(PaginationMetadata, List<OverdueLoanDto>)> Handle(GetUserOverdueLoansQuery request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetUserByIdAsync(request.UserId);
            if (user == null)
                return (null, null);
            var (paginationMetadata,loans) = await _loanRepository.GetAllOverdueLoansByUserIdAsync(request.PageNumber,request.PageSize,request.UserId);
            var loansDtos = _mapper.Map<List<OverdueLoanDto>>(loans);
            foreach (var loan in loansDtos)
            {
                var overdueTime = DateTime.Now - loan.DueDate;
                loan.Fee = Math.Round(overdueTime.TotalDays * (request.Rate/100.0), 2);
            }
            return (paginationMetadata, loansDtos);
        }
    }
}
