using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTOs;
using Application.Entities.Authors.Commands;
using AutoMapper;
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

            return _mapper.Map<AuthorDto>(author);

        }
    }
}
