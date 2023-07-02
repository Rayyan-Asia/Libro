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
    public class AddAuthorToBookCommandHandler : IRequestHandler<AddAuthorToBookCommand, IActionResult>
    {
        private readonly IBookRepository _bookRepository;
        private readonly IAuthorRepository _authorRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<AddAuthorToBookCommandHandler> _logger;

        public AddAuthorToBookCommandHandler(IBookRepository bookRepository, IAuthorRepository authorRepository,
            IMapper mapper, ILogger<AddAuthorToBookCommandHandler> logger)
        {
            _bookRepository = bookRepository;
            _authorRepository = authorRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IActionResult> Handle(AddAuthorToBookCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Retrieving book with Id {request.BookId}");
            var book = await _bookRepository.GetBookByIdAsync(request.BookId);
            if (book == null)
            {
                _logger.LogError($"Book not found with ID {request.BookId}");
                return new NotFoundObjectResult($"Book with ID {request.BookId} was not found.");
            }

            _logger.LogInformation($"Retrieving author with Id {request.AuthorId}");
            var author = await _authorRepository.GetAuthorByIdAsync(request.AuthorId);
            if (author == null)
            {
                _logger.LogError($"Author not found with ID {request.AuthorId}");
                return new NotFoundObjectResult($"Author with ID {request.AuthorId} was not found.");
            }

            if (book.Authors.Any(g => g.Id == author.Id))
                return new BadRequestObjectResult($"Author with ID {request.AuthorId} is already associated with the book.");

            book.Authors.Add(author);
            _logger.LogInformation($"Updating book with Id {request.BookId}");
            await _bookRepository.UpdateBookAsync(book);

            var bookDto = _mapper.Map<BookDto>(book);
            return new OkObjectResult(bookDto);
        }
    }
}
