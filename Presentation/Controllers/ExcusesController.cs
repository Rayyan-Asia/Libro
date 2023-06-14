using Application.Entities.Excuses.Commands;
using Domain;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{
    [Authorize(Policy = "AdministratorOrLibrarianRequired")]
    [ApiController]
    [Route("api/[controller]")]
    public class ExcusesController : Controller
    {
        private readonly IMediator _mediator;

        public ExcusesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("loan/{loanId}")]
        public async Task<IActionResult> ExcuseLoan(int loanId)
        {
            if (loanId == 0)
            {
                return BadRequest();
            }
            var excuseLoanCommand = new ExcuseLoanCommand() { LoanId = loanId };
            var result = await _mediator.Send(excuseLoanCommand);
            if (!result)
            {
                return BadRequest();
            }
            return Ok();
        }


        [HttpPost("user/{userId}")]
        public async Task<IActionResult> ExcuseUser(int userId)
        {
            if (userId == 0)
            {
                return BadRequest();
            }
            var excuseUserCommand = new ExcuseUserCommand() { UserId = userId };
            var result = await _mediator.Send(excuseUserCommand);
            if (!result)
            {
                return BadRequest();
            }
            return Ok();
        }
    }
}
