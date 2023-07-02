using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTOs;
using Application.Entities.Authors.Commands;
using Application.Interfaces;
using AutoMapper;
using Domain;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Application.Entities.Authors.Handlers
{
    public class AddAuthorCommandHandler : IRequestHandler<AddAuthorCommand,IActionResult>
    {
        private readonly IAuthorRepository _authorRepository;
        private readonly IBookRepository _bookRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<AddAuthorCommandHandler> _logger;


        public AddAuthorCommandHandler(IAuthorRepository authorRepository, IBookRepository bookRepository, IMapper mapper, ILogger<AddAuthorCommandHandler> logger)
        {
            _authorRepository = authorRepository;
            _bookRepository = bookRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IActionResult> Handle(AddAuthorCommand request, CancellationToken cancellationToken)
        {
            var author = new Author();
            foreach (var bookRequest in request.Books)
            {
                _logger.LogInformation($"Book with id {bookRequest.Id} being retrieved");
                var book = await _bookRepository.GetBookByIdAsync(bookRequest.Id);
                if (book == null)
                {
                    _logger.LogError($"Book with id {bookRequest.Id} not found!!");
                    // Return NotFoundObjectResult if any book in the request is not found.
                    return new NotFoundObjectResult($"Book with id {bookRequest.Id} not found.");
                }

                author.Books.Add(book);
            }

            author.Name = request.Name;
            author.Description = request.Description;

            author = await _authorRepository.AddAuthorAsync(author);
            _logger.LogInformation($"Author: {author.Name} added");

            // Map the Author entity to AuthorDto and return as OkObjectResult.
            var authorDto = _mapper.Map<AuthorDto>(author);
            return new OkObjectResult(authorDto);
        }
    }
}
