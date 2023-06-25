﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Application.Entities.ReadingLists.Commands;
using Application.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Entities.ReadingLists.Handlers
{
    public class RemoveReadingListCommandHandler : IRequestHandler<RemoveReadingListCommand, bool>
    {
        private readonly IReadingListRepository _readingListRepository;
        private readonly ILogger<RemoveReadingListCommandHandler> _logger;

        public RemoveReadingListCommandHandler(IReadingListRepository readingListRepository, ILogger<RemoveReadingListCommandHandler> logger)
        {
            _readingListRepository = readingListRepository;
            _logger = logger;
        }

        public async Task<bool> Handle(RemoveReadingListCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Retrieving reading list with ID {request.ReadingListId}");
            var readingList = await _readingListRepository.GetReadingListAsync(request.ReadingListId);
            if (readingList == null) {
                _logger.LogError($"Reading List NOT FOUND with ID {request.ReadingListId}");
                return false;
            }
            if (readingList.UserId != request.UserId)
            {
                _logger.LogError($"User is NOT THE OWNER  of the reading list with ID {readingList.Id}");
                return false;
            }
            _logger.LogInformation($"Removing reading list with ID {readingList.Id}");
            await _readingListRepository.RemoveReadingListAsync(readingList);
            return true;
        }
    }
}
