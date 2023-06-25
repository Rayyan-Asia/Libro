using Application.DTOs;
using Application.Entities.Books.Commands;
using AutoMapper;
using Domain;
using Application.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

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
        private readonly ILogger<EditBookCommandHandler> _logger;

        public EditBookCommandHandler(IBookRepository bookRepository, IAuthorRepository authorRepository, IGenreRepository genreRepository,
            IBookAuthorRepository bookAuthorRepository, IBookGenreRepository bookGenreRepository, IMapper mapper, ILogger<EditBookCommandHandler> logger)
        {
            _bookRepository = bookRepository;
            _authorRepository = authorRepository;
            _genreRepository = genreRepository;
            _bookAuthorRepository = bookAuthorRepository;
            _bookGenreRepository = bookGenreRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<BookDto> Handle(EditBookCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Retrieving Book with Id {request.Id}");
            var book = await _bookRepository.GetBookByIdAsync(request.Id);
            if (book == null)
            {
                _logger.LogError($"Book not found with ID {request.Id}");
                return null;
            }
                

            if (!await HasValidIds(request))
                return null;

            book.Title = request.Title;
            book.Description = request.Description;
            book.PublicationDate = request.PublicationDate;

            // Make a copy of the authors collection to avoid modifying it while iterating
            book.Authors.Clear();
            _logger.LogInformation($"Clearing authors from book with ID {book.Id}");
            await _bookAuthorRepository.RemoveAuthorsFromBook(book.Id);
            await AddAuthors(request.Authors, book);

            // Make a copy of the genres collection to avoid modifying it while iterating
            book.Genres.Clear();
            _logger.LogInformation($"Clearing genres from book with ID {book.Id}");
            await _bookGenreRepository.RemoveGenresFromBook(book.Id);
            await AddGenres(request.Genres, book);

            _logger.LogInformation($"Updating book with Id {book.Id}");
            await _bookRepository.UpdateBookAsync(book);

            return _mapper.Map<BookDto>(book);
        }

        private async Task AddGenres(List<IdDto> genres, Book book)
        {

            foreach (var genre in genres)
            {
                _logger.LogInformation($"Retrieving genre with Id {genre.Id}");
                var existingGenre = await _genreRepository.GetGenreByIdAsync(genre.Id);
                book.Genres.Add(existingGenre);
            }

        }

        private async Task AddAuthors(List<IdDto> authors, Book book)
        {
            foreach (var author in authors)
            {
                _logger.LogInformation($"Retrieving author with Id {author.Id}");
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
                _logger.LogInformation($"Retrieving author with Id {author.Id}");
                if (!await _authorRepository.AuthorExistsAsync(author.Id))
                {
                    _logger.LogError($"Author NOT FOUND with ID {author.Id}");
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
