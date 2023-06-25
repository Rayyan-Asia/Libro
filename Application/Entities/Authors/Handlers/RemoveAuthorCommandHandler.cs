using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Entities.Authors.Commands;
using Application.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Entities.Authors.Handlers
{
    public class RemoveAuthorCommandHandler :IRequestHandler<RemoveAuthorCommand,bool>
    {
        private readonly IAuthorRepository _authorRepository;
        private readonly IBookAuthorRepository _bookAuthorRepository;
        private readonly ILogger<RemoveAuthorCommandHandler> _logger;
        public RemoveAuthorCommandHandler(IAuthorRepository authorRepository, IBookAuthorRepository bookAuthorRepository, ILogger<RemoveAuthorCommandHandler> logger)
        {
            _authorRepository = authorRepository;
            _bookAuthorRepository = bookAuthorRepository;
            _logger = logger;
        }

        public async Task<bool> Handle(RemoveAuthorCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Retrieving author with Id {request.Id}");
            var author =await  _authorRepository.GetAuthorByIdAsync(request.Id);
            if (author == null) {
                _logger.LogError($"Author NOT FOUND with ID {request.Id}");
                return false; 
            }
            _logger.LogInformation($"Retrieving author's books with Id {request.Id}");
            var bookList = await _bookAuthorRepository.GetAuthorBooksAsync(author.Id);
            foreach (var book in bookList)
            {
                
                if (await _bookAuthorRepository.GetBookAuthorsCountAsync(book.BookId) <= 1)
                {
                    _logger.LogError($"Can't remove author since the book has only one author");
                    return false;
                }
                   
            }

            _logger.LogInformation($"Removing author with Id {request.Id}");
            await _authorRepository.RemoveAuthorAsync(author);
            return true;
        }
    }
}
