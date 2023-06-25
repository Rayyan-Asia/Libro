using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Entities.Books.Commands;
using Application.Interfaces;
using Domain;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Entities.Books.Handlers
{
    public class RemoveBookCommandHandler : IRequestHandler<RemoveBookCommand, bool>
    {
        private readonly IBookRepository _bookRepository;
        private readonly ILogger<RemoveBookCommandHandler> _logger;

        public RemoveBookCommandHandler(IBookRepository bookRepository, ILogger<RemoveBookCommandHandler> logger)
        {
            _bookRepository = bookRepository;
            _logger = logger;
        }

        public async Task<bool> Handle(RemoveBookCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Retrieving book with Id {request.BookId}");
            var book = await _bookRepository.GetBookByIdAsync(request.BookId);
            if (book == null)
            {
                _logger.LogError($"Book not found with ID {request.BookId}");
                return false;
            }
            _logger.LogInformation($"Removing Book with ID {book.Id}");
            await _bookRepository.RemoveBookAsync(book);
            return true;
        }
    }
}
