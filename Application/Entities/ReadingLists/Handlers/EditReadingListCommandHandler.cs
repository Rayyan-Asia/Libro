using Application.DTOs;
using Application.Entities.ReadingLists.Commands;
using Application.Interfaces;
using AutoMapper;
using Domain;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Application.Entities.ReadingLists.Handlers
{
    public class EditReadingListCommandHandler : IRequestHandler<EditReadingListCommand, IActionResult>
    {
        private readonly IReadingListRepository _readingListRepository;
        private readonly IBookRepository _bookRepository;
        private readonly IReadingListBookRepository _readingListBookRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<EditReadingListCommandHandler> _logger;

        public EditReadingListCommandHandler(IReadingListRepository readingListRepository, IBookRepository bookRepository,
            IReadingListBookRepository readingListBookRepository, IMapper mapper, ILogger<EditReadingListCommandHandler> logger)
        {
            _readingListRepository = readingListRepository;
            _bookRepository = bookRepository;
            _readingListBookRepository = readingListBookRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IActionResult> Handle(EditReadingListCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Retrieving reading list with ID {request.Id}");
            var readingList = await _readingListRepository.GetReadingListAsync(request.Id);
            if (readingList == null)
            {
                _logger.LogError($"Reading list NOT FOUND with ID {request.Id}");
                return new NotFoundObjectResult("Reading list not found with ID " + request.Id);
            }
            if (request.UserId != readingList.UserId)
            {
                _logger.LogError($"User does not own reading list with ID {request.Id}");
                return new ForbidResult($"User does not own reading list with ID {request.Id}"); // Or you can return a ForbiddenResult
            }

            _logger.LogInformation($"Clearing books from reading list with ID {readingList.Id}");
            await _readingListBookRepository.RemoveBooksFromReadingListAsync(readingList.Id);

            if (request.Books.Count() < 1)
                return new BadRequestObjectResult("At least one book should be provided.");

            foreach (var Book in request.Books)
            {
                _logger.LogInformation($"Retrieving book with ID {Book.Id}");
                var book = await _bookRepository.GetBookByIdAsync(Book.Id);
                if (book == null)
                {
                    _logger.LogError($"Book NOT FOUND with ID {Book.Id}");
                    return new NotFoundObjectResult("Book not found with ID " + Book.Id);
                }
                readingList.Books.Add(book);
            }

            readingList.CreationDate = DateTime.Now;
            readingList.Name = request.Name;
            readingList.UserId = request.UserId;
            readingList.Description = request.Description;

            _logger.LogInformation($"Updating reading list with ID {readingList.Id}");
            readingList = await _readingListRepository.UpdateReadingListAsync(readingList);

            readingList.Books = readingList.Books.Select(b => new Book
            {
                Id = b.Id,
                Title = b.Title,
                Description = b.Description,
                PublicationDate = b.PublicationDate,
            }).ToList();

            return new OkObjectResult(_mapper.Map<ReadingListDto>(readingList));
        }

    }
}
