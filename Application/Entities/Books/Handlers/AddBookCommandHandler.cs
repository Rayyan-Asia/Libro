using Application.DTOs;
using Application.Entities.Books.Commands;
using AutoMapper;
using Domain;
using Application.Interfaces;
using MediatR;

namespace Application.Entities.Books.Handlers
{
    public class AddBookCommandHandler : IRequestHandler<AddBookCommand, BookDto>
    {
        private readonly IBookRepository _bookRepository;
        private readonly IAuthorRepository _authorRepository;
        private readonly IGenreRepository _genreRepository;
        private readonly IMapper _mapper;

        public AddBookCommandHandler(IBookRepository bookRepository, IAuthorRepository authorRepository, IGenreRepository genreRepository, IMapper mapper)
        {
            _bookRepository = bookRepository;
            _authorRepository = authorRepository;
            _genreRepository = genreRepository;
            _mapper = mapper;
        }

        public async Task<BookDto> Handle(AddBookCommand request, CancellationToken cancellationToken)
        {
            if (!await HasValidIds(request)) return null;

            if(request.Authors.Count() <1 || request.Genres.Count() < 1)
                return null;

            var book = new Book()
            {
                Title = request.Title,
                Description = request.Description,
                PublicationDate = request.PublicationDate,
            };

            book = await _bookRepository.AddBookAsync(book);

            await AddAuthorsToBook(request.Authors, book);
            await AddGenresToBook(request.Genres, book);

            book = await _bookRepository.UpdateBookAsync(book);

            return _mapper.Map<BookDto>(book);
        }

        private async Task AddGenresToBook(List<IdDto> genres, Book book)
        {
            foreach (var genre in genres)
            {

                var existingGenre = await _genreRepository.GetGenreByIdAsync(genre.Id);
                book.Genres.Add(existingGenre);
            }
        }

        private async Task AddAuthorsToBook(List<IdDto> authors, Book book)
        {
            foreach (var author in authors)
            {
                var existingGenre = await _authorRepository.GetAuthorByIdAsync(author.Id);
                book.Authors.Add(existingGenre);
            }
        }

        async Task<bool> HasValidIds(AddBookCommand request)
        {
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
