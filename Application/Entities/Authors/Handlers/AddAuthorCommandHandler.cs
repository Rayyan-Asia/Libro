using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTOs;
using Application.Entities.Authors.Commands;
using AutoMapper;
using Domain;
using Infrastructure.Interfaces;
using MediatR;

namespace Application.Entities.Authors.Handlers
{
    public class AddAuthorCommandHandler : IRequestHandler<AddAuthorCommand,AuthorDto>
    {
        private readonly IAuthorRepository _authorRepository;
        private readonly IBookRepository _bookRepository;
        private readonly IMapper _mapper;

        public AddAuthorCommandHandler(IAuthorRepository authorRepository, IBookRepository bookRepository, IMapper mapper)
        {
            _authorRepository = authorRepository;
            _bookRepository = bookRepository;
            _mapper = mapper;
        }

        public async Task<AuthorDto> Handle(AddAuthorCommand request, CancellationToken cancellationToken)
        {
            var author = new Author();
            if (request.Books.Count() < 1) return null;
            foreach(var Book in request.Books)
            {
                var book = await _bookRepository.GetBookByIdAsync(Book.Id);
                if (book == null)
                    return null;
                author.Books.Add(book);
            }
            author.Name = request.Name;
            author.Description = request.Description;

            author  = await _authorRepository.AddAuthorAsync(author);

            return _mapper.Map<AuthorDto>(author);
        }
    }
}
