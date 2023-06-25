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

namespace Application.Entities.Books.Handlers
{
    public class AddGenreToBookCommandHandler : IRequestHandler<AddGenreToBookCommand,BookDto>
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

        public async Task<BookDto> Handle(AddGenreToBookCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Retrieving book with Id {request.BookId}");
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
            if (book.Genres.Any(g => g.Id == genre.Id))
                return null;
            book.Genres.Add(genre);
            _logger.LogInformation($"Updating book with Id {book.Id} with new genre");
            await _bookRepository.UpdateBookAsync(book);

            var bookDto = _mapper.Map<BookDto>(book);
            return bookDto;
        }
    }
}
