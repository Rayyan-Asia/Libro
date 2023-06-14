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
    public class ExcuseUserCommandHandler : IRequestHandler<ExcuseUserCommand, bool>
    {
        private readonly ILoanRepository _loanRepository;

        public ExcuseUserCommandHandler(ILoanRepository loanRepository)
        {
            _loanRepository = loanRepository;
        }

        public async Task<bool> Handle(ExcuseUserCommand request, CancellationToken cancellationToken)
        {
            var result = await _loanRepository.ExcuseLoansFromUser(request.UserId);
            return result;
        }
    }
}
