using Application.DTOs;
using Application.Entities.Loans.Queries;
using Application.Interfaces;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Application.Entities.Loans.Handlers
{
    public class GetOverdueLoanQueryHandler : IRequestHandler<GetOverdueLoanQuery, IActionResult>
    {
        private readonly ILoanRepository _loanRepository;
        private readonly IMapper _mapper;

        public GetOverdueLoanQueryHandler(ILoanRepository loanRepository, IMapper mapper)
        {
            _loanRepository = loanRepository;
            _mapper = mapper;
        }

        public async Task<IActionResult> Handle(GetOverdueLoanQuery request, CancellationToken cancellationToken)
        {
            var loan = await _loanRepository.GetLoanByIdAsync(request.Id);
            if (loan == null)
                return new NotFoundObjectResult("Loan is NOT FOUND with ID " + request.Id);
            if (loan.isExcused || loan.ReturnDate != null || loan.DueDate >= DateTime.UtcNow)
                return new BadRequestObjectResult("Loan is NOT OVERDUE");
            var loanDto = _mapper.Map<OverdueLoanDto>(loan);
            loanDto.Fee = Math.Round((DateTime.Now - loan.DueDate).Days * (request.Rate / 100.0), 2);
            return new OkObjectResult(loanDto);
        }
    }
}
