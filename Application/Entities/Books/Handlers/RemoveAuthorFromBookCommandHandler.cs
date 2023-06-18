using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTOs;
using Application.Entities.Books.Commands;
using AutoMapper;
using Infrastructure.Interfaces;
using MediatR;

namespace Application.Entities.Books.Handlers
{
    public class RemoveAuthorFromBookCommandHandler : IRequestHandler<RemoveAuthorFromBookCommand, BookDto>
    {
        private readonly IBookRepository _bookRepository;
        private readonly IAuthorRepository _authorRepository;
        private readonly IMapper _mapper;

        public RemoveAuthorFromBookCommandHandler(IBookRepository bookRepository, IAuthorRepository authorRepository, IMapper mapper)
        {
            _bookRepository = bookRepository;
            _authorRepository = authorRepository;
            _mapper = mapper;
        }

        public async Task<BookDto> Handle(RemoveAuthorFromBookCommand request, CancellationToken cancellationToken)
        {
            var book = await _bookRepository.GetBookByIdAsync(request.BookId);
            var author = await _authorRepository.GetAuthorByIdAsync(request.AuthorId);
            if (book == null || author == null)
                return null;
            if (!book.Authors.Any(g => g.Id == author.Id))
                return null;
            if(book.Authors.Count <=1)
                return null;

            book.Authors.Remove(author);
            await _bookRepository.UpdateBookAsync(book);

            var bookDto = _mapper.Map<BookDto>(book);
            return bookDto;
        }
    }
}
