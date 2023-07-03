using System.Text.Json;
using Application.Entities.Feedbacks.Queries;
using Application.Entities.Loans.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using Presentation.Validators.Loans;

namespace Presentation.Controllers
{
    [Authorize(Policy = "AdministratorOrLibrarianRequired")]
    [ApiController]
    [Route("api/[controller]")]
    public class LoansController : Controller
    {
        private readonly GetAllActiveLoansQueryValidator _activeLoansValidator;
        private readonly GetOverdueLoanQueryValidator _overdueLoanValidator;
        private readonly ListOverdueLoansQueryValidator _overdueLoansValidator;
        private readonly GetUserOverdueLoansQueryValidator _userOverdueLoansValidator;
        private readonly IMediator _mediatr;

        public LoansController(GetAllActiveLoansQueryValidator activeLoansValidator, GetOverdueLoanQueryValidator overdueLoanValidator, 
            ListOverdueLoansQueryValidator overdueLoansValidator, GetUserOverdueLoansQueryValidator userOverdueLoansValidator, IMediator mediatr)
        {
            _activeLoansValidator = activeLoansValidator;
            _overdueLoanValidator = overdueLoanValidator;
            _overdueLoansValidator = overdueLoansValidator;
            _userOverdueLoansValidator = userOverdueLoansValidator;
            _mediatr = mediatr;
        }

        [HttpGet]
        public async  Task<IActionResult> Index([FromBody] GetAllActiveLoansQuery query)
        {
            if (query == null) query = new GetAllActiveLoansQuery();
            var validationResult = _activeLoansValidator.Validate(query);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }
            var (pagination, list) = await _mediatr.Send(query);
            Response.Headers.Add("X-Pagination",
               JsonSerializer.Serialize(pagination));
            return Ok(list);
        }

        [HttpGet("overdue")]
        public async Task<IActionResult> GetOverdue([FromBody] ListOverdueLoansQuery query)
        {
            if (query == null) query = new ListOverdueLoansQuery();
            var validationResult = _overdueLoansValidator.Validate(query);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }
            var (pagination, list) = await _mediatr.Send(query);
            Response.Headers.Add("X-Pagination",
               JsonSerializer.Serialize(pagination));
            return Ok(list);
        }

        [HttpGet("overdue/user")]
        public async Task<IActionResult> GetOverdueByUser([FromBody] GetUserOverdueLoansQuery query)
        {
            if (query == null) query = new GetUserOverdueLoansQuery();
            var validationResult = _userOverdueLoansValidator.Validate(query);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }
            var (pagination, list) = await _mediatr.Send(query);
            if (pagination == null)
                return NotFound();
            Response.Headers.Add("X-Pagination",
               JsonSerializer.Serialize(pagination));
            return Ok(list);
        }

        [HttpGet("overdue/loan")]
        public async Task<IActionResult> GetOverdueLoan([FromBody] GetOverdueLoanQuery query)
        {
            if (query == null) query = new GetOverdueLoanQuery();
            var validationResult = _overdueLoanValidator.Validate(query);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }
            return await _mediatr.Send(query);
        }


    }
}
