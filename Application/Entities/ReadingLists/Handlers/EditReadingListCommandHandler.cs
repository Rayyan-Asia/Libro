using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTOs;
using Application.Entities.ReadingLists.Commands;
using AutoMapper;
using Domain;
using Infrastructure.Interfaces;
using MediatR;

namespace Application.Entities.ReadingLists.Handlers
{
    public class EditReadingListCommandHandler : IRequestHandler<EditReadingListCommand, ReadingListDto>
    {
        private readonly IReadingListRepository _readingListRepository;
        private readonly IBookRepository _bookRepository;
        private readonly IReadingListBookRepository _readingListBookRepository;
        private readonly IMapper _mapper;

        public EditReadingListCommandHandler(IReadingListRepository readingListRepository, IBookRepository bookRepository,
            IReadingListBookRepository readingListBookRepository, IMapper mapper)
        {
            _readingListRepository = readingListRepository;
            _bookRepository = bookRepository;
            _readingListBookRepository = readingListBookRepository;
            _mapper = mapper;
        }

        public async Task<ReadingListDto> Handle(EditReadingListCommand request, CancellationToken cancellationToken)
        {
            var readingList = await _readingListRepository.GetReadingListAsync(request.Id);
            if (readingList == null) return null;
            if (request.UserId != readingList.UserId)
                return null;

            await _readingListBookRepository.RemoveBooksFromReadingList(readingList.Id);

            if (request.Books.Count() < 1) return null;
            foreach (var Book in request.Books)
            {
                var book = await _bookRepository.GetBookByIdAsync(Book.Id);
                if (book == null)
                    return null;
                readingList.Books.Add(book);
            }



            readingList.CreationDate = DateTime.Now;
            readingList.Name = request.Name;
            readingList.UserId = request.UserId;
            readingList.Description = request.Description;
     

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
