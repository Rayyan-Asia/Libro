using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTOs;
using Application.Entities.Authors.Commands;
using AutoMapper;
using Domain;
using Application.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Entities.Authors.Handlers
{
    public class RemoveBookFromAuthorCommandHandler : IRequestHandler<RemoveBookFromAuthorCommand,AuthorDto>
    {
        private readonly IAuthorRepository _authorRepository;
        private readonly IBookRepository _bookRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<RemoveBookFromAuthorCommandHandler> _logger;

        public RemoveBookFromAuthorCommandHandler(IAuthorRepository authorRepository, IBookRepository bookRepository, IMapper mapper, ILogger<RemoveBookFromAuthorCommandHandler> logger)
        {
            _authorRepository = authorRepository;
            _bookRepository = bookRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<AuthorDto> Handle(RemoveBookFromAuthorCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Retrieving author with Id {request.AuthorId}");
            var author = await _authorRepository.GetAuthorByIdIncludingCollectionsAsync(request.AuthorId);
            if (author == null)
            {
                _logger.LogError($"Author not found with ID {request.AuthorId}");
                return null;
            }
            _logger.LogInformation($"Retrieving book with Id {request.BookId}");
            var book = await _bookRepository.GetBookByIdAsync(request.BookId);
            if (book == null)
            {
                _logger.LogError($"Book not found with ID {request.BookId}");
                return null;
            }
                
            if (!author.Books.Any(g => g.Id == book.Id))
                return null;
            author.Books.Remove(book);
            _logger.LogInformation($"Updating Author with Id {request.BookId}");
            await _authorRepository.UpdateAuthorAsync(author);
            author.Books = author.Books.Select(b => new Book
            {
                Id = b.Id,
                Title = b.Title,
                Description = b.Description,
                PublicationDate = b.PublicationDate,
            }).ToList();
            return _mapper.Map<AuthorDto>(author);
        }
    }
}
