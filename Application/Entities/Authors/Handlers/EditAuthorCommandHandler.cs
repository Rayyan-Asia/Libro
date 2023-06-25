using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTOs;
using Application.Entities.Authors.Commands;
using Application.Interfaces;
using AutoMapper;
using Domain;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Entities.Authors.Handlers
{
    public class EditAuthorCommandHandler : IRequestHandler<EditAuthorCommand, AuthorDto>
    {
        private readonly IAuthorRepository _authorRepository;
        private readonly IBookRepository _bookRepository;
        private readonly IBookAuthorRepository _bookAuthorRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<EditAuthorCommandHandler> _logger;

        public EditAuthorCommandHandler(IAuthorRepository authorRepository, IBookRepository bookRepository,
            IBookAuthorRepository bookAuthorRepository, IMapper mapper, ILogger<EditAuthorCommandHandler> logger)
        {
            _authorRepository = authorRepository;
            _bookRepository = bookRepository;
            _bookAuthorRepository = bookAuthorRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<AuthorDto> Handle(EditAuthorCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Retrieving Author from database with ID {request.Id}");
            var author = await _authorRepository.GetAuthorByIdAsync(request.Id);
            if (author == null) {
                _logger.LogError($"Author NOT FOUND with ID {request.Id}");
                return null;
            }

            _logger.LogInformation($"Clearing Books from author {author.Id}");
            await _bookAuthorRepository.RemoveBooksFromAuthor(author.Id);

            if (request.Books.Count() < 1) return null;
            foreach (var Book in request.Books)
            {
                _logger.LogInformation($"Retrieving book with Id {Book.Id}");
                var book = await _bookRepository.GetBookByIdAsync(Book.Id);
                if (book == null)
                {
                    _logger.LogError($"Book NOT FOUND with Id {Book.Id}");
                    return null;
                }
                    
                author.Books.Add(book);
            }
            author.Name = request.Name;
            author.Description = request.Description;

            _logger.LogInformation($"Updating author {author.Id}");
            author = await _authorRepository.UpdateAuthorAsync(author);
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
