using MediatR;

namespace Application.Entities.Excuses.Commands
{
    public class ExcuseLoanCommand : IRequest<bool>
    {
        public int LoanId { get; set; }
    }
}
