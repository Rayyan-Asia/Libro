using Application.DTOs;
using Application.Entities.ReadingLists.Commands;
using AutoMapper;
using Domain;
using Application.Interfaces;
using MediatR;

namespace Application.Entities.ReadingLists.Handlers
{
    public class AddReadingListCommandHandler : IRequestHandler<AddReadingListCommand, ReadingListDto>
    {
        private readonly IReadingListRepository _readingListRepository;
        private readonly IBookRepository _bookRepository;
        private readonly IMapper _mapper;

        public AddReadingListCommandHandler(IReadingListRepository readingListRepository, IBookRepository bookRepository, IMapper mapper)
        {
            _readingListRepository = readingListRepository;
            _bookRepository = bookRepository;
            _mapper = mapper;
        }

        public async Task<ReadingListDto> Handle(AddReadingListCommand request, CancellationToken cancellationToken)
        {
            var readingList = new ReadingList()
            {
                CreationDate = DateTime.Now,
                Name = request.Name,
                UserId = request.UserId,
                Description = request.Description,
            };
            foreach(var bookId in request.Books)
            {
                var book = await _bookRepository.GetBookByIdAsync(bookId.Id);
                if (book == null)
                    return null; 
                readingList.Books.Add(book);
            }

            readingList = await _readingListRepository.AddReadingListAsync(readingList);

            readingList.Books = readingList.Books.Select(b => new Book
            {
                Id = b.Id,
                Title = b.Title,
                Description = b.Description,
                PublicationDate = b.PublicationDate,
            }).ToList();

            return _mapper.Map<ReadingListDto>(readingList);
        }
    }
}
