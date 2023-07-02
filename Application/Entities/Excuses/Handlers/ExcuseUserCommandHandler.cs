using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Entities.Excuses.Commands;
using Application.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Application.Entities.Excuses.Handlers
{
    public class ExcuseUserCommandHandler : IRequestHandler<ExcuseUserCommand, IActionResult>
    {
        private readonly ILoanRepository _loanRepository;
        private readonly ILogger<ExcuseUserCommandHandler> _logger;
        public ExcuseUserCommandHandler(ILoanRepository loanRepository, ILogger<ExcuseUserCommandHandler> logger)
        {
            _loanRepository = loanRepository;
            _logger = logger;
        }

        public async Task<IActionResult> Handle(ExcuseUserCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Excusing loans of user with ID {request.UserId}");
            var result = await _loanRepository.ExcuseLoansFromUserAsync(request.UserId);
            if (!result)
            {
                _logger.LogError($"loans NOT FOUND with user ID {request.UserId}");
                return new BadRequestObjectResult($"loans NOT FOUND with user ID {request.UserId}");
            }
            return new NoContentResult();
        }
    }
}
