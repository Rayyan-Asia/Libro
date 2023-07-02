using System.Text.Json;
using Application.Entities.Authors.Commands;
using Application.Entities.Books.Commands;
using Application.Entities.Books.Handlers;
using Application.Entities.Books.Queries;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Presentation.Validators.Books;

namespace Presentation.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class BooksController : Controller
    {

        private readonly IMediator _mediator;
        private readonly SearchQueryValidator _searchQueryValidator;
        private readonly AddBookCommandValidator _addBookCommandValidator;
        private readonly EditBookCommandValidator _editBookCommandValidator;
        private readonly AddAuthorToBookCommandValidator _addAuthorToBookCommandValidator;
        private readonly RemoveAuthorFromBookCommandValidator _removeAuthorFromBookCommandValidator;
        private readonly AddGenreToBookCommandValidator _addGenreToBookCommandValidator;
        private readonly RemoveGenreFromBookCommandValidator _removeGenreFromBookCommandValidator;
        private readonly RemoveBookCommandValidator _removeBookCommandValidator;

        public BooksController(IMediator mediator, SearchQueryValidator searchQueryValidator, AddBookCommandValidator addBookCommandValidator, 
            EditBookCommandValidator editBookCommandValidator, AddAuthorToBookCommandValidator addAuthorToBookCommandValidator, 
            RemoveAuthorFromBookCommandValidator removeAuthorFromBookCommandValidator, AddGenreToBookCommandValidator addGenreToBookCommandValidator,
            RemoveGenreFromBookCommandValidator removeGenreFromBookCommandValidator, RemoveBookCommandValidator removeBookCommandValidator)
        {
            _mediator = mediator;
            _searchQueryValidator = searchQueryValidator;
            _addBookCommandValidator = addBookCommandValidator;
            _editBookCommandValidator = editBookCommandValidator;
            _addAuthorToBookCommandValidator = addAuthorToBookCommandValidator;
            _removeAuthorFromBookCommandValidator = removeAuthorFromBookCommandValidator;
            _addGenreToBookCommandValidator = addGenreToBookCommandValidator;
            _removeGenreFromBookCommandValidator = removeGenreFromBookCommandValidator;
            _removeBookCommandValidator = removeBookCommandValidator;
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
            ValidationResult validationResult = _searchQueryValidator.Validate(searchQuery);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }
            var (pagination, books) = await _mediator.Send(searchQuery);
            Response.Headers.Add("X-Pagination",
               JsonSerializer.Serialize(pagination));

            return Ok(books);

        }


        [HttpPost("add")]
        public async Task<IActionResult> Add([FromBody] AddBookCommand addBookCommand)
        {
            if (addBookCommand == null) return BadRequest();
            ValidationResult validationResult = _addBookCommandValidator.Validate(addBookCommand);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }
            return await _mediator.Send(addBookCommand);
        }

        [HttpPost("edit")]
        public async Task<IActionResult> Edit([FromBody] EditBookCommand editBookCommand)
        {
            if (editBookCommand == null) return BadRequest();
            ValidationResult validationResult = _editBookCommandValidator.Validate(editBookCommand);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }
            return await _mediator.Send(editBookCommand);
        }


        [HttpPost("add/author")]
        public async Task<IActionResult> AddAuthorToBook([FromBody] AddAuthorToBookCommand addAuthorCommand)
        {
            if (addAuthorCommand == null) return BadRequest();
            ValidationResult validationResult = _addAuthorToBookCommandValidator.Validate(addAuthorCommand);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }
            return await _mediator.Send(addAuthorCommand);
            
        }

        [HttpPost("remove/author")]
        public async Task<IActionResult> Edit([FromBody] RemoveAuthorFromBookCommand removeAuthorCommand)
        {
            if (removeAuthorCommand == null) return BadRequest();
            ValidationResult validationResult = _removeAuthorFromBookCommandValidator.Validate(removeAuthorCommand);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }
            return await _mediator.Send(removeAuthorCommand);
        }


        [HttpPost("add/genre")]
        public async Task<IActionResult> AddGenreToBook([FromBody] AddGenreToBookCommand addGenreCommand)
        {
            if (addGenreCommand == null) return BadRequest();
            ValidationResult validationResult = _addGenreToBookCommandValidator.Validate(addGenreCommand);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }
            return await _mediator.Send(addGenreCommand);
        }

        [HttpPost("remove/genre")]
        public async Task<IActionResult> RemoveGenreFromBook([FromBody] RemoveGenreFromBookCommand removeGenreCommand)
        {
            if (removeGenreCommand == null) return BadRequest();
            ValidationResult validationResult = _removeGenreFromBookCommandValidator.Validate(removeGenreCommand);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }
            return await _mediator.Send(removeGenreCommand);
        }

        [HttpPost("remove/{bookId}")]
        public async Task<IActionResult> RemoveBook([FromRoute] int bookId)
        {
            if (bookId <= 0)
                return BadRequest();
            RemoveBookCommand removeBookCommand = new RemoveBookCommand() {BookId = bookId };
            ValidationResult validationResult = _removeBookCommandValidator.Validate(removeBookCommand);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }
            return await _mediator.Send(removeBookCommand);   
        }
    }
}
