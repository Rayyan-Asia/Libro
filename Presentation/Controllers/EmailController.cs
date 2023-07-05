using Application.Entities.Emails.Commands;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Presentation.Validators.Emails;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmailController : Controller
    {
        private IMediator _mediator; 
        private SendUserEmailCommandValidator _validator;

        public EmailController(IMediator mediator, SendUserEmailCommandValidator validator)
        {
            _mediator = mediator;
            _validator = validator;
        }

        [HttpPost("add")]
        [Authorize(Policy = "AdministratorOrLibrarianRequired")]
        public async Task<IActionResult> add([FromBody] SendUserEmailCommand sendUserEmailCommand)
        {
            var validationResult = _validator.Validate(sendUserEmailCommand);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }
            return await _mediator.Send(sendUserEmailCommand);
        }
    }
}
