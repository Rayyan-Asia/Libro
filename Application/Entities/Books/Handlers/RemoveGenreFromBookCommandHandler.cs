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
using Domain;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;

namespace Application.Entities.Books.Handlers
{
    public class RemoveGenreFromBookCommandHandler : IRequestHandler<RemoveGenreFromBookCommand, IActionResult>
    {
        private readonly IBookRepository _bookRepository;
        private readonly IGenreRepository _genreRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<RemoveGenreFromBookCommandHandler> _logger;

        public RemoveGenreFromBookCommandHandler(IBookRepository bookRepository, IGenreRepository genreRepository, IMapper mapper, ILogger<RemoveGenreFromBookCommandHandler> logger)
        {
            _bookRepository = bookRepository;
            _genreRepository = genreRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IActionResult> Handle(RemoveGenreFromBookCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Retrieving Book with ID {request.BookId}");
            var book = await _bookRepository.GetBookByIdAsync(request.BookId);
            if (book == null)
            {
                _logger.LogError($"Book not found with ID {request.BookId}");
                return new NotFoundObjectResult($"Book not found with ID {request.BookId}"); // Return a 404 Not Found response
            }

            _logger.LogInformation($"Retrieving genre with Id {request.GenreId}");
            var genre = await _genreRepository.GetGenreByIdAsync(request.GenreId);
            if (genre == null)
            {
                _logger.LogError($"Genre not found with ID {request.GenreId}");
                return new NotFoundObjectResult($"Genre not found with ID {request.GenreId}"); // Return a 404 Not Found response
            }

            if (!book.Genres.Any(g => g.Id == genre.Id))
                return new BadRequestObjectResult($"Genre with ID {genre.Id} is not associated with this book"); // Return a 404 Not Found response
            if (book.Genres.Count() <= 1)
                return new BadRequestObjectResult("Book Cannot remove its only genre"); // Return a 400 Bad Request response

            book.Genres.Remove(genre);
            _logger.LogInformation($"Updating book with Id {book.Id}");
            await _bookRepository.UpdateBookAsync(book);

            var bookDto = _mapper.Map<BookDto>(book);
            return new OkObjectResult(bookDto); // Return a 200 OK response with the updated book data
        }
    }
}
