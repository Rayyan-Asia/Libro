using Application.DTOs;
using Application.Entities.Books.Commands;
using AutoMapper;
using Domain;
using Infrastructure.Interfaces;
using MediatR;

namespace Application.Entities.Books.Handlers
{
    public class EditBookCommandHandler : IRequestHandler<EditBookCommand, BookDto>
    {
        private readonly IBookRepository _bookRepository;
        private readonly IAuthorRepository _authorRepository;
        private readonly IGenreRepository _genreRepository;
        private readonly IBookAuthorRepository _bookAuthorRepository;
        private readonly IBookGenreRepository _bookGenreRepository;
        private readonly IMapper _mapper;

        public EditBookCommandHandler(IBookRepository bookRepository, IAuthorRepository authorRepository, IGenreRepository genreRepository,
            IBookAuthorRepository bookAuthorRepository, IBookGenreRepository bookGenreRepository, IMapper mapper)
        {
            _bookRepository = bookRepository;
            _authorRepository = authorRepository;
            _genreRepository = genreRepository;
            _bookAuthorRepository = bookAuthorRepository;
            _bookGenreRepository = bookGenreRepository;
            _mapper = mapper;
        }

        public async Task<BookDto> Handle(EditBookCommand request, CancellationToken cancellationToken)
        {
            var book = await _bookRepository.GetBookByIdAsync(request.Id);
            if (book == null)
                return null;

            if (!await HasValidIds(request))
                return null;

            book.Title = request.Title;
            book.Description = request.Description;
            book.PublicationDate = request.PublicationDate;

            // Make a copy of the authors collection to avoid modifying it while iterating
            book.Authors.Clear();
            await _bookAuthorRepository.RemoveAuthorsFromBook(book.Id);
            await AddAuthors(request.Authors, book);

            // Make a copy of the genres collection to avoid modifying it while iterating
            book.Genres.Clear();
            await _bookGenreRepository.RemoveGenresFromBook(book.Id);
            await AddGenres(request.Genres, book);

            await _bookRepository.UpdateBookAsync(book);

            return _mapper.Map<BookDto>(book);
        }

        private async Task AddGenres(List<BookGenreDto> genres, Book book)
        {

            foreach (var genre in genres)
            {
                var existingGenre = await _genreRepository.GetGenreByIdAsync(genre.Id);
                book.Genres.Add(existingGenre);
            }

        }

        private async Task AddAuthors(List<BookAuthorDto> authors, Book book)
        {
            foreach (var author in authors)
            {
                var existingGenre = await _authorRepository.GetAuthorByIdAsync(author.Id);
                book.Authors.Add(existingGenre);
            }
            
        }

        async Task<bool> HasValidIds(EditBookCommand request)
        {
            if (request.Authors.Count() == 0 || request.Genres.Count() == 0)
                return false;
            var authors = request.Authors;
            foreach (var author in authors)
            {
                if (!await _authorRepository.AuthorExistsAsync(author.Id))
                    return false;
            }
            var genres = request.Genres;
            foreach (var genre in genres)
            {
                if (!await _genreRepository.GenreExistsAsync(genre.Id))
                    return false;
            }
            return true;
        }
        
    }
}
