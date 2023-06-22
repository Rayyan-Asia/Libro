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

namespace Application.Entities.ReadingLists.Handlers
{
    public class RemoveBookFromReadingListCommandHandler : IRequestHandler<RemoveBookFromReadingListCommand, ReadingListDto>
    {
        private readonly IBookRepository _bookRepository;
        private readonly IReadingListRepository _readingListRepository;
        private readonly IMapper _mapper;

        public RemoveBookFromReadingListCommandHandler(IBookRepository bookRepository, IReadingListRepository readingListRepository, IMapper mapper)
        {
            _bookRepository = bookRepository;
            _readingListRepository = readingListRepository;
            _mapper = mapper;
        }

        public async Task<ReadingListDto> Handle(RemoveBookFromReadingListCommand request, CancellationToken cancellationToken)
        {
            var readingList = await _readingListRepository.GetReadingListIncludingCollectionsAsync(request.ReadingListId);
            if (readingList == null) return null;
            if (readingList.UserId != request.UserId) return null;
            var book = await _bookRepository.GetBookByIdAsync(request.BookId);
            if (book == null) return null;
            if (!readingList.Books.Any(b => b.Id == book.Id)) return null;
            if (readingList.Books.Count < 2) return null;
            readingList.Books.Remove(book);
            readingList = await _readingListRepository.UpdateReadingListAsync(readingList);
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
