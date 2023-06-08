using System.Text.Json;
using Application.Books.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class BooksController : Controller
    {

        private readonly IMediator _mediator;

        public BooksController(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        [HttpGet]
        public async Task<IActionResult> Index([FromBody] BrowseBooksQuery? browseBooksQuery)
        {
            if(browseBooksQuery == null) browseBooksQuery = new BrowseBooksQuery();
            var (pagination, books) = await _mediator.Send(browseBooksQuery);
            Response.Headers.Add("X-Pagination",
               JsonSerializer.Serialize(pagination));

            return Ok(books);

        }
    }
}
