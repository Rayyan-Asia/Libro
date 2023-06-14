using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Entities.Excuses.Commands;
using Infrastructure.Interfaces;
using MediatR;

namespace Application.Entities.Excuses.Handlers
{
    public class ExcuseLoanCommandHandler : IRequestHandler<ExcuseLoanCommand, bool>
    {
        private readonly ILoanRepository _loanRepository;

        public ExcuseLoanCommandHandler(ILoanRepository loanRepository)
        {
            _loanRepository = loanRepository;
        }

        public async Task<bool> Handle(ExcuseLoanCommand request, CancellationToken cancellationToken)
        {
            var result = await _loanRepository.ExcuseLoan(request.LoanId);
            return result;
        }
    }
}
