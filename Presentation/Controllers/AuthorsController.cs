using System;
using System.Text.Json;
using Application.Entities.Authors.Commands;
using Application.Entities.Authors.Queries;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Presentation.Validators.Authors;

namespace Presentation.Controllers
{
    [Authorize(Policy = "LibrarianRequired")]
    [ApiController]
    [Route("api/[controller]")]
    public class AuthorsController : Controller
    {
        private readonly IMediator _mediator;
        private readonly AddAuthorCommandValidator _addAuthorValidator;
        private readonly EditAuthorCommandValidator _editAuthorValidator;
        private readonly AddBookToAuthorCommandValidator _addBookToAuthorValidator;
        private readonly RemoveBookFromAuthorCommandValidator _removeBookFromAuthorValidator;
        private readonly RemoveAuthorCommandValidator _removeAuthorValidator;

        public AuthorsController(IMediator mediator, AddAuthorCommandValidator addAuthorValidator,
            EditAuthorCommandValidator editAuthorValidator, AddBookToAuthorCommandValidator addBookToAuthorValidator, 
            RemoveBookFromAuthorCommandValidator removeBookFromAuthorValidator, RemoveAuthorCommandValidator removeAuthorCommandValidator)
        {
            _mediator = mediator;
            _addAuthorValidator = addAuthorValidator;
            _editAuthorValidator = editAuthorValidator;
            _addBookToAuthorValidator = addBookToAuthorValidator;
            _removeBookFromAuthorValidator = removeBookFromAuthorValidator;
            _removeAuthorValidator = removeAuthorCommandValidator;
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
            
            ValidationResult result = _addAuthorValidator.Validate(addAuthorCommand);

            if (!result.IsValid)
            {
                return BadRequest(result.Errors);
            }

            var author = await _mediator.Send(addAuthorCommand);
            if (author == null) return BadRequest();
            return Ok(author);
        }

        [HttpPost("edit")]
        public async Task<IActionResult> EditAuthor([FromBody] EditAuthorCommand editAuthorCommand)
        {
            if (editAuthorCommand == null) return BadRequest();

            ValidationResult result = _editAuthorValidator.Validate(editAuthorCommand);

            if (!result.IsValid)
            {
                return BadRequest(result.Errors);
            }
            var author = await _mediator.Send(editAuthorCommand);
            if (author == null) return BadRequest();
            return Ok(author);
        }

        [HttpPost("remove/{authorId}")]
        public async Task<IActionResult> RemoveAuthor([FromRoute] int authorId)
        {
            if (authorId == 0) return BadRequest();
            
            var removeAuthorCommand = new RemoveAuthorCommand() {Id = authorId };
            ValidationResult validationResult = _removeAuthorValidator.Validate(removeAuthorCommand);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }
            var result = await _mediator.Send(removeAuthorCommand);
            if (!result) return BadRequest();
            return Ok();
        }



        [HttpPost("add/book")]
        public async Task<IActionResult> AddBookToAuthor([FromBody] AddBookToAuthorCommand addBookToAuthorCommand)
        {
            if (addBookToAuthorCommand == null) return BadRequest();
            ValidationResult validationResult = _addBookToAuthorValidator.Validate(addBookToAuthorCommand);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }
            var author = await _mediator.Send(addBookToAuthorCommand);
            if (author == null) return BadRequest();
            return Ok(author);
        }

        [HttpPost("remove/book")]
        public async Task<IActionResult> RemoveBookFromAuthor([FromBody] RemoveBookFromAuthorCommand removeBookFromAuthorCommand)
        {
            if (removeBookFromAuthorCommand == null) return BadRequest();
            ValidationResult validationResult = _removeBookFromAuthorValidator.Validate(removeBookFromAuthorCommand);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }
            var result = await _mediator.Send(removeBookFromAuthorCommand);
            if (result == null) return BadRequest();
            return Ok(result);
        }


    }
}
