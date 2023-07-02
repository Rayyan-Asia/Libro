using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Application.Entities.Excuses.Commands
{
    public class ExcuseLoanCommand : IRequest<IActionResult>
    {
        public int LoanId { get; set; }
    }
}
