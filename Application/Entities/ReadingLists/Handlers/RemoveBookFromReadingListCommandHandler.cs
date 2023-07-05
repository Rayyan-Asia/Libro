using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    public class RemoveBookFromReadingListCommandHandler : IRequestHandler<RemoveBookFromReadingListCommand, IActionResult>
    {
        private readonly IBookRepository _bookRepository;
        private readonly IReadingListRepository _readingListRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<RemoveBookFromReadingListCommandHandler> _logger;

        public RemoveBookFromReadingListCommandHandler(IBookRepository bookRepository, IReadingListRepository readingListRepository, IMapper mapper, ILogger<RemoveBookFromReadingListCommandHandler> logger)
        {
            _bookRepository = bookRepository;
            _readingListRepository = readingListRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IActionResult> Handle(RemoveBookFromReadingListCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Retrieving reading list with ID {request.ReadingListId}");
            var readingList = await _readingListRepository.GetReadingListIncludingCollectionsAsync(request.ReadingListId);
            if (readingList == null)
            {
                _logger.LogError($"Reading list NOT FOUND with ID {request.ReadingListId}");
                return new NotFoundObjectResult("Reading list not found with ID " + request.ReadingListId);
            }

            if (readingList.UserId != request.UserId)
            {
                _logger.LogError($"User is not the owner of reading list with ID {request.ReadingListId}");
                return new BadRequestObjectResult($"User is not the owner of reading list with ID {request.ReadingListId}"); // Or you can return a ForbiddenResult
            }

            _logger.LogInformation($"Retrieving book with ID {request.BookId}");
            var book = await _bookRepository.GetBookByIdAsync(request.BookId);
            if (book == null)
            {
                _logger.LogError($"Book NOT FOUND with ID {request.BookId}");
                return new NotFoundObjectResult("Book not found with ID " + request.BookId);
            }

            if (!readingList.Books.Any(b => b.Id == book.Id))
                return new BadRequestObjectResult("The book is not in the reading list.");
            if (readingList.Books.Count < 2)
                return new BadRequestObjectResult("At least one book should be left in the reading list.");

            readingList.Books.Remove(book);

            _logger.LogInformation($"Updating reading list with ID {readingList.Id}");
            readingList = await _readingListRepository.UpdateReadingListAsync(readingList);

            readingList.Books = readingList.Books.Select(b => new Book
            {
                Id = b.Id,
                Title = b.Title,
                Description = b.Description,
                PublicationDate = b.PublicationDate,
            }).ToList();

            var readingListDto = _mapper.Map<ReadingListDto>(readingList);
            return new OkObjectResult(readingListDto);
        }
    }
}
