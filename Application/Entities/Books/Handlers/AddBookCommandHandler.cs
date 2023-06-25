using Application.DTOs;
using Application.Entities.Books.Commands;
using AutoMapper;
using Domain;
using Application.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Entities.Books.Handlers
{
    public class AddBookCommandHandler : IRequestHandler<AddBookCommand, BookDto>
    {
        private readonly IBookRepository _bookRepository;
        private readonly IAuthorRepository _authorRepository;
        private readonly IGenreRepository _genreRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<AddBookCommandHandler> _logger;

        public AddBookCommandHandler(IBookRepository bookRepository, IAuthorRepository authorRepository, IGenreRepository genreRepository,
            IMapper mapper, ILogger<AddBookCommandHandler> logger)
        {
            _bookRepository = bookRepository;
            _authorRepository = authorRepository;
            _genreRepository = genreRepository;
            _mapper = mapper;
            _logger = logger;
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

            _logger.LogInformation($"Adding Book");
            book = await _bookRepository.AddBookAsync(book);

            await AddAuthorsToBook(request.Authors, book);
            await AddGenresToBook(request.Genres, book);

            _logger.LogInformation($"Updating book with Id {book.Id} with books");
            book = await _bookRepository.UpdateBookAsync(book);

            return _mapper.Map<BookDto>(book);
        }

        private async Task AddGenresToBook(List<IdDto> genres, Book book)
        {
            foreach (var genre in genres)
            {
                _logger.LogInformation($"Retrieving genre with Id {genre.Id}");
                var existingGenre = await _genreRepository.GetGenreByIdAsync(genre.Id);
                book.Genres.Add(existingGenre);
            }
        }

        private async Task AddAuthorsToBook(List<IdDto> authors, Book book)
        {
            foreach (var author in authors)
            {
                _logger.LogInformation($"Retrieving author with Id {author.Id}");
                var existingGenre = await _authorRepository.GetAuthorByIdAsync(author.Id);
                book.Authors.Add(existingGenre);
            }
        }

        async Task<bool> HasValidIds(AddBookCommand request)
        {
            if (request.Authors.Count() == 0 || request.Genres.Count() == 0)
                return false;
            var authors = request.Authors; 
            foreach (var author in authors)
            {
                _logger.LogInformation($"Retrieving author with Id {author.Id}");
                if (!await _authorRepository.AuthorExistsAsync(author.Id))
                {
                    _logger.LogError($"Author not found with ID {author.Id}");
                    return false;
                }
                   
            }
            var genres = request.Genres;
            foreach (var genre in genres)
            {
                _logger.LogInformation($"Retrieving genre with Id {genre.Id}");
                if (!await _genreRepository.GenreExistsAsync(genre.Id))
                {
                    _logger.LogError($"Genre not found with ID {genre.Id}");
                    return false;
                }
            }
            return true;
        }
    }
}
