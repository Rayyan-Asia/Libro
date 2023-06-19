using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTOs;
using Application.Entities.Books.Commands;
using Application.Entities.ReadingLists.Queries;
using AutoMapper;
using Infrastructure;
using Infrastructure.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Application.Entities.ReadingLists.Handlers
{
    public class BrowseReadingListsQueryHandler : IRequestHandler<BrowseReadingListsQuery, (PaginationMetadata, List<ReadingListDto>)>
    {
        private readonly IReadingListRepository _readingListRepository;
        private readonly IMapper _mapper;

        public BrowseReadingListsQueryHandler(IReadingListRepository readingListRepository, IMapper mapper)
        {
            _readingListRepository = readingListRepository;
            _mapper = mapper;
        }

        public async  Task<(PaginationMetadata, List<ReadingListDto>)> Handle(BrowseReadingListsQuery request, CancellationToken cancellationToken)
        {
            var (paginationMetadata, readingLists) = await _readingListRepository.GetReadingListsAsync(request.pageNumber, request.pageSize,request.UserId);
            var dtoList = _mapper.Map<List<ReadingListDto>>(readingLists);
            return (paginationMetadata, dtoList);
        }
    }
}
