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
    public class AddBookToAuthorCommandHandler : IRequestHandler<AddBookToAuthorCommand, IActionResult>
    {
        private readonly IAuthorRepository _authorRepository;
        private readonly IBookRepository _bookRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<AddBookToAuthorCommandHandler> _logger;

        public AddBookToAuthorCommandHandler(IAuthorRepository authorRepository, IBookRepository bookRepository, IMapper mapper, ILogger<AddBookToAuthorCommandHandler> logger)
        {
            _authorRepository = authorRepository;
            _bookRepository = bookRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IActionResult> Handle(AddBookToAuthorCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Retrieving author with Id {request.AuthorId}");
            var author = await _authorRepository.GetAuthorByIdIncludingCollectionsAsync(request.AuthorId);
            if (author == null)
            {
                _logger.LogError($"Author Was NOT FOUND with Id {request.AuthorId}");
                return new NotFoundObjectResult($"Author with Id {request.AuthorId} was not found.");
            }

            _logger.LogInformation($"Retrieving book with Id {request.BookId}");
            var book = await _bookRepository.GetBookByIdAsync(request.BookId);
            if (book == null)
            {
                _logger.LogError($"Book Was Not Found with Id {request.BookId}");
                return new NotFoundObjectResult($"Book with Id {request.BookId} was not found.");
            }

            if (author.Books.Any(g => g.Id == book.Id))
            {
                // If the book is already in the author's collection, return a BadRequest result.
                return new BadRequestObjectResult($"Book with Id {request.BookId} is already in the author's collection.");
            }

            author.Books.Add(book);
            _logger.LogInformation($"Adding Book to author's collection and updating author");
            await _authorRepository.UpdateAuthorAsync(author);
            author.Books = author.Books.Select(b => new Book
            {
                Id = b.Id,
                Title = b.Title,
                Description = b.Description,
                PublicationDate = b.PublicationDate,
            }).ToList();
            return new OkObjectResult(_mapper.Map<AuthorDto>(author));
        }
    }
}
