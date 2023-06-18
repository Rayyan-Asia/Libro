using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Entities.Authors.Commands;
using Infrastructure.Interfaces;
using MediatR;

namespace Application.Entities.Authors.Handlers
{
    public class RemoveAuthorCommandHandler :IRequestHandler<RemoveAuthorCommand,bool>
    {
        private readonly IAuthorRepository _authorRepository;
        private readonly IBookAuthorRepository _bookAuthorRepository;

        public RemoveAuthorCommandHandler(IAuthorRepository authorRepository, IBookAuthorRepository bookAuthorRepository)
        {
            _authorRepository = authorRepository;
            _bookAuthorRepository = bookAuthorRepository;
        }

        public async Task<bool> Handle(RemoveAuthorCommand request, CancellationToken cancellationToken)
        {
            var author =await  _authorRepository.GetAuthorByIdAsync(request.Id);
            if (author == null) { return false; }
            var bookList = await _bookAuthorRepository.GetAuthorBooksAsync(author.Id);
            foreach (var book in bookList)
            {
                if (await _bookAuthorRepository.GetBookAuthorsCountAsync(book.BookId) <= 1)
                    return false;
            }

            await _authorRepository.RemoveAuthorAsync(author);
            return true;
        }
    }
}
