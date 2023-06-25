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
    public class AddAuthorCommandHandler : IRequestHandler<AddAuthorCommand,AuthorDto>
    {
        private readonly IAuthorRepository _authorRepository;
        private readonly IBookRepository _bookRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<AddAuthorCommandHandler> _logger;


        public AddAuthorCommandHandler(IAuthorRepository authorRepository, IBookRepository bookRepository, IMapper mapper, ILogger<AddAuthorCommandHandler> logger)
        {
            _authorRepository = authorRepository;
            _bookRepository = bookRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<AuthorDto> Handle(AddAuthorCommand request, CancellationToken cancellationToken)
        {
            var author = new Author();
            if (request.Books.Count() < 1) return null;
            foreach(var Book in request.Books)
            {
                _logger.LogInformation($"Book with id {Book.Id} being retrieved");
                var book = await _bookRepository.GetBookByIdAsync(Book.Id);
                if (book == null)
                {
                    _logger.LogError($"Book with id {Book.Id} not found!!");
                    return null;
                }
                    
                author.Books.Add(book);
            }
            author.Name = request.Name;
            author.Description = request.Description;

            author  = await _authorRepository.AddAuthorAsync(author);
            _logger.LogInformation($"Author: {author.Name} added");
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
