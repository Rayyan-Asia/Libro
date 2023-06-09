﻿using System;
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
    public class AddGenreToBookCommandHandler : IRequestHandler<AddGenreToBookCommand,IActionResult>
    {
        private readonly IBookRepository _bookRepository;
        private readonly IGenreRepository _genreRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<AddGenreToBookCommandHandler> _logger;

        public AddGenreToBookCommandHandler(IBookRepository bookRepository, IGenreRepository genreRepository, IMapper mapper, ILogger<AddGenreToBookCommandHandler> logger)
        {
            _bookRepository = bookRepository;
            _genreRepository = genreRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IActionResult> Handle(AddGenreToBookCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Retrieving book with Id {request.BookId}");
            var book = await _bookRepository.GetBookByIdAsync(request.BookId);
            if (book == null)
            {
                _logger.LogError($"Book not found with ID {request.BookId}");
                return new NotFoundObjectResult("Book not found.");
            }

            _logger.LogInformation($"Retrieving genre with Id {request.GenreId}");
            var genre = await _genreRepository.GetGenreByIdAsync(request.GenreId);
            if (genre == null)
            {
                _logger.LogError($"Genre not found with ID {request.GenreId}");
                return new NotFoundObjectResult("Genre not found.");
            }

            if (book.Genres.Any(g => g.Id == genre.Id))
            {
                return new BadRequestObjectResult("Genre is already associated with the book.");
            }

            book.Genres.Add(genre);
            _logger.LogInformation($"Updating book with Id {book.Id} with new genre");
            await _bookRepository.UpdateBookAsync(book);

            var bookDto = _mapper.Map<BookDto>(book);
            return new OkObjectResult(bookDto);
        }
    }
}
