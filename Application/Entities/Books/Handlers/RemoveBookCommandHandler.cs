using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Entities.Books.Commands;
using Application.Interfaces;
using MediatR;

namespace Application.Entities.Books.Handlers
{
    public class RemoveBookCommandHandler : IRequestHandler<RemoveBookCommand, bool>
    {
        private readonly IBookRepository _bookRepository;

        public RemoveBookCommandHandler(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }

        public async Task<bool> Handle(RemoveBookCommand request, CancellationToken cancellationToken)
        {
            var book = await _bookRepository.GetBookByIdAsync(request.BookId);
            if (book == null)
            {
                return false;
            }
            await _bookRepository.RemoveBookAsync(book);
            return true;
        }
    }
}
