using Application.DTOs;
using Application.Entities.ReadingLists.Commands;
using AutoMapper;
using Domain;
using Application.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;

namespace Application.Entities.ReadingLists.Handlers
{
    public class AddBookToReadingListCommandHandler : IRequestHandler<AddBookToReadingListCommand, IActionResult>
    {
        private readonly IBookRepository _bookRepository;
        private readonly IReadingListRepository _readingListRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<AddBookToReadingListCommandHandler> _logger;

        public AddBookToReadingListCommandHandler(IBookRepository bookRepository, IReadingListRepository readingListRepository, IMapper mapper, ILogger<AddBookToReadingListCommandHandler> logger)
        {
            _bookRepository = bookRepository;
            _readingListRepository = readingListRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IActionResult> Handle(AddBookToReadingListCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Retrieving reading list with ID {request.ReadingListId}");
            var readingList = await _readingListRepository.GetReadingListIncludingCollectionsAsync(request.ReadingListId);
            if (readingList == null)
            {
                _logger.LogError($"Reading list NOT FOUND with ID {request.ReadingListId}");
                return new NotFoundObjectResult($"Reading list NOT FOUND with ID {request.ReadingListId}"); // Return a 404 Not Found response
            }

            if (readingList.UserId != request.UserId)
            {
                _logger.LogError($"User does not own reading list with ID {request.ReadingListId}");
                return new ForbidResult($"User does not own reading list with ID {request.ReadingListId}"); // Return a 401 Unauthorized response
            }

            _logger.LogInformation($"Retrieving book with ID {request.BookId}");
            var book = await _bookRepository.GetBookByIdAsync(request.BookId);
            if (book == null)
            {
                _logger.LogError($"Book NOT FOUND with ID {request.BookId}");
                return new NotFoundObjectResult($"Book NOT FOUND with ID {request.BookId}"); // Return a 404 Not Found response
            }

            if (readingList.Books.Any(b => b.Id == book.Id))
            {
                _logger.LogInformation($"Book with ID {book.Id} already exists in the reading list.");
                return new NoContentResult(); // Return a 204 No Content response
            }

            readingList.Books.Add(book);
            _logger.LogInformation($"Updating reading list with Id {readingList.Id} with book {book.Id}");
            readingList = await _readingListRepository.UpdateReadingListAsync(readingList);
            readingList.Books = readingList.Books.Select(b => new Book
            {
                Id = b.Id,
                Title = b.Title,
                Description = b.Description,
                PublicationDate = b.PublicationDate,
            }).ToList();

            return new OkObjectResult(_mapper.Map<ReadingListDto>(readingList)); // Return a 200 OK response with the updated ReadingListDto
        }
    }
}
