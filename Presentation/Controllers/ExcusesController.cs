using System.Security.AccessControl;
using Application.Entities.Authors.Commands;
using Application.Entities.Excuses.Commands;
using Domain;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Presentation.Validators.Excuses;

namespace Presentation.Controllers
{
    [Authorize(Policy = "AdministratorOrLibrarianRequired")]
    [ApiController]
    [Route("api/[controller]")]
    public class ExcusesController : Controller
    {
        private readonly IMediator _mediator;
        private readonly ExcuseLoanCommandValidator _excuseLoanValidator;
        private readonly ExcuseUserCommandValidator _excuseUserValidator;

        public ExcusesController(IMediator mediator, ExcuseLoanCommandValidator excuseLoanValidator, ExcuseUserCommandValidator excuseUserValidator)
        {
            _mediator = mediator;
            _excuseLoanValidator = excuseLoanValidator;
            _excuseUserValidator = excuseUserValidator;
        }

        [HttpPost("loan/{loanId}")]
        public async Task<IActionResult> ExcuseLoan(int loanId)
        {
            if (loanId <= 0)
            {
                return BadRequest();
            }
            var excuseLoanCommand = new ExcuseLoanCommand() { LoanId = loanId };
            ValidationResult validationResult = _excuseLoanValidator.Validate(excuseLoanCommand);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }
            return await _mediator.Send(excuseLoanCommand); 
        }


        [HttpPost("user/{userId}")]
        public async Task<IActionResult> ExcuseUser(int userId)
        {
            if (userId <= 0)
            {
                return BadRequest();
            }
            var excuseUserCommand = new ExcuseUserCommand() { UserId = userId };
            ValidationResult validationResult = _excuseUserValidator.Validate(excuseUserCommand);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }
            return await _mediator.Send(excuseUserCommand);

        }
    }
}
