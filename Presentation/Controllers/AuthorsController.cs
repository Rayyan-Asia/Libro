using System.Text.Json;
using Application.Entities.Authors.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{
    [Authorize(Policy = "LibrarianRequired")]
    [ApiController]
    [Route("api/[controller]")]
    public class AuthorsController : Controller
    {
        private readonly IMediator _mediator;

        public AuthorsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> Index([FromBody] BrowseAuthorsQuery? browseAuthorsQuery)
        {
            if (browseAuthorsQuery == null) browseAuthorsQuery = new BrowseAuthorsQuery();
            var (pagination, books) = await _mediator.Send(browseAuthorsQuery);
            Response.Headers.Add("X-Pagination",
               JsonSerializer.Serialize(pagination));
            return Ok(books);

        }
    }
}
