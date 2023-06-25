using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTOs;
using Application.Entities.Books.Commands;
using Application.Entities.ReadingLists.Queries;
using Application.Interfaces;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Entities.ReadingLists.Handlers
{
    public class BrowseReadingListsQueryHandler : IRequestHandler<BrowseReadingListsQuery, (PaginationMetadata, List<ReadingListDto>)>
    {
        private readonly IReadingListRepository _readingListRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<BrowseReadingListsQueryHandler> _logger;

        public BrowseReadingListsQueryHandler(IReadingListRepository readingListRepository, IMapper mapper, ILogger<BrowseReadingListsQueryHandler> logger)
        {
            _readingListRepository = readingListRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async  Task<(PaginationMetadata, List<ReadingListDto>)> Handle(BrowseReadingListsQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Retrieving reading lists for user with ID {request.UserId}");
            var (paginationMetadata, readingLists) = await _readingListRepository.GetReadingListsAsync(request.pageNumber, request.pageSize,request.UserId);
            var dtoList = _mapper.Map<List<ReadingListDto>>(readingLists);
            return (paginationMetadata, dtoList);
        }
    }
}
