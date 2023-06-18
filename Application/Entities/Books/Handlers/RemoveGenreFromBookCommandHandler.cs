using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTOs;
using Application.Entities.Books.Commands;
using AutoMapper;
using Infrastructure.Interfaces;
using MediatR;

namespace Application.Entities.Books.Handlers
{
    public class RemoveGenreFromBookCommandHandler : IRequestHandler<RemoveGenreFromBookCommand, BookDto>
    {
        private readonly IBookRepository _bookRepository;
        private readonly IGenreRepository _genreRepository;
        private readonly IMapper _mapper;

        public RemoveGenreFromBookCommandHandler(IBookRepository bookRepository, IGenreRepository genreRepository, IMapper mapper)
        {
            _bookRepository = bookRepository;
            _genreRepository = genreRepository;
            _mapper = mapper;
        }

        public async Task<BookDto> Handle(RemoveGenreFromBookCommand request, CancellationToken cancellationToken)
        {
            var book = await _bookRepository.GetBookByIdAsync(request.BookId);
            var genre = await _genreRepository.GetGenreByIdAsync(request.GenreId);
            if (book == null || genre == null)
                return null;
            if (!book.Genres.Any(g => g.Id == genre.Id))
                return null;
            if (book.Genres.Count() <= 1)
                return null;

            book.Genres.Remove(genre);
            await _bookRepository.UpdateBookAsync(book);

            var bookDto = _mapper.Map<BookDto>(book);
            return bookDto;
        }
    }
}
