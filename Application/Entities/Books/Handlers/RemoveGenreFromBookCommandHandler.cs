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

namespace Application.Entities.Books.Handlers
{
    public class RemoveGenreFromBookCommandHandler : IRequestHandler<RemoveGenreFromBookCommand, BookDto>
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

        public async Task<BookDto> Handle(RemoveGenreFromBookCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Retrieving Book with ID {request.BookId}");
            var book = await _bookRepository.GetBookByIdAsync(request.BookId);
            if (book == null)
            {
                _logger.LogError($"Book not found with ID {request.BookId}");
                return null;
            }

            _logger.LogInformation($"Retrieving genre with Id {request.GenreId}");
            var genre = await _genreRepository.GetGenreByIdAsync(request.GenreId);
            if (genre == null)
            {
                _logger.LogError($"Genre not found with ID {request.GenreId}");
                return null;
            }
                
            if (!book.Genres.Any(g => g.Id == genre.Id))
                return null;
            if (book.Genres.Count() <= 1)
                return null;

            book.Genres.Remove(genre);
            _logger.LogInformation($"Updating book with Id {book.Id}");
            await _bookRepository.UpdateBookAsync(book);

            var bookDto = _mapper.Map<BookDto>(book);
            return bookDto;
        }
    }
}
