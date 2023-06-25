﻿using System;
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

namespace Application.Entities.ReadingLists.Handlers
{
    public class RemoveBookFromReadingListCommandHandler : IRequestHandler<RemoveBookFromReadingListCommand, ReadingListDto>
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

        public async Task<ReadingListDto> Handle(RemoveBookFromReadingListCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Retrieving reading list with ID {request.ReadingListId}");
            var readingList = await _readingListRepository.GetReadingListIncludingCollectionsAsync(request.ReadingListId);
            if (readingList == null)
            {
                _logger.LogError($"Reading list NOT FOUND with ID {request.ReadingListId}");
                return null;
            }

            if (readingList.UserId != request.UserId) {
                _logger.LogError($"User is not the owner of reading list with ID {request.ReadingListId}");
                return null; 
            }   
            _logger.LogInformation($"Retrieving book with ID {request.BookId}");
            var book = await _bookRepository.GetBookByIdAsync(request.BookId);
            if (book == null) 
            {
                _logger.LogError($"Book NOT FOUND with ID {request.BookId}");
                return null; 
            }
            if (!readingList.Books.Any(b => b.Id == book.Id)) return null;
            if (readingList.Books.Count < 2) return null;
            readingList.Books.Remove(book);
            _logger.LogInformation($"Updating reading list with Id {readingList.Id}");
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
