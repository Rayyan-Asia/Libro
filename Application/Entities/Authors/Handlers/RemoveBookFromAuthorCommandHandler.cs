﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTOs;
using Application.Entities.Authors.Commands;
using AutoMapper;
using Domain;
using Application.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;

namespace Application.Entities.Authors.Handlers
{
    public class RemoveBookFromAuthorCommandHandler : IRequestHandler<RemoveBookFromAuthorCommand,IActionResult>
    {
        private readonly IAuthorRepository _authorRepository;
        private readonly IBookRepository _bookRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<RemoveBookFromAuthorCommandHandler> _logger;

        public RemoveBookFromAuthorCommandHandler(IAuthorRepository authorRepository, IBookRepository bookRepository, IMapper mapper, ILogger<RemoveBookFromAuthorCommandHandler> logger)
        {
            _authorRepository = authorRepository;
            _bookRepository = bookRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IActionResult> Handle(RemoveBookFromAuthorCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Retrieving author with Id {request.AuthorId}");
            var author = await _authorRepository.GetAuthorByIdIncludingCollectionsAsync(request.AuthorId);
            if (author == null)
            {
                _logger.LogError($"Author not found with ID {request.AuthorId}");
                return new NotFoundObjectResult($"Author with ID {request.AuthorId} was not found.");
            }

            _logger.LogInformation($"Retrieving book with Id {request.BookId}");
            var book = await _bookRepository.GetBookByIdAsync(request.BookId);
            if (book == null)
            {
                _logger.LogError($"Book not found with ID {request.BookId}");
                return new NotFoundObjectResult($"Book with ID {request.BookId} was not found.");
            }

            if (!author.Books.Any(g => g.Id == book.Id))
                return new BadRequestObjectResult($"Book with ID {request.BookId} is not associated with the author.");

            author.Books.Remove(book);
            _logger.LogInformation($"Updating Author with Id {request.BookId}");
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
