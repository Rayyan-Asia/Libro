﻿using System;
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
    public class RemoveBookFromAuthorCommandHandler : IRequestHandler<RemoveBookFromAuthorCommand,AuthorDto>
    {
        private readonly IAuthorRepository _authorRepository;
        private readonly IBookRepository _bookRepository;
        private readonly IMapper _mapper;

        public RemoveBookFromAuthorCommandHandler(IAuthorRepository authorRepository, IBookRepository bookRepository, IMapper mapper)
        {
            _authorRepository = authorRepository;
            _bookRepository = bookRepository;
            _mapper = mapper;
        }

        public async Task<AuthorDto> Handle(RemoveBookFromAuthorCommand request, CancellationToken cancellationToken)
        {
            var author = await _authorRepository.GetAuthorByIdIncludingCollectionsAsync(request.AuthorId);
            var book = await _bookRepository.GetBookByIdAsync(request.BookId);
            if (book == null || author == null)
                return null;
            if (!author.Books.Any(g => g.Id == book.Id))
                return null;
            author.Books.Remove(book);
            await _authorRepository.UpdateAuthorAsync(author);
            return _mapper.Map<AuthorDto>(author);
        }
    }
}
