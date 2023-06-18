using System.Text.Json;
using Application.Entities.Books.Commands;
using Application.Entities.Books.Queries;
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

        [HttpGet("search")]
        public async Task<IActionResult> Search([FromBody] SearchQuery searchQuery)
        {
            if (searchQuery == null) searchQuery = new SearchQuery();
            var (pagination, books) = await _mediator.Send(searchQuery);
            Response.Headers.Add("X-Pagination",
               JsonSerializer.Serialize(pagination));

            return Ok(books);

        }


        [HttpPost("add")]
        public async Task<IActionResult> Add([FromBody] AddBookCommand addBookCommand)
        {
            if (addBookCommand == null) return BadRequest();
            var book = await _mediator.Send(addBookCommand);
            if (book == null) return BadRequest();
            return Ok(book);
        }

        [HttpPost("edit")]
        public async Task<IActionResult> Edit([FromBody] EditBookCommand editBookCommand)
        {
            if (editBookCommand == null) return BadRequest();
            var book = await _mediator.Send(editBookCommand);
            if (book == null) return BadRequest();
            return Ok(book);
        }

    }
}
