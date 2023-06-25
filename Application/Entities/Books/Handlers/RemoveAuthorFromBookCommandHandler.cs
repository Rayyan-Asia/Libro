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
    public class RemoveAuthorFromBookCommandHandler : IRequestHandler<RemoveAuthorFromBookCommand, BookDto>
    {
        private readonly IBookRepository _bookRepository;
        private readonly IAuthorRepository _authorRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<RemoveAuthorFromBookCommandHandler> _logger;

        public RemoveAuthorFromBookCommandHandler(IBookRepository bookRepository, IAuthorRepository authorRepository,
            IMapper mapper, ILogger<RemoveAuthorFromBookCommandHandler> logger)
        {
            _bookRepository = bookRepository;
            _authorRepository = authorRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<BookDto> Handle(RemoveAuthorFromBookCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Retrieving book with Id {request.BookId}");
            var book = await _bookRepository.GetBookByIdAsync(request.BookId);
            if (book == null)
            {
                _logger.LogError($"Book not found with ID {request.BookId}");
                return null;
            }
            _logger.LogInformation($"Retrieving author with ID {request.AuthorId}");
            var author = await _authorRepository.GetAuthorByIdAsync(request.AuthorId);
            if (author == null)
            {
                _logger.LogError($"Author not found with ID {request.AuthorId}");
                return null;
            }
                
            if (!book.Authors.Any(g => g.Id == author.Id))
                return null;
            if(book.Authors.Count <=1)
                return null;

            book.Authors.Remove(author);
            _logger.LogInformation($"Updating book with Id {book.Id}");
            await _bookRepository.UpdateBookAsync(book);

            var bookDto = _mapper.Map<BookDto>(book);
            return bookDto;
        }
    }
}
