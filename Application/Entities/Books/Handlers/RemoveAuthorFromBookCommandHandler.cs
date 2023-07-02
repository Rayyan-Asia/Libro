using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTOs;
using Application.Entities.Books.Commands;
using AutoMapper;
using Application.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;

namespace Application.Entities.Books.Handlers
{
    public class RemoveAuthorFromBookCommandHandler : IRequestHandler<RemoveAuthorFromBookCommand, IActionResult>
    {
        private readonly IBookRepository _bookRepository;
        private readonly IAuthorRepository _authorRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<RemoveAuthorFromBookCommandHandler> _logger;

        public RemoveAuthorFromBookCommandHandler(IBookRepository bookRepository, IAuthorRepository authorRepository,
            IMapper mapper, ILogger<RemoveAuthorFromBookCommandHandler> logger)
        {
            _bookRepository = bookRepository;
            _authorRepository = authorRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IActionResult> Handle(RemoveAuthorFromBookCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Retrieving book with Id {request.BookId}");
            var book = await _bookRepository.GetBookByIdAsync(request.BookId);
            if (book == null)
            {
                _logger.LogError($"Book not found with ID {request.BookId}");
                return new NotFoundObjectResult($"Book not found with ID {request.BookId}"); // Return a 404 Not Found response
            }
            _logger.LogInformation($"Retrieving author with ID {request.AuthorId}");
            var author = await _authorRepository.GetAuthorByIdAsync(request.AuthorId);
            if (author == null)
            {
                _logger.LogError($"Author not found with ID {request.AuthorId}");
                return new NotFoundObjectResult($"Author not found with ID {request.AuthorId}"); // Return a 404 Not Found response
            }

            if (!book.Authors.Any(g => g.Id == author.Id))
                return new BadRequestObjectResult($"Author with ID {author.Id} is not associated with this book"); // Return a 404 Not Found response
            if (book.Authors.Count <= 1)
                return new BadRequestObjectResult("Book Cannot remove its only author"); // Return a 400 Bad Request response

            book.Authors.Remove(author);
            _logger.LogInformation($"Updating book with Id {book.Id}");
            await _bookRepository.UpdateBookAsync(book);

            var bookDto = _mapper.Map<BookDto>(book);
            return new OkObjectResult(bookDto); // Return a 200 OK response with the updated book data
        }
    }
}
