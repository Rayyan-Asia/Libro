using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Entities.Excuses.Commands;
using Application.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Entities.Excuses.Handlers
{
    public class ExcuseLoanCommandHandler : IRequestHandler<ExcuseLoanCommand, bool>
    {
        private readonly ILoanRepository _loanRepository;
        private readonly ILogger<ExcuseLoanCommandHandler> _logger;
        public ExcuseLoanCommandHandler(ILoanRepository loanRepository, ILogger<ExcuseLoanCommandHandler> logger)
        {
            _loanRepository = loanRepository;
            _logger = logger;
        }

        public async Task<bool> Handle(ExcuseLoanCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Excusing loan with ID {request.LoanId}");
            var result = await _loanRepository.ExcuseLoanAsync(request.LoanId);
            if (!result)
                _logger.LogError($"Loan NOT FOUND with ID {request.LoanId}");
            return result;
        }
    }
}
