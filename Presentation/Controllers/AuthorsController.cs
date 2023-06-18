using System.Text.Json;
using Application.Entities.Authors.Commands;
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

        [HttpPost("add")]
        public async Task<IActionResult> AddAuthor([FromBody] AddAuthorCommand addAuthorCommand)
        {
            if(addAuthorCommand == null) return BadRequest();
            var author = await _mediator.Send(addAuthorCommand);
            if (author == null) return BadRequest();
            return Ok(author);
        }

        [HttpPost("edit")]
        public async Task<IActionResult> EditAuthor([FromBody] EditAuthorCommand editAuthorCommand)
        {
            if (editAuthorCommand == null) return BadRequest();
            var author = await _mediator.Send(editAuthorCommand);
            if (author == null) return BadRequest();
            return Ok(author);
        }

        [HttpPost("remove/{authorId}")]
        public async Task<IActionResult> EditAuthor([FromRoute] int authorId)
        {
            if (authorId == 0) return BadRequest();
            var removeAuthorCommand = new RemoveAuthorCommand() {Id = authorId };
            var result = await _mediator.Send(removeAuthorCommand);
            if (!result) return BadRequest();
            return Ok();
        }



        [HttpPost("add/book")]
        public async Task<IActionResult> AddBookToAuthor([FromBody] AddBookToAuthorCommand addBookToAuthorCommand)
        {
            if (addBookToAuthorCommand == null) return BadRequest();
            var author = await _mediator.Send(addBookToAuthorCommand);
            if (author == null) return BadRequest();
            return Ok(author);
        }

        [HttpPost("remove/book")]
        public async Task<IActionResult> RemoveBookFromAuthor([FromBody] RemoveBookFromAuthorCommand removeBookFromAuthorCommand)
        {
            if (removeBookFromAuthorCommand == null) return BadRequest();
            var result = await _mediator.Send(removeBookFromAuthorCommand);
            if (result == null) return BadRequest();
            return Ok(result);
        }


    }
}
