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
    public class EditAuthorCommandHandler : IRequestHandler<EditAuthorCommand, AuthorDto>
    {
        private readonly IAuthorRepository _authorRepository;
        private readonly IBookRepository _bookRepository;
        private readonly IBookAuthorRepository _bookAuthorRepository;
        private readonly IMapper _mapper;

        public EditAuthorCommandHandler(IAuthorRepository authorRepository, IBookRepository bookRepository, IBookAuthorRepository bookAuthorRepository, IMapper mapper)
        {
            _authorRepository = authorRepository;
            _bookRepository = bookRepository;
            _bookAuthorRepository = bookAuthorRepository;
            _mapper = mapper;
        }

        public async Task<AuthorDto> Handle(EditAuthorCommand request, CancellationToken cancellationToken)
        {
            var author = await _authorRepository.GetAuthorByIdAsync(request.Id);
            if (author == null) return null;

            await _bookAuthorRepository.RemoveBooksFromAuthor(author.Id);

            if (request.Books.Count() < 1) return null;
            foreach (var Book in request.Books)
            {
                var book = await _bookRepository.GetBookByIdAsync(Book.Id);
                if (book == null)
                    return null;
                author.Books.Add(book);
            }
            author.Name = request.Name;
            author.Description = request.Description;

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
