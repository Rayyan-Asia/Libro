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
            if (browseBooksQuery == null) browseBooksQuery = new BrowseBooksQuery();
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


        [HttpPost("add/author")]
        public async Task<IActionResult> AddAuthorToBook([FromBody] AddAuthorToBookCommand addAuthorCommand)
        {
            if (addAuthorCommand == null) return BadRequest();
            var book = await _mediator.Send(addAuthorCommand);
            if (book == null) return BadRequest();
            return Ok(book);
        }

        [HttpPost("remove/author")]
        public async Task<IActionResult> Edit([FromBody] RemoveAuthorFromBookCommand removeAuthorCommand)
        {
            if (removeAuthorCommand == null) return BadRequest();
            var book = await _mediator.Send(removeAuthorCommand);
            if (book == null) return BadRequest();
            return Ok(book);
        }


        [HttpPost("add/genre")]
        public async Task<IActionResult> AddGenreToBook([FromBody] AddGenreToBookCommand addGenreCommand)
        {
            if (addGenreCommand == null) return BadRequest();
            var book = await _mediator.Send(addGenreCommand);
            if (book == null) return BadRequest();
            return Ok(book);
        }

        [HttpPost("remove/genre")]
        public async Task<IActionResult> RemoveGenreFromBook([FromBody] RemoveGenreFromBookCommand removeGenreCommand)
        {
            if (removeGenreCommand == null) return BadRequest();
            var book = await _mediator.Send(removeGenreCommand);
            if (book == null) return BadRequest();
            return Ok(book);
        }

        [HttpPost("remove/{bookId}")]
        public async Task<IActionResult> RemoveBook([FromRoute] int bookId)
        {
            if (bookId == 0)
                return BadRequest();
            RemoveBookCommand removeBookCommand = new RemoveBookCommand() {BookId = bookId };
            var book = await _mediator.Send(removeBookCommand);
            if (book == false) return BadRequest();
            return Ok();
        }



    }
}
