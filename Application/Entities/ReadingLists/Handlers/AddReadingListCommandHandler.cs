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
    public class AddReadingListCommandHandler : IRequestHandler<AddReadingListCommand, IActionResult>
    {
        private readonly IReadingListRepository _readingListRepository;
        private readonly IBookRepository _bookRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<AddReadingListCommandHandler> _logger;

        public AddReadingListCommandHandler(IReadingListRepository readingListRepository, IBookRepository bookRepository, IMapper mapper,ILogger<AddReadingListCommandHandler> logger)
        {
            _readingListRepository = readingListRepository;
            _bookRepository = bookRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IActionResult> Handle(AddReadingListCommand request, CancellationToken cancellationToken)
        {
            var readingList = new ReadingList()
            {
                CreationDate = DateTime.Now,
                Name = request.Name,
                UserId = request.UserId,
                Description = request.Description,
            };

            foreach (var bookId in request.Books)
            {
                _logger.LogInformation($"Retrieving book with ID {bookId.Id}");
                var book = await _bookRepository.GetBookByIdAsync(bookId.Id);
                if (book == null)
                {
                    _logger.LogError($"Book NOT FOUND with ID {bookId.Id}");
                    return new NotFoundObjectResult("Book not found with ID " + bookId.Id);
                }
                readingList.Books.Add(book);
            }

            _logger.LogInformation($"Creating reading list {readingList.Name}");
            readingList = await _readingListRepository.AddReadingListAsync(readingList);

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
